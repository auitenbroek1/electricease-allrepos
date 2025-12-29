import { useState } from 'react'

import { uploader } from '@/helpers/uploader'
import axios from 'axios'

import { Card, Form } from '@/components'

import { Document } from './Document'
import { useNavigate } from 'react-router-dom'

export const Upload = (props: any) => {
  const scope = `debug.upload`

  const { job } = props

  const [file_meta, set_file_meta] = useState<any>(null)
  const [file_data, set_file_data] = useState<any>(null)
  const [progress, set_progress] = useState<number>(0)

  const process_file = async (event: any) => {
    const file = event.target.files[0]

    console.log(scope, file)

    if (!file) return

    const modified = file.lastModified
    const name = file.name
    const size = file.size
    const type = file.type

    const meta = { modified, name, size, type }
    console.log(scope, meta)
    set_file_meta(meta)

    if (type !== `application/pdf`) return

    const file_data = await get_file_data(file)
    console.log(scope, { file_data })
    set_file_data(file_data)
  }

  const get_file_data = async (file: any) => {
    return new Promise((resolve, reject) => {
      const handleEvent = (event: any) => {
        console.log(scope, event.type, event)
        console.log(scope, event.loaded)

        const percent = Math.round((event.loaded / event.total) * 100)
        const result = event.target.result

        if (event.type === `loadend`) resolve(result)
        if (event.type === `progress`) set_progress(percent)
        if (event.type === `error`) reject(event)
        if (event.type === `abort`) reject(event)
      }

      const reader = new FileReader()
      reader.addEventListener(`loadstart`, handleEvent)
      reader.addEventListener(`load`, handleEvent)
      reader.addEventListener(`loadend`, handleEvent)
      reader.addEventListener(`progress`, handleEvent)
      reader.addEventListener(`error`, handleEvent)
      reader.addEventListener(`abort`, handleEvent)
      reader.readAsArrayBuffer(file)
    })
  }

  //

  const [percent, setPercent] = useState<number>(0)
  // const [id, setID] = useState(data?.id)
  // const [url, setUrl] = useState(data?.url)

  const navigate = useNavigate()

  const handle_save = async (blob: any) => {
    console.log(scope, `blob`, blob)

    const s3: any = await uploader.store(blob, {
      progress: (progress: any) => {
        const percent: any = Math.round(progress * 100)
        setPercent(percent)
      },
      contentType: file_meta.type,
      visibility: `public-read`,
    })
    console.log(scope, `s3`, s3)

    const upload = await axios.post(`/api/uploads`, {
      uuid: s3.uuid,
      key: s3.key,
      bucket: s3.bucket,
      name: file_meta.name,
      type: file_meta.type,
      size: file_meta.size,
    })
    console.log(scope, `upload`, upload.data)

    // setID(upload.data.id)
    // setUrl(upload.data.url)
    // console.log(scope, ref.current)

    const job_id = job.id
    const name = file_meta.name
    const upload_id = upload.data.id

    const payload = {
      job_id,
      name,
      upload_id,
    }

    console.log(scope, `payload`, payload)

    try {
      const response: any = await axios.post(`/api/jobs/${job.id}/plans`, payload)
      const data2 = response?.data?.data
      console.log(scope, `success!!!`, data2)
      // setErrors(null)
      navigate(`/app/jobs/all/${job.id}/plans/all/${data2.id}/takeoff`)
    } catch (errors: any) {
      const response = errors?.response?.data
      console.log(scope, `error!!!`, response)
      // setErrors(response.errors)
    }
  }

  const handle_cancel = () => {
    navigate(`/app/jobs/all/${job.id}/plans/all`)
  }

  //

  console.log(scope, `render`)

  return (
    <Card.Root key={file_meta?.name}>
      <Card.Main>
        {file_data ? (
          <Card.Columns number={1}>
            {percent === 0 ? (
              <Document
                data={file_data}
                on_cancel={handle_cancel}
                on_save={handle_save}
              />
            ) : (
              <div>Progress: {percent}</div>
            )}
          </Card.Columns>
        ) : (
          <Card.Columns number={1}>
            <input
              type="file"
              onChange={process_file}
            />
          </Card.Columns>
        )}
      </Card.Main>
    </Card.Root>
  )
}
