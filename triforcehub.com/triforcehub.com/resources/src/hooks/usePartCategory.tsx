import { useNavigate, useParams } from 'react-router-dom'
import { useEffect, useRef, useState } from 'react'

import { useController } from './useController'

export const usePartCategory = () => {
  const navigate = useNavigate()

  const {
    id
  } = useParams()

  const {
    show,
    save,
  } = useController({ endpoint: `/api/parts/categories` })

  //

  const [data, setData] = useState<any>({})
  const [errors, setErrors] = useState<any>({})

  const fetchData = async () => {
    if (!id) return

    show(id).then((data) => {
      setData(data)
    }).catch(errors => {
      setErrors(errors)
    })
  }

  useEffect(() => {
    fetchData()
  }, [])

  //

  const nameRef = useRef<HTMLInputElement>(null)
  const descriptionRef = useRef<HTMLInputElement>(null)

  //

  const onCancelClick = () => {
    navigate('/app/parts/categories')
  }

  const onSaveClick = async () => {
    const payload = {
      name: nameRef?.current?.value,
      description: descriptionRef?.current?.value,
    }

    console.log('onSaveClick', payload)

    save(id, payload).then((data) => {
      setData(data)
      navigate('/app/parts/categories')
    }).catch(errors => {
      setErrors(errors)
    })
  }

  //

  return {
    data,
    errors,

    nameRef,
    descriptionRef,

    onCancelClick,
    onSaveClick,
  }
}
