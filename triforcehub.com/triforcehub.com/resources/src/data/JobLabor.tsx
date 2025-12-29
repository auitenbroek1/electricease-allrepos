import axios from 'axios'
import debounce from 'lodash/debounce'
import toast from 'react-hot-toast'

import { useContext, useMemo, useRef, useState } from 'react'

import JobsContext from '@/pages/jobs/contexts/Job'

import { useJob } from '@/data/Job'

export const useJobLabor = (data: any) => {
  const { id } = data

  //

  const { reloadJob } = useJob()

  const { job }: any = useContext(JobsContext)
  const [loading, setLoading] = useState(false)
  const [errors, setErrors] = useState<any>({})

  //

  const nameRef = useRef<HTMLInputElement>(null)
  const hoursRef = useRef<HTMLInputElement>(null)
  const rateRef = useRef<HTMLInputElement>(null)
  const notesRef = useRef<HTMLInputElement>(null)
  const enabledRef = useRef<HTMLInputElement>(null)

  const onDeleteClick = async () => {
    const response = await axios.delete(`/api/jobs/labor/${id}`)
    console.log(`onDeleteClick`, response.data.data)

    reloadJob()
  }

  const save = async () => {
    setLoading(true)

    // console.log('errors', errors)
    // if (Object.keys(errors)) return

    const payload = {
      job_id: job.id,
      name: nameRef?.current?.value,
      hours: hoursRef?.current?.value,
      rate: rateRef?.current?.value,
      notes: notesRef?.current?.value,
      enabled: enabledRef?.current?.checked,
    }

    if (id) {
      console.log(`debug.save`, `update`, payload)
      try {
        const response = await axios.patch(`/api/jobs/labor/${id}`, payload)
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
        const response = await axios.post(`/api/jobs/labor`, payload)
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
    rateRef,
    enabledRef,
    nameRef,
    notesRef,
    hoursRef,

    onDeleteClick,
    save: debouncedSave,

    loading,
    errors,
  }
}
