import { useEffect, useRef } from 'react'

import { PDFDocument } from 'pdf-lib'

export const Preview = (props: any) => {
  const scope = `debug.pdf.preview`

  const { index, pdflib_ref } = props

  const iframe_ref = useRef<HTMLIFrameElement>(null)

  useEffect(() => {
    console.log(scope, `mount`, index)

    const load = async () => {
      console.log(scope, `zoom in`, index)

      if (!iframe_ref.current) return

      const clone = await PDFDocument.create()

      const pages = await clone.copyPages(pdflib_ref.current, [index])
      for (const page of pages) {
        clone.addPage(page)
      }

      const data = await clone.saveAsBase64({ dataUri: true })
      iframe_ref.current.src = `${data}#navpanes=0&view=fitH&toolbar=1`
    }

    load()

    return () => {
      console.log(scope, `unmount`, index)
    }
  }, [])

  return (
    <div>
      <iframe
        className="w-full h-[75vh]"
        ref={iframe_ref}
      ></iframe>
    </div>
  )
}
