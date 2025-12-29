import axios from 'axios'
import debounce from 'lodash/debounce'
import { useContext, useEffect, useMemo, useRef, useState } from 'react'

import JobsContext from '@/pages/jobs/contexts/Job'

import { useJob } from '@/data/Job'
import toast from 'react-hot-toast'

export function useIsMounted() {
  const isMounted = useRef(false)

  useEffect(() => {
    isMounted.current = true
    return () => {
      isMounted.current = false
    }
  }, [])

  return isMounted
}

export const useJobCustomer = (data: any) => {
  const id = data ? data.id : null

  //

  const { reloadJob } = useJob()
  const { job }: any = useContext(JobsContext)

  //

  const name_ref = useRef<HTMLInputElement>(null)
  const email_ref = useRef<HTMLInputElement>(null)
  const address1_ref = useRef<HTMLInputElement>(null)
  const address2_ref = useRef<HTMLInputElement>(null)
  const city_ref = useRef<HTMLInputElement>(null)
  const state_id_ref = useRef<HTMLSelectElement>(null)
  const zip_ref = useRef<HTMLInputElement>(null)
  const mobile_ref = useRef<HTMLInputElement>(null)
  const office_ref = useRef<HTMLInputElement>(null)

  // const isMounted = useIsMounted()
  const [loading, setLoading] = useState(false)
  const [errors, setErrors] = useState<any>({})

  //

  // console.log(`debug.mounted0`, isMounted.current)

  const onDeleteClick = async () => {
    const response = await axios.delete(`/api/jobs/customers/${id}`)
    console.log(`onDeleteClick`, response.data.data)

    reloadJob()
  }

  const validate = () => {
    const errors: any = {}
    // if (!nameRef?.current?.validity.valid) {
    //   errors.name = [nameRef?.current?.validationMessage]
    // }
    // if (!costRef?.current?.validity.valid) {
    //   errors.cost = [costRef?.current?.validationMessage]
    // }
    if (Object.keys(errors).length) {
      console.log(`errors`, errors)
      setErrors(errors)
      return false
    }

    setErrors(errors)
    return true
  }

  const save = async () => {
    if (!validate()) {
      return
    }

    // console.log(`debug.mounted1`, isMounted.current)
    setLoading(true)

    // console.log('errors', errors)
    // if (Object.keys(errors)) return

    const payload = {
      job_id: job?.id,
      name: name_ref?.current?.value,
      email: email_ref?.current?.value,
      address1: address1_ref?.current?.value,
      address2: address2_ref?.current?.value,
      city: city_ref?.current?.value,
      state_id: state_id_ref?.current?.value,
      zip: zip_ref?.current?.value,
      mobile: mobile_ref?.current?.value,
      office: office_ref?.current?.value,
    }

    if (id) {
      console.log(`debug.save`, `update`, payload)
      try {
        const response = await axios.patch(`/api/jobs/customers/${id}`, payload)
        console.log(`debug.save`, response.data.data)
        await reloadJob()
        toast.success(`Saved!`)
      } catch (exception: any) {
        console.log(`debug.save`, exception.response.data.errors)
        setErrors(exception.response.data.errors)
      }
    } else {
      console.log(`debug.save`, `create`, payload)
      try {
        const response = await axios.post(`/api/jobs/customers`, payload)
        console.log(`debug.save`, response.data.data)
        await reloadJob()
        toast.success(`Saved!`)
      } catch (exception: any) {
        console.log(`debug.save`, exception.response.data.errors)
        setErrors(exception.response.data.errors)
      }
    }

    // console.log(`debug.mounted2`, isMounted.current)
    setLoading(false)
  }

  //

  const debouncedSave = useMemo(() => debounce(() => save(), 1000), [id])

  useEffect(() => {
    console.log(`debug.effect.1`)
    return () => {
      console.log(`debug.effect.0`)
      debouncedSave.cancel
    }
  }, [debouncedSave])

  return {
    name_ref,
    email_ref,
    address1_ref,
    address2_ref,
    city_ref,
    state_id_ref,
    zip_ref,
    mobile_ref,
    office_ref,

    onDeleteClick,
    sync: debouncedSave,

    loading,
    errors,
  }
}
