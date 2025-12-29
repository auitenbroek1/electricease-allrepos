import { useEffect, useRef, useState } from 'react'

import * as pdfJS from 'pdfjs-dist'

import { Preview } from './Preview'

export const Page = (props: any) => {
  const worker = window.Vapor.asset(`pdf.worker.min.mjs`)
  pdfJS.GlobalWorkerOptions.workerSrc = worker

  const scope = `debug.pdf.page`

  const {
    index,
    is_loading,
    is_queued,
    n,
    on_loaded,
    on_rotate,
    on_selected,
    on_unselected,
    pdfjs_ref,
    pdflib_ref,
    title,
  } = props

  const canvasRef = useRef(null)

  const [rotate, set_rotate] = useState<any>(null)

  const is_disabled = is_queued || is_loading

  useEffect(() => {
    if (is_queued) return

    const get_page_to_be_rendered = async () => {
      const time2 = Date.now()
      const page = await pdfjs_ref.current.getPage(n)
      console.log(scope, n, `get page`, Date.now() - time2)

      set_rotate(page.rotate)
      return page
    }

    const render = async () => {
      const page = await get_page_to_be_rendered()

      //

      const max_width = 300
      // const viewport = page.getViewport({ scale: 1 })
      const viewport = page.getViewport({ scale: 1, rotation: 0 })
      console.log(`debug.pdf`, `viewport`, viewport)
      const scale = max_width / viewport.width
      // const scale = 1

      // const scaled_viewport = page.getViewport({ scale: scale })
      const scaled_viewport = page.getViewport({ scale: scale, rotation: 0 })
      // console.log(`debug.pdf`, `scaled_viewport`, scaled_viewport.width)
      const canvas_height = scaled_viewport.height
      const canvas_width = scaled_viewport.width

      //

      // Prepare canvas using PDF page dimensions.
      const canvas: any = canvasRef.current
      const canvasContext = canvas.getContext(`2d`)
      canvas.height = canvas_height
      canvas.width = canvas_width

      // Render PDF page into canvas context.
      const renderContext = { canvasContext, viewport: scaled_viewport }
      // page.render(renderContext).promise.then(() => {
      //   console.log(`debug.pdf`, n, `end`)
      // })

      const time3 = Date.now()
      await page.render(renderContext).promise
      console.log(scope, n, `render page`, Date.now() - time3)

      if (on_loaded) on_loaded(index)
    }

    const timeout = setTimeout(render, 0 * n)

    return () => {
      clearTimeout(timeout)
    }
  }, [is_queued])

  const [css_rotation, set_css_rotation] = useState<string>(`rotate-[0deg]`)

  const test = (deg: any) => {
    let css = `rotate-[0deg]`
    if (deg === 0) css = `rotate-[0deg]`
    if (deg === 90) css = `rotate-[90deg]`
    if (deg === 180) css = `rotate-[180deg]`
    if (deg === 270) css = `rotate-[270deg]`

    return css
  }

  const handle_rotate = (deg: any) => {
    let final = rotate + deg
    if (final > 360) final = 90
    if (final < 0) final = 270

    console.log(scope, `rotate`, rotate + deg, final)

    set_rotate(final)
    on_rotate(index, final)
    const test1 = test(final)
    set_css_rotation(test1)
  }

  const handle_toggle = (event: any) => {
    console.log(scope, `toggle`, event.target.checked, index)

    if (event.target.checked) {
      on_selected(index)
    } else {
      on_unselected(index)
    }
  }

  const [show_preview, set_show_preview] = useState<boolean>(false)

  return (
    <div className={`${is_disabled ? `opacity-50 pointer-events-none` : ``}`}>
      <div className="border border-gray-300 bg-gray-100 rounded p-4 space-y-4">
        <div className="flex gap-4 justify-center">
          <div>
            <button
              onClick={() => handle_rotate(-90)}
              type={`button`}
            >
              <svg
                className="w-5 h-5"
                fill="none"
                stroke="currentColor"
                strokeWidth={1.5}
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M9 15 3 9m0 0 6-6M3 9h12a6 6 0 0 1 0 12h-3"
                />
              </svg>
            </button>
          </div>
          <div>
            <button
              onClick={() => handle_rotate(90)}
              type={`button`}
            >
              <svg
                className="w-5 h-5"
                fill="none"
                stroke="currentColor"
                strokeWidth={1.5}
                viewBox="0 0 24 24"
                xmlns="http://www.w3.org/2000/svg"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="m15 15 6-6m0 0-6-6m6 6H9a6 6 0 0 0 0 12h3"
                />
              </svg>
            </button>
          </div>
        </div>
        <div className="w-60 h-60 flex justify-center items-center">
          <button
            className="rounded bg-white p-4 aspect-[8.5/11] w-full h-full cursor-zoom-in"
            onClick={() => set_show_preview(true)}
          >
            <canvas
              className={`object-contain h-full w-full ${css_rotation}`}
              ref={canvasRef}
            />
          </button>
        </div>
        {show_preview && (
          <button
            className="fixed inset-0 z-[999] bg-gray-500/50 cursor-zoom-out"
            onClick={() => set_show_preview(false)}
          >
            <div className="absolute left-1/2 top-1/2 w-1/2 -translate-x-1/2 -translate-y-1/2 rounded-lg bg-white p-8">
              <Preview
                index={index}
                pdflib_ref={pdflib_ref}
              />
            </div>
          </button>
        )}
        <div className="w-60 justify-center items-center flex">
          <label className="flex max-w-60 text-sm justify-center items-center gap-2 cursor-pointer">
            <div className="">
              <input
                className="cursor-pointer"
                onClick={handle_toggle}
                type={`checkbox`}
              />
            </div>
            <div className="overflow-hidden text-ellipsis whitespace-nowrap">
              {title}
            </div>
          </label>
        </div>
        <div className="text-center text-sm"></div>
      </div>
    </div>
  )
}
