import { useNavigate, useParams } from 'react-router-dom'
import { useEffect, useMemo, useRef, useState } from 'react'
import debounce from 'lodash/debounce'

import { useController } from './useController'

export const useAssembly = () => {
  const navigate = useNavigate()

  const { id } = useParams()

  //

  const [data, setData] = useState<any>({})
  const [categoriesData, setCategoriesData] = useState<any>([])
  const [tagsData, setTagsData] = useState<any>([])
  const [parts, setParts] = useState<any>([])
  const [errors, setErrors] = useState<any>({})

  const nameRef = useRef<HTMLInputElement>(null)
  const descriptionRef = useRef<HTMLInputElement>(null)
  const categoriesRef = useRef([])
  const tagsRef = useRef([])
  const partsRef = useRef([])

  const { show, save } = useController({
    endpoint: `/api/assemblies`,
  })

  const { index: categoriesIndex } = useController({
    endpoint: `/api/assemblies/categories`,
  })

  const { index: tagsIndex } = useController({
    endpoint: `/api/assemblies/tags`,
  })

  //

  const fetchData = async () => {
    if (!id) return

    try {
      const response: any = await show(id)
      setData(response)
      //
      categoriesRef.current = response.categories.map((category: any) =>
        category.id.toString(),
      )

      tagsRef.current = response.tags.map((tag: any) => tag.id.toString())

      partsRef.current = response.parts.map((part: any) => {
        return {
          id: part.id,
          quantity: part.quantity,
        }
      })
      //
      setErrors({})
    } catch (errors) {
      setErrors(errors)
    }
  }

  const fetchCategoriesData = async () => {
    categoriesIndex({ size: 999 })
      .then((data: any) => {
        const items = data?.map((item: any) => {
          return {
            label: item.name,
            value: item.id.toString(),
          }
        })
        // console.log(items)
        setCategoriesData(items)
      })
      .catch((error) => console.log(error))
  }

  const fetchTagsData = async () => {
    tagsIndex({ size: 999 })
      .then((data: any) => {
        const items = data?.map((item: any) => {
          return {
            label: item.name,
            value: item.id.toString(),
          }
        })
        // console.log(items)
        setTagsData(items)
      })
      .catch((error) => console.log(error))
  }

  useEffect(() => {
    fetchData()
    fetchCategoriesData()
    fetchTagsData()
  }, [])

  //

  const handleCategoriesChange = (event: any) => {
    console.log(event)
    categoriesRef.current = event
    debouncedAutoSave()
  }

  const handleTagsChange = (event: any) => {
    console.log(event)
    tagsRef.current = event
    debouncedAutoSave()
  }

  const handlePartsChange = (event: any) => {
    console.log(event)
    partsRef.current = event
    debouncedAutoSave()
  }

  //

  const saveData = async () => {
    return new Promise(async (resolve, reject) => {
      const payload = {
        name: nameRef?.current?.value,
        description: descriptionRef?.current?.value,
        categories: categoriesRef?.current,
        tags: tagsRef?.current,
        parts: partsRef?.current,
      }

      console.log(payload)
      // return reject({})

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
      navigate(`/app/assemblies/all/${response?.id}/edit`)
    } catch (errors) {}
  }

  const debouncedAutoSave = useMemo(
    () => debounce(() => autoSave(), 1000),
    [id],
  )

  //

  const onCancelClick = () => {
    navigate(`/app/assemblies/all`)
  }

  const onSaveClick = async () => {
    try {
      await saveData()
      navigate(`/app/assemblies/all`)
    } catch (errors) {}
  }

  //

  return {
    data,
    errors,

    nameRef,
    descriptionRef,

    categoriesData,
    categoriesRef,
    handleCategoriesChange,

    tagsData,
    tagsRef,
    handleTagsChange,

    parts,
    setParts,
    handlePartsChange,

    debouncedAutoSave,

    onCancelClick,
    onSaveClick,
  }
}
