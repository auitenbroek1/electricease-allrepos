import { useNavigate, useParams } from 'react-router-dom'
import { useEffect, useMemo, useRef, useState } from 'react'
import debounce from 'lodash/debounce'

import { useController } from './useController'

export const useAssemblyCategory = () => {
  const navigate = useNavigate()

  const { id } = useParams()

  //

  const [data, setData] = useState<any>({})
  const [errors, setErrors] = useState<any>({})

  const nameRef = useRef<HTMLInputElement>(null)
  const descriptionRef = useRef<HTMLInputElement>(null)

  const { show, save } = useController({
    endpoint: `/api/assemblies/categories`,
  })

  //

  const fetchData = async () => {
    if (!id) return

    try {
      const response = await show(id)
      setData(response)
      setErrors({})
    } catch (errors) {
      setErrors(errors)
    }
  }

  useEffect(() => {
    fetchData()
  }, [])

  //

  const saveData = async () => {
    return new Promise(async (resolve, reject) => {
      const payload = {
        name: nameRef?.current?.value,
        description: descriptionRef?.current?.value,
      }

      try {
        const response = await save(id, payload)
        setData(response)
        setErrors({})
        return resolve(response)
      } catch (errors) {
        setErrors(errors)
        return reject(errors)
      }
    })
  }

  const autoSave = async () => {
    try {
      const response: any = await saveData()
      navigate(`/app/assemblies/categories/${response?.id}/edit`)
    } catch (errors) {}
  }

  const debouncedAutoSave = useMemo(
    () => debounce(() => autoSave(), 1000),
    [id],
  )

  //

  const onCancelClick = () => {
    navigate(`/app/assemblies/categories`)
  }

  const onSaveClick = async () => {
    try {
      await saveData()
      navigate(`/app/assemblies/categories`)
    } catch (errors) {}
  }

  //

  return {
    data,
    errors,

    nameRef,
    descriptionRef,

    debouncedAutoSave,

    onCancelClick,
    onSaveClick,
  }
}
