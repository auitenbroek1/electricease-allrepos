import { useEffect, useRef, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'

import { useController } from './useController'

export const usePart = () => {
  const navigate = useNavigate()

  const { id } = useParams()

  const { show, save } = useController({ endpoint: `/api/parts` })

  //

  const [data, setData] = useState<any>({})
  const [errors, setErrors] = useState<any>({})

  const [categories, setCategories] = useState<any>([])
  const [tags, setTags] = useState<any>([])

  const fetchData = async () => {
    if (!id) return

    show(id)
      .then((data: any) => {
        setData(data)

        setCategories(
          data.categories.map((category: any) => category.id.toString()),
        )

        setTags(data.tags.map((tag: any) => tag.id.toString()))
      })
      .catch((errors) => {
        setErrors(errors)
      })
  }

  useEffect(() => {
    fetchData()
  }, [])

  //

  const nameRef = useRef<HTMLInputElement>(null)
  const descriptionRef = useRef<HTMLInputElement>(null)
  const costRef = useRef<HTMLInputElement>(null)
  const laborRef = useRef<HTMLInputElement>(null)

  //

  const onCancelClick = () => {
    navigate(`/app/parts/all`)
  }

  const onSaveClick = async () => {
    const _cost: any = costRef?.current?.value ?? 0
    const _labor: any = laborRef?.current?.value ?? 0

    const payload = {
      name: nameRef?.current?.value,
      description: descriptionRef?.current?.value,
      cost: _cost,
      labor: _labor,
      categories,
      tags,
    }

    console.log(`onSaveClick`, payload)

    save(id, payload)
      .then((data: any) => {
        setData(data)
        navigate(`/app/parts/all/${data.id}/edit`)
      })
      .catch((errors) => {
        setErrors(errors)
      })
  }

  //

  const [categoriesData, setCategoriesData] = useState<any>([])
  const [tagsData, setTagsData] = useState<any>([])

  const { index: categoriesIndex } = useController({
    endpoint: `/api/parts/categories`,
  })

  const { index: tagsIndex } = useController({ endpoint: `/api/parts/tags` })

  useEffect(() => {
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
  }, [])

  //

  return {
    data,
    errors,

    nameRef,
    descriptionRef,
    costRef,
    laborRef,

    onCancelClick,
    onSaveClick,

    categoriesData,
    categories,
    setCategories,

    tagsData,
    tags,
    setTags,
  }
}
