import { forwardRef, useState } from 'react'
import axios from 'axios'
import { uploader } from '@/helpers/uploader'

const UploadRef = (props: any, ref: any) => {
  const { data, disabled, max_width } = props

  const [percent, setPercent] = useState(null)
  const [id, setID] = useState(data?.id)
  const [url, setUrl] = useState(data?.url)

  const handleUpload = async (event: any) => {
    const file = event.target.files[0]
    console.log(`debug.upload`, file)

    const s3: any = await uploader.store(file, {
      progress: (progress: any) => {
        const percent: any = Math.round(progress * 100)
        setPercent(percent)
      },
      visibility: `public-read`,
    })
    console.log(`debug.upload`, s3)

    const upload = await axios.post(`/api/uploads`, {
      uuid: s3.uuid,
      key: s3.key,
      bucket: s3.bucket,
      name: file.name,
      type: file.type,
      size: file.size,
      max_width: max_width ?? 0,
    })
    console.log(`debug.upload`, upload.data)

    setID(upload.data.id)
    setUrl(upload.data.url)
    console.log(`debug.upload`, ref.current)
  }

  console.log(`debug.upload`, { data, disabled, max_width })

  return (
    <div className={`space-y-2`}>
      <div
        className={`
        relative
        z-0
        flex
        h-9
        w-full
        items-center
        space-x-2
        rounded
        border
        border-gray-300
        bg-white
        px-2
        text-sm
        focus-within:border-blue-600
        focus-within:outline-none
        focus-within:ring-2
        focus-within:ring-blue-200
        focus-within:ring-offset-1
        ${disabled ? `!bg-gray-50` : ``}
        ${disabled ? `!border-gray-200` : ``}
        ${disabled ? `!pointer-events-none` : ``}
        ${disabled ? `!text-gray-600` : ``}
      `}
      >
        <div className={`flex-1`}>
          <input
            accept="image/*,.pdf"
            className={`
              relative
              z-0
              w-full
              placeholder-gray-400
              invalid:border-red-600
              read-only:border-transparent
              read-only:bg-transparent
              read-only:text-inherit
              focus:outline-none
              invalid:focus:border-red-600 invalid:focus:ring-red-200
            `}
            disabled={disabled}
            onChange={handleUpload}
            type={`file`}
          />
          <input
            ref={ref}
            type={`hidden`}
            defaultValue={id}
          />
        </div>
        {percent && (
          <div
            className={`
            pointer-events-none
          `}
          >
            {percent}
          </div>
        )}
      </div>
      {url && (
        <div className={`max-w-sm`}>
          <img src={url} />
        </div>
      )}
    </div>
  )
}

export const Upload = forwardRef(UploadRef)
