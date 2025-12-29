import debounce from 'lodash/debounce'

import { useContext, useEffect, useMemo, useRef, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'

import { useController } from './useController'

import JobContext from '@/pages/jobs/contexts/Job'

export const useJob = () => {
  const { setJob }: any = useContext(JobContext)

  const navigate = useNavigate()

  const { id } = useParams()

  // console.log('debug.param', id)

  //

  const id_ref = useRef<any>(id)
  const number_ref = useRef<HTMLInputElement>(null)
  const name_ref = useRef<HTMLInputElement>(null)
  const description_ref = useRef<HTMLInputElement>(null)
  const temporary_power_ref = useRef<HTMLInputElement>(null)
  const temporary_lighting_ref = useRef<HTMLInputElement>(null)
  const sqft_ref = useRef<HTMLInputElement>(null)
  const labor_factor_ref = useRef<HTMLInputElement>(null)

  // console.log('debug.ref', id_ref.current)

  useEffect(() => {
    console.log(`debug`, 222)
    id_ref.current = id
  }, [id])

  //

  const { show, save } = useController({ endpoint: `/api/jobs` })

  //

  const [errors, setErrors] = useState<any>({})

  const updateRefs = (data: any) => {
    console.log(data)

    number_ref?.current && (number_ref.current.value = data.number)
    name_ref?.current && (name_ref.current.value = data.name)
    description_ref?.current &&
      (description_ref.current.value = data.description)
    temporary_power_ref?.current &&
      (temporary_power_ref.current.value = data.temporary_power)
    temporary_lighting_ref?.current &&
      (temporary_lighting_ref.current.value = data.temporary_lighting)
    sqft_ref?.current && (sqft_ref.current.value = data.sqft)
    labor_factor_ref?.current &&
      (labor_factor_ref.current.value = data.labor_factor)

    console.log(`debug.render`, `set`)
  }

  const fetchData = async () => {
    if (!id) return

    show(id)
      .then((data: any) => {
        setJob(data)
        updateRefs(data)
        setErrors([])
      })
      .catch((errors) => {
        setErrors(errors)
      })
  }

  useEffect(() => {
    console.log(`debug`, 111)
    fetchData()
  }, [])

  //

  const onCancelClick = () => {
    navigate(`/app/jobs/all`)
  }

  const saveOnly = async () => {
    return new Promise((resolve, reject) => {
      const payload = {
        number: number_ref?.current?.value,
        name: name_ref?.current?.value,
        description: description_ref?.current?.value,
        temporary_power: temporary_power_ref?.current?.value,
        temporary_lighting: temporary_lighting_ref?.current?.value,
        sqft: sqft_ref?.current?.value,
        labor_factor: labor_factor_ref?.current?.value,
      }

      console.log(`saveOnly`, payload)

      save(id, payload)
        .then((data: any) => {
          console.log(`debug.all`, id, data.id, id_ref.current)
          setJob(data)
          if (!id) {
            // id_ref.current = data.id
            navigate(`/app/jobs/all/${data.id}/edit`)
          }
          updateRefs(data)
          setErrors([])
          return resolve(data)
        })
        .catch((errors) => {
          setErrors(errors)
          return reject(errors)
        })
    })
  }

  const onSaveClick = async () => {
    const payload = {
      number: number_ref?.current?.value,
      name: name_ref?.current?.value,
      description: description_ref?.current?.value,
      temporary_power: temporary_power_ref?.current?.value,
      temporary_lighting: temporary_lighting_ref?.current?.value,
      sqft: sqft_ref?.current?.value,
      labor_factor: labor_factor_ref?.current?.value,
    }

    console.log(`onSaveClick`, payload)

    // return

    save(id, payload)
      .then(() => {
        navigate(`/app/jobs/all`)
      })
      .catch((errors) => {
        setErrors(errors)
      })
  }

  const debouncedSave = useMemo(() => debounce(() => saveOnly(), 1000), [id])

  //

  return {
    errors,

    id_ref,
    number_ref,
    name_ref,
    description_ref,
    temporary_power_ref,
    temporary_lighting_ref,
    sqft_ref,
    labor_factor_ref,

    onCancelClick,
    onSaveClick,

    saveOnly,
    debouncedSave,
  }
}
