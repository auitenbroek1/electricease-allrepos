import { useNavigate } from 'react-router-dom'
import { useForm } from 'react-hook-form'

import { usePartController } from '@/api/usePartController'
import { getFormErrors } from '@/helpers/exception'

type Props = {
  part: Partial<PartResource>
}

type DetailsForm = {
  categories: string[] | undefined
  cost: number | undefined
  description: string | undefined
  labor: number | undefined
  name: string | undefined
  tags: string[] | undefined
}

export const useDetailsForm = (props: Props) => {
  const { part } = props

  //

  const {
    control,
    formState: { errors },
    handleSubmit,
    register,
    setError,
  } = useForm<DetailsForm>({
    defaultValues: {
      name: part.name,
      description: part.description,
      cost: part.cost,
      labor: part.labor,
      categories: part.categories?.map((category) => category.id.toString()),
      tags: part.tags?.map((tag) => tag.id.toString()),
    },
  })

  //

  const { save } = usePartController()

  const navigate = useNavigate()

  const onSaveClick = () => {
    handleSubmit(async (data) => {
      console.log(`debug.api.save.valid`, data)

      const params: SavePartRequest = { ...data }

      save.mutate(
        { id: part.id, params },
        {
          onSuccess: (response) => {
            console.log(`debug.api.save.onsuccess.2`, response)
            // if (!part.id) navigate(`/app/parts/all/${response.id}/edit`)
            navigate(`/app/parts/all/`)
          },
          onError: (error) => {
            console.log(`debug.api.save.onerror.2`, error)
            const errors = getFormErrors<SavePartRequest>(error)
            for (const error of errors) {
              console.log(`debug.api.save.error`, error)
              setError(error.name, error.error, { shouldFocus: true })
            }
          },
        },
      )
    })()
  }

  //

  const cancel = () => {
    navigate(`/app/parts/all`)
  }

  return {
    control,
    register,
    errors,
    save: onSaveClick,
    cancel,
  }
}
