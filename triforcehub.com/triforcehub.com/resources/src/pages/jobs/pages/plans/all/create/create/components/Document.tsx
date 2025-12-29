import { useEffect, useReducer, useRef, useState } from 'react'

import * as pdfJS from 'pdfjs-dist'
import { PDFDocument, degrees } from 'pdf-lib'

import { Page } from './Page'
import { range, uniq } from 'lodash'
import { Form } from '@/components'

const normalize = (items: number[]) => uniq(items.sort((a, b) => a - b))

export const Document = (props: any) => {
  const scope = `debug.document`

  const { data, on_cancel, on_save } = props

  const pdflib_ref = useRef<any>(null)
  const pdfjs_ref = useRef<any>(null)
  const [number_of_pages, set_number_of_pages] = useState<number>(0)
  const [page_labels, set_page_labels] = useState<any>([])

  //

  const max_items_per_tick = 1

  const [indexes, dispatch] = useReducer(
    (state: any, action: any) => {
      console.log(scope, `reducer`, action)

      if (action.type === `add_all`) {
        if (state.all.includes(action.index)) return state

        const all = normalize([...state.all, action.index])
        const queued = normalize([...state.queued, action.index])

        return {
          ...state,
          all,
          queued,
        }
      }

      if (action.type === `tick`) {
        if (state.queued.length === 0) return state
        if (state.loading.length) return state

        const items = state.queued.slice(0, max_items_per_tick)
        if (items.length === 0) return state

        const loading = normalize([...state.loading, ...items])
        const queued = state.queued.filter((index: number) => !items.includes(index))

        return {
          ...state,
          loading,
          queued,
        }
      }

      if (action.type === `add_loaded`) {
        const loaded = normalize([...state.loaded, action.index])
        const loading = state.loading.filter((index: number) => index !== action.index)
        const queued = state.queued.filter((index: number) => index !== action.index)

        return {
          ...state,
          loaded,
          loading,
          queued,
        }
      }

      if (action.type === `add_selected`) {
        const selected = normalize([...state.selected, action.index])

        return {
          ...state,
          selected,
        }
      }

      if (action.type === `remove_selected`) {
        const selected = state.selected.filter((index: number) => index !== action.index)

        return {
          ...state,
          selected,
        }
      }

      throw `action.type: ${action.type}`
    },
    {
      all: [],
      queued: [],
      loading: [],
      loaded: [],
      selected: [],
    },
  )

  useEffect(() => {
    const items = range(0, number_of_pages)
    for (const item of items) {
      dispatch({ type: `add_all`, index: item })
    }
    dispatch({ type: `tick` })
  }, [number_of_pages])

  //

  useEffect(() => {
    if (!data) return
    ;(async () => {
      console.log(scope, data.detached)
      if (data?.detached) return

      pdflib_ref.current = await PDFDocument.load(data)
      console.log(scope, `meta`, pdflib_ref.current.getAuthor())
      console.log(scope, `meta`, pdflib_ref.current.getCreationDate())
      console.log(scope, `meta`, pdflib_ref.current.getCreator())
      console.log(scope, `meta`, pdflib_ref.current.getKeywords())
      console.log(scope, `meta`, pdflib_ref.current.getModificationDate())
      console.log(scope, `meta`, pdflib_ref.current.getPageCount())
      console.log(scope, `meta`, pdflib_ref.current.getPageIndices())
      console.log(scope, `meta`, pdflib_ref.current.getProducer())
      console.log(scope, `meta`, pdflib_ref.current.getSubject())
      console.log(scope, `meta`, pdflib_ref.current.getTitle())
      const pages = pdflib_ref.current.getPages()
      for (const page of pages) {
        // console.log(scope, `page.before`, page.getRotation())
        page.setRotation(degrees(0))
        // console.log(scope, `page.after`, page.getRotation(), page.getWidth())
      }
      update_iframe()

      //

      const test = await pdflib_ref.current.save()

      //

      const worker = window.Vapor.asset(`pdf.worker.min.mjs`)

      pdfJS.GlobalWorkerOptions.workerSrc = worker

      pdfjs_ref.current = await pdfJS.getDocument({
        data: test,
        worker,
      }).promise

      const _page_labels = await pdfjs_ref.current.getPageLabels()
      set_page_labels(_page_labels)
      console.log(scope, `page labels`, _page_labels)

      console.log(scope, pdfjs_ref.current.numPages)
      set_number_of_pages(pdfjs_ref.current.numPages)
    })()
  }, [data])

  const inspect = async () => {
    const test1 = await pdfjs_ref.current.getDownloadInfo()
    console.log(`debug.pdf`, `download info`, test1)

    const test2 = await pdfjs_ref.current.getData()
    console.log(`debug.pdf`, `data`, test2)

    const test3 = await pdfjs_ref.current.getMetadata()
    console.log(`debug.pdf`, `metadata`, test3)

    const test4 = await pdfjs_ref.current.getPageLabels()
    console.log(`debug.pdf`, `page labels`, test4)
  }

  //

  const handle_save = async () => {
    const original = await pdflib_ref.current.save()

    console.log(scope, `save`, original)

    const clone = await PDFDocument.create()
    console.log(scope, `save`, clone)

    const pages = await clone.copyPages(pdflib_ref.current, indexes.selected)
    for (const page of pages) {
      clone.addPage(page)
    }
    // console.log(scope, `save`, pages)

    console.log(scope, `save.pages`, pdflib_ref.current.getPages(), clone.getPages())
    const data = await clone.save()
    // console.log(scope, `save`, data)

    on_save(data)
  }

  //

  const [iframe_src, set_iframe_src] = useState<string | undefined>()

  const update_iframe = async () => {
    const test = await pdflib_ref.current.save()
    const tempblob = new Blob([test], {
      type: `application/pdf`,
    })
    const docUrl = URL.createObjectURL(tempblob)
    set_iframe_src(docUrl)
  }

  const handle_rotate = async (index: number, deg: number) => {
    pdflib_ref.current.getPage(index).setRotation(degrees(deg))
    const test = await pdflib_ref.current.save()
    const tempblob = new Blob([test], {
      type: `application/pdf`,
    })
    const docUrl = URL.createObjectURL(tempblob)
    set_iframe_src(docUrl)
  }

  //

  const handle_loaded = (index: number) => {
    dispatch({ type: `add_loaded`, index })
    dispatch({ type: `tick` })
  }

  const handle_selected = (index: number) => {
    dispatch({ type: `add_selected`, index })
  }

  const handle_unselected = (index: number) => {
    dispatch({ type: `remove_selected`, index })
  }

  if (indexes.all.length === 0) return

  console.log(scope, `indexes`, { ...indexes })

  //

  return (
    <div>
      {false && (
        <div>
          {JSON.stringify(indexes)}
          <br />
          {JSON.stringify(page_labels)}
          <br />
        </div>
      )}
      {false && (
        <div>
          <button
            onClick={inspect}
            type="button"
          >
            Inspect
          </button>
        </div>
      )}
      <div className="flex gap-4 flex-wrap">
        {indexes.all.map((index: number) => (
          <Page
            index={index}
            is_loading={indexes.loading.includes(index)}
            is_queued={indexes.queued.includes(index)}
            is_selected={indexes.selected.includes(index)}
            key={index}
            n={index + 1}
            on_loaded={handle_loaded}
            on_rotate={handle_rotate}
            on_selected={handle_selected}
            on_unselected={handle_unselected}
            pdfjs_ref={pdfjs_ref}
            pdflib_ref={pdflib_ref}
            pending={indexes.queued.includes(index)}
            title={page_labels?.[index] ?? `Page ${index + 1}`}
          />
        ))}
      </div>
      {false && (
        <div>
          <br />
          <iframe
            className="border border-black h-96"
            src={iframe_src}
          ></iframe>
        </div>
      )}
      <hr className="my-8" />
      <div className={indexes.selected.length === 0 ? `opacity-50 pointer-events-none` : ``}>
        <Form.Buttons>
          <Form.Button
            onClick={on_cancel}
            type={`secondary`}
          >
            Cancel
          </Form.Button>
          <Form.Button
            onClick={handle_save}
            type={`primary`}
          >
            Save
          </Form.Button>
        </Form.Buttons>
      </div>
    </div>
  )
}
