// TODO: auto-count

import { range } from 'lodash'
import { useEffect, useRef, useState } from 'react'
import toast from 'react-hot-toast'

const ZOOM = 2
const INITIAL_SCALE = 1 / ZOOM
const ANGLES_STEP = 10

export const useAutoCount = (instanceRef: any, selected_entity: any) => {
  const scope = `debug.test`

  const instance = instanceRef.current
  console.log(scope, `init`, { instance, selected_entity })

  //

  const [total_pages, set_total_pages] = useState(0)

  //

  const customToolSystemName = `CustomToolAutoCountSelect`

  const [selectionRect, setSelectionRect] = useState<any>(null)

  // with old
  const defaultPX = 72
  const scanPX = 92 * 2

  // const defaultPX = 1
  // const scanPX = 1

  //

  // let coordinatesResult: any
  const url: any = `https://opencv.electric-ease.com/match`

  const get_crop_from_selection = (input: any) => {
    const { x, y, width, height } = input

    const { Core } = instance
    const { documentViewer } = Core

    const currentPage = documentViewer.getCurrentPage()
    const document = documentViewer.getDocument()
    const rotation = (document.getPageRotation(currentPage) / 90) % 4
    const currentPageWidth = documentViewer.getPageWidth(currentPage)
    const currentPageHeight = documentViewer.getPageHeight(currentPage)

    //

    const page = {
      width: currentPageWidth,
      height: currentPageHeight,
    }

    const scale = {
      x: 1,
      y: 1,
    }

    const item = page_blobs.current.get(currentPage)

    const image = {
      width: item?.val?.width ?? 0,
      height: item?.val?.height ?? 0,
    }

    console.log(`debug.test`, `get_crop_from_selection`, input, {
      page,
      image,
      scale,
      rotation,
    })

    //

    const output = {
      x,
      y,
      width,
      height,
    }

    if (rotation === 3) {
      const tempWidth = page.width
      const tempHeight = page.height

      page.width = tempHeight
      page.height = tempWidth

      const tempX = input.x
      const tempY = input.y
      const tempWidthSelection = input.width
      const tempHeightSelection = input.height

      output.x = tempY
      output.y = page.height - tempWidthSelection - tempX
      output.width = tempHeightSelection
      output.height = tempWidthSelection
    } else if (rotation === 1) {
      const tempWidth = page.width
      const tempHeight = page.height

      page.width = tempHeight
      page.height = tempWidth

      const tempX = input.x
      const tempY = input.y
      const tempWidthSelection = input.width
      const tempHeightSelection = input.height

      output.x = page.width - tempHeightSelection - tempY
      output.y = tempX
      output.width = tempHeightSelection
      output.height = tempWidthSelection
    }

    //

    if (image.width && image.height) {
      scale.x = image.width / page.width
      scale.y = image.height / page.height
    }

    output.x *= scale.x
    output.y *= scale.y
    output.width *= scale.x
    output.height *= scale.y

    //

    console.log(`debug.test`, `get_crop_from_selection`, output, {
      page,
      image,
      scale,
    })

    return output
  }

  // all but x works
  const get_selection_from_crop = (input: any) => {
    const { x, y, width, height } = input

    const { Core } = instance
    const { documentViewer } = Core

    const currentPage = documentViewer.getCurrentPage()
    const document = documentViewer.getDocument()
    const rotation = (document.getPageRotation(currentPage) / 90) % 4
    const currentPageWidth = documentViewer.getPageWidth(currentPage)
    const currentPageHeight = documentViewer.getPageHeight(currentPage)

    //

    const page = {
      width: currentPageWidth,
      height: currentPageHeight,
    }

    const scale = {
      x: 1,
      y: 1,
    }

    const item = page_blobs.current.get(currentPage)

    const image = {
      width: item?.val?.width ?? 0,
      height: item?.val?.height ?? 0,
    }

    console.log(`debug.test`, `get_selection_from_crop`, input, {
      page,
      image,
      scale,
    })

    //

    const output = {
      x,
      y,
      width,
      height,
    }

    if (rotation === 3) {
      const tempWidth = page.width
      const tempHeight = page.height

      page.width = tempHeight
      page.height = tempWidth

      const tempX = input.x
      const tempY = input.y
      const tempWidthSelection = input.width
      const tempHeightSelection = input.height

      output.x = image.height - tempHeightSelection - tempY
      output.y = tempX
      output.width = tempHeightSelection
      output.height = tempWidthSelection
    } else if (rotation === 1) {
      const tempWidth = page.width
      const tempHeight = page.height

      page.width = tempHeight
      page.height = tempWidth

      const tempX = input.x
      const tempY = input.y
      const tempWidthSelection = input.width
      const tempHeightSelection = input.height

      output.x = tempY
      output.y = image.width - tempWidthSelection - tempX
      output.width = tempHeightSelection
      output.height = tempWidthSelection
    }

    //

    if (image.width && image.height) {
      scale.x = image.width / page.width
      scale.y = image.height / page.height
    }

    output.x /= scale.x
    output.y /= scale.y
    output.width /= scale.x
    output.height /= scale.y

    //

    console.log(`debug.test`, `get_selection_from_crop`, output, {
      page,
      image,
      scale,
    })

    return output
  }

  const get_page = async (page: any) => {
    const { documentViewer } = instance.Core

    const doc = documentViewer.getDocument()
    const pageNumber = page //documentViewer.getPageCount()
    const zoom = ZOOM
    // const pageRotation = (doc.getPageRotation(page) / 90) % 4
    const pageRotation = 0

    console.log(scope, `get_page`, { pageNumber, zoom })

    const load = async () =>
      new Promise((resolve) => {
        doc.loadCanvas({
          pageNumber,
          zoom,
          pageRotation,
          drawComplete: async (thumbnail: any) => {
            resolve(thumbnail.toDataURL())
          },
        })
      })

    const base64: any = await load()

    console.log(scope, `here!!!!`, base64)

    return base64
  }

  //

  const page_blobs = useRef<any>(new Map())

  const get_page_blob = async (index: number) => {
    const scope2 = `get_page_blob`

    console.log(scope, scope2, index)

    if (page_blobs.current.has(index)) {
      console.log(scope, scope2, index, `found in cache`)
      return page_blobs.current.get(index)
    }

    console.log(scope, scope2, index, `not in cache`)

    //

    const base64 = await get_page(index)
    console.log(scope, scope2, index, `base64 generated`)

    //

    return new Promise((resolve) => {
      const img = document.createElement(`img`)
      img.src = base64
      img.onload = (e) => {
        const blob = {
          id: index,
          val: e.target,
        }
        page_blobs.current.set(index, blob)
        console.log(scope, scope2, index, `blob generated`)
        resolve(`done`)
      }
    })
  }

  //

  const delete_annotations = (tool: any = null) => {
    const { annotationManager }: any = instance.Core

    const annotations = get_annotations(tool)

    annotationManager.deleteAnnotations(annotations)
  }

  const get_annotations = (tool: any = null) => {
    const { annotationManager }: any = instance.Core

    const annotations = annotationManager
      .getAnnotationsList()
      .filter((annotation: any) => {
        console.log(scope, annotation)
        if (tool) {
          return annotation.ToolName === tool
        }
        const meta = (() => {
          try {
            const string = annotation.getCustomData(`meta`)
            const json = string ? JSON.parse(string) : {}
            return json
          } catch (error) {
            console.log(scope, error)
          }
        })()
        console.log(scope, {
          meta: meta.entity?.uuid,
          selected_entity: selected_entity.entity?.uuid,
        })
        return meta.entity?.uuid === selected_entity.entity?.uuid
      })

    return annotations
  }

  const get_overlap_percentage = (a: any, b: any) => {
    const intersection = {
      x: Math.max(a.x, b.x),
      y: Math.max(a.y, b.y),
      width: Math.min(a.x + a.width, b.x + b.width) - Math.max(a.x, b.x),
      height: Math.min(a.y + a.height, b.y + b.height) - Math.max(a.y, b.y),
    }

    // if there is no intersection, width or height will be negative
    if (intersection.width > 0 && intersection.height > 0) {
      // calculate areas
      const intersectionArea = intersection.width * intersection.height
      const aArea = a.width * a.height
      const bArea = b.width * b.height

      // calculate overlap percentage relative to smaller area
      const overlapPercentage = intersectionArea / Math.min(aArea, bArea)

      return overlapPercentage
    }

    return 0
  }

  const get_is_overlapped = (annotation: any, existing_annotations: any) => {
    for (const existing_annotation of existing_annotations) {
      const annotation2 = {
        page: existing_annotation.PageNumber,
        x: existing_annotation.X,
        y: existing_annotation.Y,
        width: existing_annotation.Width,
        height: existing_annotation.Height,
      }

      if (annotation.page !== annotation2.page) {
        continue
      }

      const overlapPercentage = get_overlap_percentage(annotation, annotation2)

      if (overlapPercentage > 0.5) {
        console.log(
          `conflict`,
          `Skipping annotation due to high overlap`,
          overlapPercentage,
        )
        return true
      }
    }

    return false
  }

  const reset_selection = () => {
    setTemplate(null)
    setAutoCountTemplate(null)

    setSelectionRect(null)

    delete_annotations(customToolSystemName)
  }

  const auto_count_handle_reset = () => {
    console.log(scope, `auto_count_handle_reset`)

    //

    reset_selection()

    delete_annotations()

    //

    const { documentViewer, Tools }: any = instance.Core

    documentViewer.setToolMode(documentViewer.getTool(Tools.ToolNames.EDIT))
  }

  const [showModal1AC, setShowModal1AC] = useState(false)
  const [showModal2AC, setShowModal2AC] = useState(false)

  const [template, setTemplate] = useState<any>(null)
  const [autoCountTemplate, setAutoCountTemplate] = useState<any>()

  // const [show, setShow] = useState<any>(false)

  const button_2_count = async () => {
    console.log(`debug.tool.auto_count`, `click 222`)
    console.log(`debug.test`, `cropImage`)
    // crop template(symbol) for auto count
    // debugger
    console.log(`debug.opencv`, { selectionRect })

    // if (selectionRect.height < 12 || selectionRect.width < 12) {
    //   toast.error(`Selection is too small.`)
    //   return
    // }

    setShowModal1AC(true)

    const canvas = document.createElement(`canvas`)
    // const scaleX = 1;
    // const scaleY = 1;
    // canvas.width = selectionRect.width
    // canvas.height = selectionRect.height
    // canvas.width = cropImg.width;
    // canvas.height = cropImg.height;
    const ctx: any = canvas.getContext(`2d`)

    // const pixelRatio = 1 //window.devicePixelRatio;
    // canvas.width = selectionRect.width * pixelRatio
    // canvas.height = selectionRect.height * pixelRatio
    // canvas.width = cropImg.width * pixelRatio;
    // canvas.height = cropImg.height * pixelRatio;
    // ctx.setTransform(pixelRatio, 0, 0, pixelRatio, 0, 0)
    // ctx.imageSmoothingQuality = `high`

    const { Core } = instance
    const { documentViewer } = Core

    const currentPage = documentViewer.getCurrentPage()

    await get_page_blob(currentPage)

    const item = page_blobs.current.get(currentPage)

    console.log(scope, `button 2`, currentPage, page_blobs.current, item)

    if (item?.id == currentPage) {
      const doc = documentViewer.getDocument()
      const pageRotation = (doc.getPageRotation(currentPage) / 90) % 4
      console.log(scope, `cropImage`, { pageRotation })
      // if (pageRotation === 3) {
      //   const _currentPageHeight = currentPageWidth
      //   const _currentPageWidth = currentPageHeight
      //   currentPageWidth = _currentPageWidth
      //   currentPageHeight = _currentPageHeight
      // }

      // const scaleX = item?.val?.naturalWidth / item?.val?.width
      // const scaleY = item?.val?.naturalHeight / item?.val?.height

      const crop = get_crop_from_selection(selectionRect)
      const cropX = crop.x
      const cropY = crop.y
      const cropWidth = crop.width
      const cropHeight = crop.height

      //

      console.log(scope, `cropImage`, {
        cropX,
        cropY,
        cropWidth,
        cropHeight,
      })

      canvas.width = cropWidth
      canvas.height = cropHeight

      ctx.drawImage(
        item?.val,
        cropX,
        cropY,
        cropWidth,
        cropHeight,
        0,
        0,
        cropWidth,
        cropHeight,
      )
    }

    // Converting to base64
    const base64Image: any = canvas.toDataURL()
    console.log(scope, `base64`, base64Image)
    setTemplate(base64Image)
  }

  const [resultedImage, setResultedImage] = useState<any>(null)
  const [modal_1_page_loading, set_modal_1_page_loading] = useState(0)
  const [modal_1_pages_to_load, set_modal_1_pages_to_load] = useState(0)

  const handleModal1SubmitAC = async () => {
    const scope2 = `debug.preview.submit`
    console.log(scope, scope2, `handleModal1SubmitAC`)
    // send API request to backend to get coordinates
    // fd.append("image", cropImageSource.src);
    // fd.append("templateImage", template);
    console.log(scope, scope2, `pages`, total_pages)
    setResultedImage(null)
    const modifiedList: any = []
    const templates: any = []

    set_modal_1_pages_to_load(total_pages)

    for (let i = 1; i <= total_pages; i++) {
      await get_page_blob(i)
      const item = page_blobs.current.get(i)
      set_modal_1_page_loading(i)
      console.log(scope, scope2, `item`, item)
      const fd = new FormData()
      fd.append(`image`, item?.val?.src)
      fd.append(`templateImage`, template)
      fd.append(`angles`, range(0, 360, ANGLES_STEP).join(`,`))
      fd.append(`initial_scale`, INITIAL_SCALE.toString())
      fd.append(`max_workers`, `16`)
      fd.append(`scales`, `1`)
      fd.append(`threshold`, `0.66`)
      const header = new Headers()
      header.append(`username`, `user123`)
      header.append(`password`, `123456`)

      const start = performance.now()
      console.log(scope, scope2, `page`, i)

      const markedPoints = await fetch(url, {
        method: `POST`,
        body: fd,
        headers: header,
      })
        .then(async (res: any) => {
          const response = await res.json()

          const sorted = response?.result?.sort((a: any, b: any) => {
            return b.score - a.score
          })

          const autocountResult = sorted?.map((obj: any) => obj)

          console.log(scope, scope2, `page`, i, { autocountResult })

          for (let i = 0; i < autocountResult.length; i++) {
            let score = autocountResult[i].score
            score = score * 100
            score = score.toFixed(2)
            //
            const newVal = {
              id: item.id,
              score,
              val: autocountResult[i].image,
            }
            templates.push(newVal) // Push each item individually
          }

          return sorted
        })
        .catch((err) => {
          setShowModal1AC(false)
          console.log(err)
        })

      const end = performance.now()
      console.log(scope, scope2, `page`, i, end - start)

      console.log(scope, scope2, `page`, i, { markedPoints })

      if (markedPoints) {
        for (let i = 0; i < markedPoints.length; i++) {
          const newVal = {
            id: item.id,
            val: markedPoints[i],
          }
          modifiedList.push(newVal) // Push each item individually
        }
      }

      console.log(scope, scope2, `page`, i, { modifiedList })
    }

    console.log(scope, scope2, `all pages are done`)

    setAutoCountTemplate(templates)
    setResultedImage(modifiedList)
    console.log(scope, scope2, `modifiedList`, modifiedList)
    setShowModal1AC(false)
    //
    set_modal_1_page_loading(0)
    set_modal_1_pages_to_load(0)
    //
    console.log(scope, `templates`, templates)
    if (templates.length > 0) {
      setShowModal2AC(true)
    }
  }

  const hex2rgb = (hex: any) => {
    const r = parseInt(hex.slice(1, 3), 16)
    const g = parseInt(hex.slice(3, 5), 16)
    const b = parseInt(hex.slice(5, 7), 16)

    const { Core } = instance
    const { Annotations } = Core

    return new Annotations.Color(r, g, b)
  }

  const handleModal2SubmitAC = async (coordinateIndex: any) => {
    console.log(scope, `handleModal2SubmitAC`, coordinateIndex)
    setShowModal2AC(false)

    console.log(`debug.tool.auto_count`, `click`)
    const { Core } = instance

    const { Annotations } = Core

    const parsedData = resultedImage
    // console.log(getLocalData)
    // await annotationManager.importAnnotations(planDocAssemblyId?.meta_data);

    const { annotationManager }: any = Core

    const existing_annotations = get_annotations()
    const to_add: any = []

    coordinateIndex.forEach((coordinate: any) => {
      const matchedIndex = parsedData?.findIndex(
        (_: any, index: number) => index === coordinate,
      )
      if (matchedIndex !== -1) {
        const coordinates: any = parsedData[matchedIndex].val
        // add annotations with matching coordinates
        const pageNum = parsedData[matchedIndex].id // documentViewer.getCurrentPage()
        // const pageHeight = documentViewer.getPageHeight(selectedPage)
        // const pageWidth = documentViewer.getPageWidth(selectedPage)

        // const widthPercentage = scanPX / defaultPX

        const changedX2 = coordinates?.coordinates[2] / INITIAL_SCALE
        const changedX = coordinates.coordinates[0] / INITIAL_SCALE
        const changedY = coordinates.coordinates[1] / INITIAL_SCALE
        const changedY2 = coordinates.coordinates[3] / INITIAL_SCALE

        const width = changedX2 - changedX
        const height = changedY2 - changedY

        const annotation = get_selection_from_crop({
          x: changedX,
          y: changedY,
          width,
          height,
        })

        const annotation_with_page = {
          page: pageNum,
          ...annotation,
        }

        if (get_is_overlapped(annotation_with_page, existing_annotations)) {
          return
        }

        to_add.push(annotation_with_page)
      }
    })

    for (const annotation of to_add) {
      const rectangleAnnot = new Annotations.RectangleAnnotation({
        PageNumber: annotation.page,
        X: annotation.x,
        Y: annotation.y,
        Width: annotation.width,
        Height: annotation.height,
        StrokeColor: hex2rgb(selected_entity?.annotation?.color ?? `#000000`),
        StrokeThickness: 1,
        Author: annotationManager.getCurrentUser(),
      })

      annotationManager.addAnnotation(rectangleAnnot)
      annotationManager.redrawAnnotation(rectangleAnnot)
    }

    setResultedImage(null)

    reset_selection()
  }

  const registerAutoCount = () => {
    console.log(`debug.tool`, `auto-count`)

    const { UI, Core } = instance

    const { documentViewer, Tools } = Core

    //

    class CustomAnnotationAutoCount extends Tools.RectangleCreateTool {
      constructor(element: any) {
        super(element)
        // delete this.defaults.StrokeColor
        // delete this.defaults.FillColor
        // delete this.defaults.StrokeThickness
        this.defaults.StrokeColor = hex2rgb(
          selected_entity?.annotation?.color ?? `#000000`,
        )
      }
    }

    // documentViewer.getTool(Tools.ToolNames.AREA_MEASUREMENT).setStyles(() => ({
    //   StrokeColor: hex2rgb(selected_entity?.annotation?.color ?? `#000000`),
    // }))

    instance.UI.unregisterTool(customToolSystemName)

    const customAutoCountTool = new CustomAnnotationAutoCount(documentViewer)

    instance.UI.registerTool({
      toolName: customToolSystemName,
      toolObject: customAutoCountTool,
      buttonImage: `<svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 0 448 512">
      <path d="M88 32h24c8.8 0 16 7.2 16 16s-7.2 16-16 16H88c-30.9 0-56 25.1-56 56v24c0 8.8-7.2 16-16 16s-16-7.2-16-16V120C0 71.4 39.4 32 88 32zM16 192c8.8 0 16 7.2 16 16v96c0 8.8-7.2 16-16 16s-16-7.2-16-16V208c0-8.8 7.2-16 16-16zm416 0c8.8 0 16 7.2 16 16v96c0 8.8-7.2 16-16 16s-16-7.2-16-16V208c0-8.8 7.2-16 16-16zm0-32c-8.8 0-16-7.2-16-16V120c0-30.9-25.1-56-56-56H336c-8.8 0-16-7.2-16-16s7.2-16 16-16h24c48.6 0 88 39.4 88 88v24c0 8.8-7.2 16-16 16zm16 208v24c0 48.6-39.4 88-88 88H336c-8.8 0-16-7.2-16-16s7.2-16 16-16h24c30.9 0 56-25.1 56-56V368c0-8.8 7.2-16 16-16s16 7.2 16 16zM32 368v24c0 30.9 25.1 56 56 56h24c8.8 0 16 7.2 16 16s-7.2 16-16 16H88c-48.6 0-88-39.4-88-88V368c0-8.8 7.2-16 16-16s16 7.2 16 16zM176 480c-8.8 0-16-7.2-16-16s7.2-16 16-16h96c8.8 0 16 7.2 16 16s-7.2 16-16 16H176zM160 48c0-8.8 7.2-16 16-16h96c8.8 0 16 7.2 16 16s-7.2 16-16 16H176c-8.8 0-16-7.2-16-16z"/>
    </svg>`,
      buttonName: `customAutoCountToolButton`,
      tooltip: `Select`,
      // }, null, (annot: any) => annot && annot.isCustomAnnot);
    })

    //

    UI.createToolbarGroup({
      name: `Auto Count`,
      dataElementSuffix: `AutoCount`,
      useDefaultElements: true,
      children: [
        { type: `spacer` },
        {
          type: `toolButton`,
          // dataElement: `ellipseAreaToolGroupButton`,
          toolName: customToolSystemName,
          // onClick: button_1_select,
        },
        {
          type: `actionButton`,
          // toolGroup: "ellipseAreaTools",
          // img: crop_symbol,
          // dataElement: "ellipseAreaToolGroupButton",
          img: `<svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 0 448 512">
            <path d="M306.8 6.3C311.4 2.2 317.3 0 323.4 0c17.2 0 29.2 17.1 23.4 33.3L278.7 224H389c14.9 0 27 12.1 27 27c0 7.8-3.3 15.1-9.1 20.3L141.1 505.8c-4.5 4-10.4 6.2-16.5 6.2c-17.2 0-29.2-17.1-23.5-33.3L169.3 288H57.8C43.6 288 32 276.4 32 262.2c0-7.4 3.2-14.4 8.7-19.3L306.8 6.3zm.5 42.4L74.1 256H192c5.2 0 10.1 2.5 13.1 6.8s3.7 9.7 2 14.6L140.6 463.6 375.8 256H256c-5.2 0-10.1-2.5-13.1-6.8s-3.7-9.7-2-14.6l66.4-186z"/>
          </svg>`,
          title: `Count`,
          onClick: button_2_count,
          isNotClickableSelector: () => {
            return selectionRect === null
          },
        },
        {
          type: `actionButton`,
          img: `<svg xmlns="http://www.w3.org/2000/svg" height="24" viewBox="0 0 512 512">
            <path d="M69.4 210.6C89.8 126.5 165.6 64 256 64c71.1 0 133.1 38.6 166.3 96H368c-8.8 0-16 7.2-16 16s7.2 16 16 16h80.7H464c8.8 0 16-7.2 16-16V80c0-8.8-7.2-16-16-16s-16 7.2-16 16v60.6C408.8 75.5 337.5 32 256 32C149.6 32 60.5 106.2 37.7 205.6C35.5 215.2 43.1 224 53 224c7.9 0 14.6-5.7 16.5-13.4zm373.2 90.9C422.2 385.5 346.4 448 256 448c-71.1 0-133.1-38.6-166.3-96h54.5c8.8 0 16-7.2 16-16s-7.2-16-16-16H63.3 48.2c-8.8 0-16 7.2-16 16v96c0 8.8 7.2 16 16 16s16-7.2 16-16V371.8C103.4 436.6 174.7 480 256 480c106.4 0 195.5-74.2 218.3-173.6c2.2-9.6-5.4-18.4-15.3-18.4c-7.9 0-14.6 5.7-16.5 13.4z"/>
          </svg>`,
          onClick: auto_count_handle_reset,
          title: `Reset`,
          isNotClickableSelector: () => {
            return false
            return selectionRect === null
          },
        },
        { type: `spacer` },
      ],
    })

    UI.setToolbarGroup(`toolbarGroup-AutoCount`)

    documentViewer.setToolMode(documentViewer.getTool(customToolSystemName))

    //

    const total = documentViewer.getPageCount()
    set_total_pages(total)
  }

  const handle_annotation_changed = async (
    annotations: any,
    action: any,
    info: any,
  ) => {
    if (info.imported) return
    console.log(scope, `annotationChanged`, {
      annotations,
      action,
      info,
    })

    for (const annotation of annotations) {
      console.log(scope, annotation.ToolName)

      if (annotation.ToolName !== customToolSystemName) {
        console.log(scope, `skip`)
        continue
      }

      if (action === `add`) {
        console.log(scope, `crop`, annotation.Rotation)

        //
        const { documentViewer } = instance.Core

        const currentPage = documentViewer.getCurrentPage()
        const currentPageWidth = documentViewer.getPageWidth(currentPage)
        const currentPageHeight = documentViewer.getPageHeight(currentPage)

        const doc = documentViewer.getDocument()
        const pageRotation = (doc.getPageRotation(currentPage) / 90) % 4
        console.log(scope, `crop`, { pageRotation })

        // the one that's not working, is 3
        // the ones that ARE working are 0
        // 0: E_0
        // 1: E_90
        // 2: E_180
        // 3: E_270

        const page = {
          width: currentPageWidth,
          height: currentPageHeight,
        }

        const selection = {
          x: annotation.X,
          y: annotation.Y,
          width: annotation.Width,
          height: annotation.Height,
        }

        console.log(scope, `crop.before`, selection, page)

        if (pageRotation === 3) {
          //  page.width = currentPageHeight
          //  page.height = currentPageWidth
          // selection.x = annotation.Y
          // selection.y = annotation.X
          // selection.width = annotation.Height
          // selection.height = annotation.Width
        }

        console.log(scope, `crop.after`, selection, page)

        // selection.x = 0
        // selection.y = 0
        // selection.width = 200
        // selection.height = 200

        //

        setSelectionRect({
          ...selection,
          unit: `px`,
        })
      }
    }
  }

  //

  const on_cancel = () => {
    console.log(scope, `on_cancel`)

    // setSelectionRect(null)
    setTemplate(null)
    setAutoCountTemplate(null)
  }

  //

  useEffect(() => {
    console.log(scope, `instance`, instance)
    if (!instance) return

    const { annotationManager }: any = instance.Core

    annotationManager.addEventListener(
      `annotationChanged`,
      handle_annotation_changed,
    )

    return () => {
      annotationManager.removeEventListener(
        `annotationChanged`,
        handle_annotation_changed,
      )
    }
  }, [instance])

  //

  useEffect(() => {
    const type = selected_entity?.annotation?.type

    console.log(scope, `selected_entity.annotation.type`, type)

    if (type === `auto-count`) {
      registerAutoCount()
    }
  }, [selected_entity, selectionRect])

  return {
    showModal1AC,
    setShowModal1AC,
    handleModal1SubmitAC,
    showModal2AC,
    setShowModal2AC,
    handleModal2SubmitAC,
    template,
    autoCountTemplate,
    on_cancel,
    //
    selectionRect,
    modal_1_page_loading,
    modal_1_pages_to_load,
  }
}
