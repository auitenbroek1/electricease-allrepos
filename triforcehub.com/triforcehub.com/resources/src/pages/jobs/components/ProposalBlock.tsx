import axios from 'axios'
import debounce from 'lodash/debounce'
import { useContext, useMemo, useRef } from 'react'

import JobContext from '../contexts/Job'
import { useJob } from '@/data/Job'

import { Form } from '@/components'
import { TrashIcon } from '@/components/Icons'

export const ProposalBlock = (props: any) => {
  const { data, section } = props

  const { job }: any = useContext(JobContext)
  const { reloadJob } = useJob()

  const content_ref = useRef<any>()

  const save = async () => {
    console.log(`debug.save`)
    // if (!validate()) {
    //   return
    // }

    // setLoading(true)

    // console.log('errors', errors)
    // if (Object.keys(errors)) return

    const payload = {
      job_id: job?.id,
      job_section_id: section,
      content: content_ref?.current?.value,
    }

    console.log(`debug.save`, payload)

    if (data?.id) {
      console.log(`debug.update`)
      try {
        const response = await axios.patch(
          `/api/jobs/blocks/${data?.id}`,
          payload,
        )
        console.log(`debug.save`, response.data?.data)
        await reloadJob()
      } catch (exception: any) {
        console.log(`debug.save`, exception.response.data?.errors)
        // setErrors(exception.response.data?.errors)
      }
    } else {
      console.log(`debug.store`)
      try {
        const response = await axios.post(`/api/jobs/blocks`, payload)
        console.log(`debug.save`, response.data?.data)
        await reloadJob()
      } catch (exception: any) {
        console.log(`debug.save`, exception.response.data?.errors)
        // setErrors(exception.response.data?.errors)
      }
    }

    // setLoading(false)
  }

  const sync = useMemo(() => debounce(() => save(), 1000), [job?.id, data?.id])

  const handleRemove = async () => {
    try {
      const response = await axios.delete(`/api/jobs/blocks/${data?.id}`)
      console.log(`debug.delete`, response.data)
      await reloadJob()
    } catch (exception: any) {
      console.log(`debug.save`, exception.response.data?.errors)
      // setErrors(exception.response.data?.errors)
    }
  }

  return (
    <div className={`flex w-full space-x-4`}>
      <div className={`w-full`}>
        <Form.Textarea
          defaultValue={data?.content}
          onChange={sync}
          ref={content_ref}
        />
      </div>
      <div className={`flex items-center justify-center`}>
        <div className={`w-5 text-brand-gradient-light`}>
          {data?.id && (
            <button
              onClick={handleRemove}
              type={`button`}
            >
              <div className={`h-5 w-5`}>
                <TrashIcon />
              </div>
            </button>
          )}
        </div>
      </div>
    </div>
  )
}
