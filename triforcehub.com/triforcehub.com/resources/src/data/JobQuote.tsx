import axios from 'axios'
import debounce from 'lodash/debounce'
import toast from 'react-hot-toast'

import { useContext, useMemo, useRef, useState } from 'react'

import JobsContext from '@/pages/jobs/contexts/Job'

import { useJob } from '@/data/Job'

export const useJobQuote = (data: any) => {
  const { id } = data

  //

  const { reloadJob } = useJob()

  const { job }: any = useContext(JobsContext)
  const [loading, setLoading] = useState(false)
  const [errors, setErrors] = useState<any>({})

  //

  const costRef = useRef<HTMLInputElement>(null)
  const enabledRef = useRef<HTMLInputElement>(null)
  const nameRef = useRef<HTMLInputElement>(null)
  const notesRef = useRef<HTMLInputElement>(null)

  const onDeleteClick = async () => {
    const response = await axios.delete(`/api/jobs/quotes/${id}`)
    console.log(`onDeleteClick`, response.data.data)

    reloadJob()
  }

  const save = async () => {
    setLoading(true)

    // console.log('errors', errors)
    // if (Object.keys(errors)) return

    const payload = {
      job_id: job.id,
      cost: costRef?.current?.value,
      enabled: enabledRef?.current?.checked,
      name: nameRef?.current?.value,
      notes: notesRef?.current?.value,
    }

    if (id) {
      console.log(`debug.save`, `update`, payload)
      try {
        const response = await axios.patch(`/api/jobs/quotes/${id}`, payload)
        console.log(`debug.save`, response.data.data)
        toast.success(`Saved!`)
        setErrors([])
        await reloadJob()
      } catch (exception: any) {
        console.log(`debug.save`, exception.response.data.errors)
        setErrors(exception.response?.data?.errors ?? [])
      }
    } else {
      console.log(`debug.save`, `create`, payload)
      try {
        const response = await axios.post(`/api/jobs/quotes`, payload)
        console.log(`debug.save`, response.data.data)
        toast.success(`Saved!`)
        setErrors([])
        await reloadJob()
      } catch (exception: any) {
        console.log(`debug.save`, exception.response.data.errors)
        setErrors(exception.response?.data?.errors ?? [])
      }
    }

    setLoading(false)
  }

  const debouncedSave = useMemo(() => debounce(() => save(), 1000), [])

  return {
    costRef,
    enabledRef,
    nameRef,
    notesRef,

    onDeleteClick,
    save: debouncedSave,

    loading,
    errors,
  }
}
