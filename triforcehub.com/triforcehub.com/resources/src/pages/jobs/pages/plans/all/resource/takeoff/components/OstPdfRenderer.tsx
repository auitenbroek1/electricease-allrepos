// @ts-nocheck
import { useEffect, useMemo, useRef, useState } from 'react'
import axios from 'axios'
import debounce from 'lodash/debounce'
import WebViewer from '@pdftron/webviewer'
import { get_quantity_from_units } from '@/helpers/units'

import { useJob } from '@/data/Job'

import { EntityContext } from '../contexts/EntityContext'

import { Takeoff } from './Takeoff'
import { AutoCount } from './AutoCount'

const OstPdfRenderer = (props: any) => {
  const { plan, annotations } = props

  //

  const touched_entity_uuids = useRef(new Set())

  const { reloadJob } = useJob()

  const reloadJob2 = useMemo(
    () =>
      debounce(() => {
        const payload = {
          touched_entity_uuids: Array.from(touched_entity_uuids.current),
        }
        console.log(`debug.ost.update`, {
          plan,
          payload,
        })
        const update = async () => {
          await axios.post(
            `/api/jobs/${plan.job_id}/plans/${plan.id}/update_quantities`,
            payload,
          )
          reloadJob()
        }
        update()
      }, 1000),
    [plan?.id],
  )

  //

  const currentAnnotation = useRef(null)

  const [selected_entity, set_selected_entity]: any = useState<any>(null)
  const entity_context_value = { selected_entity, set_selected_entity }

  const viewer = useRef<any>(null)
  const instanceRef = useRef()

  const get_quantity_from_annotation = (annotation: any, meta: any) => {
    const get_linear_length = (input: any) => {
      const contents = input.getContents()
      console.log(`debug.ost`, `qty`, { contents })
      return get_quantity_from_units(contents)
    }
    if (meta?.annotation?.type === `count`) {
      return 1
    } else if (meta?.annotation?.type === `auto-count`) {
      return 1
    } else if (meta?.annotation?.type === `linear`) {
      const quantity = get_linear_length(annotation)
      console.log(`debug.test`, meta?.annotation, { quantity })
      return quantity
    } else if (meta?.annotation?.type === `linear-with-drops`) {
      let points = 0
      points = annotation?.getPath?.().length ?? 1
      //
      let length = meta?.annotation?.length ?? 1
      length = Number(length)
      //
      const quantity = get_linear_length(annotation)
      //
      console.log(`debug.test`, meta?.annotation, { points, length, quantity })
      return points * length + quantity
    } else if (meta?.annotation?.type === `count-by-distance`) {
      return 1
    } else if (meta?.annotation?.type === `area-or-volume`) {
      let contents = annotation.getContents()
      console.log(`debug.ost`, `qty`, { contents })
      if (contents.includes(` sq ft`)) {
        contents = contents.replace(` sq ft`, ``)
      }
      const quantity = Number(contents)
      return quantity
    }
    return 0
  }

  const storeAnnotation = async (uuid: any, data: any, entity: any) => {
    const payload = {
      uuid,
      job_plan_id: plan.id,
      data,
      entity_uuid: entity?.uuid ?? `00000000-0000-0000-0000-000000000000`,
      entity_quantity: entity?.quantity ?? 0,
    }

    touched_entity_uuids.current.add(payload.entity_uuid)

    console.log(`debug.ost`, `store annotation`, uuid, payload)

    await axios.post(`/api/jobs/annotations`, payload)

    console.log(`debug.ost.reload`)
    reloadJob2()
  }

  const updateAnnotation = async (uuid: any, data: any, entity: any) => {
    const payload = {
      data,
      entity_uuid: entity?.uuid ?? `00000000-0000-0000-0000-000000000000`,
      entity_quantity: entity?.quantity ?? 0,
    }

    touched_entity_uuids.current.add(payload.entity_uuid)

    console.log(`debug.ost`, `update annotation`, uuid, payload)

    await axios.patch(`/api/jobs/annotations/${uuid}`, payload)

    console.log(`debug.ost.reload`)
    reloadJob2()
  }

  const destroy_annotations = async (annotations: any) => {
    console.log(`debug.test.reset`, `destroy_annotations`, annotations)

    const payload = annotations.map((annotation) => {
      return {
        uuid: annotation.uuid,
        entity_uuid:
          annotation.entity?.uuid ?? `00000000-0000-0000-0000-000000000000`,
      }
    })

    for (const item of payload) {
      touched_entity_uuids.current.add(item.entity_uuid)
    }

    await axios.post(`/api/jobs/annotations/bulk/destroy`, payload)

    console.log(`debug.test.reset`, `reload`)
    reloadJob2()
  }

  const import_annotations = async (manager: any) => {
    console.log(`debug.ost`, `import`, annotations.length)
    for (const annotation of annotations) {
      console.log(`debug.ost`, `import`, { annotation })
      manager.importAnnotations(annotation.data)
    }
  }

  //

  let webviewer_lib = window.Vapor.asset(`webviewer/lib`)
  try {
    webviewer_lib = new URL(webviewer_lib)
    webviewer_lib = webviewer_lib.pathname
  } catch (error) {}

  //

  const production = [`3.electric-ease.com`, `app.electric-ease.com`].includes(
    window.location.hostname,
  )

  useEffect(() => {
    WebViewer(
      {
        licenseKey: production
          ? `TriForce Management Applications LLC, DBA Electric Ease:PWS:Electric Ease::B+2:BD3A0CA987166CAFDD24FA5FB8AFAD87B81CB059DB181491C8C17DC640D826BD`
          : null,
        path: webviewer_lib,
        initialDoc: plan.upload.url,
        enableMeasurement: true,
        fullAPI: true,
        autoExpandOutlines: true,
        useDownloader: false,
        streaming: false,
      },
      viewer.current,
    ).then(async (instance: any) => {
      instanceRef.current = instance

      const Feature = instance.UI.Feature

      instance.UI.disableFeatures([
        Feature.ContentEdit,
        Feature.MultipleViewerMerging,
        Feature.OutlineEditing,
        Feature.ThumbnailMerging,
        Feature.ThumbnailReordering,
      ])

      instance.UI.disableElements([
        `toolbarGroup-Edit`,
        `toolbarGroup-FillAndSign`,
        `toolbarGroup-Forms`,
        `documentControl`,
        `thumbnailControl`,
      ])

      const { annotationManager, documentViewer }: any = instance.Core

      documentViewer.addEventListener(`documentLoaded`, () => {
        import_annotations(annotationManager)
      })

      annotationManager.addEventListener(
        `annotationChanged`,
        async (annotations: any, action: any, info: any) => {
          if (info.imported) return
          console.log(`debug.ost`, `annotationChanged`, {
            annotations,
            action,
            info,
          })

          // An annotation is an measurement annotation if it contains a Measure property
          const measurementAnnotations = annotations.filter(
            (annotation) => annotation.Measure,
          )

          measurementAnnotations.forEach((annotation) => {
            console.log(`debug.ost`, annotation.Scale)
            console.log(`debug.ost`, annotation.Precision)
            console.log(`debug.ost`, annotation.Measure)
          })

          const to_delete = []

          for (const annotation of annotations) {
            console.log(`debug.test`, `tool`, annotation.ToolName)

            if (annotation.ToolName === `CustomToolAutoCountSelect`) {
              console.log(`debug.test`, `skip`)
              continue
            }

            const meta = (() => {
              if (action === `add`) {
                const json = {
                  annotation: {
                    uuid: annotation.Id,
                    type: currentAnnotation?.current?.annotation?.type,
                    length: currentAnnotation?.current?.annotation?.length,
                  },
                  entity: currentAnnotation?.current?.entity,
                }
                return json
              } else {
                try {
                  const string = annotation.getCustomData(`meta`)
                  const json = string ? JSON.parse(string) : {}
                  return json
                } catch (error) {
                  console.log(`debug.ost`, error)
                }
              }
              return {}
            })()

            annotation.setCustomData(`meta`, JSON.stringify(meta))

            const data = await annotationManager.exportAnnotations({
              annotationList: [annotation],
            })

            console.log(`debug.ost`, `annotationChanged`, {
              action,
              data,
              meta,
            })

            //

            const quantity = get_quantity_from_annotation(annotation, meta)
            console.log(`debug.ost`, `qty`, {
              action,
              quantity,
            })
            if (meta.entity) meta.entity.quantity = quantity

            //

            if (action === `add`) {
              storeAnnotation(meta.annotation.uuid, data, meta.entity)
            }

            if (action === `modify`) {
              console.log(`debug.ost`, meta)
              if (meta?.annotation?.uuid) {
                updateAnnotation(meta.annotation.uuid, data, meta.entity)
              }
            }

            if (action === `delete`) {
              console.log(`debug.ost`, meta)
              if (meta?.annotation?.uuid) {
                to_delete.push({
                  uuid: meta.annotation.uuid,
                  entity: meta.entity,
                })
              }
            }
          }

          if (to_delete.length) {
            destroy_annotations(to_delete)
          }
        },
      )
    })
  }, [props.fileName])

  //

  const registerManualCountFeature = () => {
    const options = selected_entity

    console.log(`debug.ost`, `registerManualCountFeature`, options)

    const color = options.annotation.color ?? `#000000`
    const image = options.annotation.image ?? ``

    console.log(`debug.ost`, `registerManualCountFeature`, { color, image })

    const { Annotations, annotationManager, documentViewer, Tools } =
      instanceRef.current.Core

    const keepAsSVG = true

    // const stampAnnot = new Annotations.StampAnnotation();
    // stampAnnot.PageNumber = 1;
    // stampAnnot.X = 1000;
    // stampAnnot.Y = 1000;
    // stampAnnot.Width = 100;
    // stampAnnot.Height = 100;
    // stampAnnot.setImageData(image, keepAsSVG);

    // annotationManager.storeAnnotation(stampAnnot);
    // annotationManager.redrawAnnotation(stampAnnot);

    //

    const customAnnotationSystemName = `custom-annotation-manual-count`

    class CustomAnnotationManualCount extends Tools.GenericAnnotationCreateTool {
      constructor(documentViewer) {
        // Inherit generic annotation create tool
        super(documentViewer, Annotations.StampAnnotation)
        delete this.defaults.StrokeColor
        delete this.defaults.FillColor
        delete this.defaults.StrokeThickness
      }

      // Override mouseLeftDown
      mouseLeftDown(...args) {
        Tools.AnnotationSelectTool.prototype.mouseLeftDown.apply(this, args)
      }

      // Override mouseMove
      mouseMove(...args) {
        Tools.AnnotationSelectTool.prototype.mouseMove.apply(this, args)
      }

      // Override mouseLeftUp
      mouseLeftUp(e) {
        super.mouseLeftDown(e)

        let annotation
        if (this.annotation) {
          let width = 30
          let height = 30
          const rotation =
            this.documentViewer.getCompleteRotation(
              this.annotation.PageNumber,
            ) * 90
          this.annotation.Rotation = rotation
          if (rotation === 270 || rotation === 90) {
            const t = height
            height = width
            width = t
          }
          // 'ImageData' can be a bas64 ImageString or an URL. If it's an URL, relative paths will cause issues when downloading
          this.annotation.setImageData(image, keepAsSVG)
          this.annotation.Width = width
          this.annotation.Height = height
          this.annotation.X -= width / 2
          this.annotation.Y -= height / 2
          this.annotation.MaintainAspectRatio = true
          // this.annotation.NoZoom = true;
          annotation = this.annotation
        }

        super.mouseLeftUp(e)

        if (annotation) {
          annotationManager.redrawAnnotation(annotation)
        }
      }
    }

    //

    const customToolSystemName = `custom-tool-manual-count`

    instanceRef.current.UI.unregisterTool(customToolSystemName)

    const customManualCountTool = new CustomAnnotationManualCount(
      documentViewer,
    )

    instanceRef.current.UI.registerTool({
      toolName: customToolSystemName,
      toolObject: customManualCountTool,
      buttonImage: `<svg xmlns="http://www.w3.org/2000/svg" width="24" height="24" viewBox="0 0 24 24" fill="currentColor">
        <path d="M12 7.77L18.39 18H5.61L12 7.77M12 4L2 20h20L12 4z"/>
        <path fill="none" d="M0 0h24v24H0V0z"/>
      </svg>`,
      buttonName: `customManualCountToolButton`,
      tooltip: `Custom Manual Count ${color}`,
      // }, null, (annot: any) => annot && annot.isCustomAnnot);
    })

    console.log(
      `debug.ost`,
      `get tool`,
      documentViewer.getTool(customToolSystemName),
    )

    try {
      documentViewer.setToolMode(documentViewer.getTool(customToolSystemName))
    } catch (error) {
      console.log(`debug.catch`, error)
    }
  }

  const set_tool_mode = (toolbar: string, tool: string) => {
    try {
      setTimeout(() => {
        instanceRef.current.UI.setToolbarGroup(toolbar)
      }, 0)

      setTimeout(() => {
        instanceRef.current.Core.documentViewer.setToolMode(
          instanceRef.current.Core.documentViewer.getTool(tool),
        )
      }, 0)

      setTimeout(() => {
        instanceRef.current.UI.setToolbarGroup(toolbar)
      }, 0)
    } catch (error) {
      console.log(`debug.catch`, error)
    }
  }

  useEffect(() => {
    console.log(`debug.ost`, { selected_entity })

    if (!selected_entity) return

    currentAnnotation.current = selected_entity

    const { documentViewer, Annotations, Tools } = instanceRef.current.Core

    if (selected_entity.annotation.type === `count`) {
      registerManualCountFeature()
      return
    }

    if (selected_entity.annotation.type === `auto-count`) {
      console.log(`debug.test`, `skip`)
      return
    }

    //

    const hex2rgb = (hex) => {
      const r = parseInt(hex.slice(1, 3), 16)
      const g = parseInt(hex.slice(3, 5), 16)
      const b = parseInt(hex.slice(5, 7), 16)

      return new Annotations.Color(r, g, b)
    }

    documentViewer
      .getTool(Tools.ToolNames.PERIMETER_MEASUREMENT)
      .setStyles(() => ({
        StrokeColor: hex2rgb(selected_entity?.annotation?.color ?? `#000000`),
      }))

    documentViewer.getTool(Tools.ToolNames.AREA_MEASUREMENT).setStyles(() => ({
      StrokeColor: hex2rgb(selected_entity?.annotation?.color ?? `#000000`),
    }))

    //

    if (selected_entity.annotation.type === `linear`) {
      set_tool_mode(
        `toolbarGroup-Measure`,
        Tools.ToolNames.PERIMETER_MEASUREMENT,
      )
      return
    }

    if (selected_entity.annotation.type === `linear-with-drops`) {
      set_tool_mode(
        `toolbarGroup-Measure`,
        Tools.ToolNames.PERIMETER_MEASUREMENT,
      )
      return
    }

    if (selected_entity.annotation.type === `count-by-distance`) {
      set_tool_mode(
        `toolbarGroup-Measure`,
        Tools.ToolNames.PERIMETER_MEASUREMENT,
      )
      return
    }

    if (selected_entity.annotation.type === `area-or-volume`) {
      set_tool_mode(`toolbarGroup-Measure`, Tools.ToolNames.AREA_MEASUREMENT)
      return
    }

    documentViewer.setToolMode(documentViewer.getTool(Tools.ToolNames.EDIT))
  }, [selected_entity])

  const [showComments, setShowComments] = useState(false)

  return (
    <EntityContext.Provider value={entity_context_value}>
      <div className="relative">
        <div className="absolute left-0 top-0 w-[300px]">
          <Takeoff />
        </div>
        <div
          className={`${showComments ? `mx-4 px-[300px]` : `ml-4 pl-[300px]`} min-h-screen`}
        >
          <AutoCount instanceRef={instanceRef} />
          <div
            className="webviewer"
            ref={viewer}
          ></div>
        </div>
        {showComments && (
          <div className="absolute right-0 top-0 w-[300px]">notes, tasks</div>
        )}
      </div>
    </EntityContext.Provider>
  )
}

export default OstPdfRenderer
