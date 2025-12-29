import { useContext, useEffect, useRef, useState } from 'react'
import axios from 'axios'
import { Tooltip } from '@mantine/core'

import { Form } from '@/components/Form'
import { useJob } from '@/data/Job'

import { EntityContext } from '../contexts/EntityContext'
import { useEntity } from '../hooks/useEntity'

import { useAuth } from '@/contexts/AuthContext'

const annotation_types = [
  {
    label: `Count`,
    id: `count`,
    enabled: true,
  },
  {
    label: `Auto Count`,
    id: `auto-count`,
    enabled: false,
  },
  {
    label: `Linear`,
    id: `linear`,
    enabled: true,
  },
  {
    label: `Linear with Drops`,
    id: `linear-with-drops`,
    enabled: false,
  },
  // {
  //   label: `Count by Distance`,
  //   id: `count-by-distance`,
  //   enabled: true,
  // },
  {
    label: `Area / Volume`,
    id: `area-or-volume`,
    enabled: true,
  },
]

export const AnnotationModal = (props: any) => {
  const { entity, onSave, onCancel } = props

  const { set_selected_entity }: any = useContext(EntityContext)

  const { reloadJob } = useJob()

  //

  const { user } = useAuth()
  const auto_count_enabled = user?.member?.feature_auto_count_enabled ?? false
  const linear_with_drops_enabled =
    user?.member?.feature_linear_with_drops_enabled ?? false
  console.log(`debug.annotations`, {
    user,
    auto_count_enabled,
    linear_with_drops_enabled,
  })

  for (const annotation_type of annotation_types) {
    if (annotation_type.id === `auto-count`) {
      annotation_type.enabled = auto_count_enabled
    }
    if (annotation_type.id === `linear-with-drops`) {
      annotation_type.enabled = linear_with_drops_enabled
    }
  }

  //

  const [symbols, set_symbols] = useState<any>([])

  useEffect(() => {
    const get = async () => {
      const response = await axios.get(`/api/symbols`)
      set_symbols(response.data.data)
    }

    get()
  }, [])

  //

  const {
    annotation_type,
    set_annotation_type,

    annotation_symbol_id,
    set_annotation_symbol_id,

    annotation_color,
    set_annotation_color,

    annotation_length,
    set_annotation_length,
  } = useEntity(entity)

  //

  const [annotation_image, set_annotation_image] = useState<any>(null)

  useEffect(() => {
    const symbol = symbols.find(
      (symbol: any) => symbol.id == annotation_symbol_id,
    )

    console.log(`debug.symbols`, symbol)

    const data = symbol?.data

    if (!data) {
      set_annotation_image(null)
      return
    }

    const xml = data
      .replaceAll(`currentColor`, annotation_color)
      .replaceAll(`currentcolor`, annotation_color)

    const encoded = btoa(unescape(encodeURIComponent(xml)))
    const image = `data:image/svg+xml;base64,${encoded}`

    set_annotation_image(image)
  }, [symbols, annotation_symbol_id, annotation_color])

  //

  const can_save = () => {
    if (annotation_type === `count`) {
      if (!annotation_image) return false
    }

    return true
  }

  const handle_save = async () => {
    const payload = {
      annotation_type,
      annotation_symbol_id,
      annotation_color,
      annotation_length,
    }

    if (annotation_type !== `count`) {
      payload.annotation_symbol_id = null
    }

    if (annotation_type !== `linear-with-drops`) {
      payload.annotation_length = null
    }

    console.log(`debug.ost`, `update_entity`, payload)

    const response = await axios.patch(
      `/api/jobs/${entity.plural}/${entity.id}/partial`,
      payload,
    )
    console.log(`debug.ost`, `update_entity`, response.data)

    await reloadJob()

    //

    set_selected_entity({
      annotation: {
        color: annotation_color,
        image: annotation_image,
        length: annotation_length,
        type: annotation_type,
      },
      entity: {
        id: entity.id,
        type: entity.singular,
        uuid: entity.uuid,
      },
    })

    //

    onSave()
  }

  //

  const background_ref = useRef<HTMLDivElement>(null)
  const modal_ref = useRef<HTMLDivElement>(null)

  useEffect(() => {
    const handle_click = (event: any) => {
      console.log(`debug.modal`, event)
      if (!background_ref.current) {
        return
      }
      if (!modal_ref.current) {
        return
      }
      // click happened outside of the background
      if (!background_ref.current.contains(event.target)) {
        return
      }
      // click happened outside of the modal, within the background
      if (!modal_ref.current.contains(event.target)) {
        onCancel()
      }
    }

    document.addEventListener(`click`, handle_click)

    return () => {
      document.removeEventListener(`click`, handle_click)
    }
  }, [])

  //

  return (
    <div
      className="fixed inset-0 z-[999] bg-gray-500/50"
      ref={background_ref}
    >
      <div
        className="absolute left-1/2 top-1/2 w-1/2 -translate-x-1/2 -translate-y-1/2 rounded-lg bg-white p-8"
        ref={modal_ref}
      >
        <div className="space-y-2">
          <div className="font-semibold">Annotation Type:</div>
          <div>
            {annotation_types.map((annotation: any, index: number) => (
              <div key={index}>
                {annotation.enabled && (
                  <label className="flex cursor-pointer space-x-2">
                    <input
                      checked={annotation.id === annotation_type}
                      name="annotation_type"
                      onChange={(event) =>
                        set_annotation_type(event.target.value)
                      }
                      type={`radio`}
                      value={annotation.id}
                    />
                    <span>{annotation.label}</span>
                  </label>
                )}
              </div>
            ))}
          </div>
        </div>
        {annotation_type === `count` ? (
          <div>
            <br />
            <div className="space-y-2">
              <div className="font-semibold">Annotation Symbol:</div>
              <div className="flex flex-wrap">
                {symbols.map((symbol: any, index: any) => {
                  const checked = symbol.id == annotation_symbol_id
                  const color = annotation_color ?? `#000000`
                  return (
                    <label
                      className="cursor-pointer"
                      key={index}
                      style={{ color }}
                    >
                      <input
                        checked={checked}
                        className="hidden"
                        name="annotation_symbol_id"
                        onChange={(event) =>
                          set_annotation_symbol_id(event.target.value)
                        }
                        type={`radio`}
                        value={symbol.id}
                      />
                      <Symbol
                        {...symbol}
                        checked={checked}
                      />
                    </label>
                  )
                })}
              </div>
              <div key={annotation_symbol_id}>
                {annotation_image && (
                  <img
                    src={annotation_image}
                    height={48}
                    width={48}
                  />
                )}
              </div>
            </div>
          </div>
        ) : null}
        {annotation_type === `linear-with-drops` ? (
          <div>
            <br />
            <div className="space-y-2">
              <div className="font-semibold">Default Drop Length (ft):</div>
              <Form.Input
                name="annotation_length"
                onChange={(event: any) =>
                  set_annotation_length(event.target.value)
                }
                type="number"
                value={annotation_length}
              />
            </div>
          </div>
        ) : null}
        <br />
        <div className="space-y-2">
          <div className="font-semibold">Annotation Color:</div>
          <div>
            <input
              name="annotation_color"
              onChange={(event) => set_annotation_color(event.target.value)}
              type={`color`}
              value={annotation_color}
            />
          </div>
        </div>
        <br />
        <hr />
        <br />
        <div
          className={`${can_save() ? `` : `pointer-events-none opacity-50`}`}
        >
          <Form.Button onClick={handle_save}>Save</Form.Button>
        </div>
      </div>
    </div>
  )
}

const Symbol = (props: any) => {
  console.log(`debug.symbols`, props)

  const { asset, checked, data, name } = props

  if (asset) return null

  return (
    <div
      className={`flex h-12 w-12 items-center justify-center ${checked ? `border border-gray-500` : ``}`}
    >
      <Tooltip
        arrowSize={4}
        label={name}
        withArrow
      >
        <div dangerouslySetInnerHTML={{ __html: data }} />
      </Tooltip>
    </div>
  )
}
