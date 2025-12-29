import axios from 'axios'
import { useMutation, useQueryClient } from '@tanstack/react-query'

import { getErrors } from '@/helpers/exception'

export const usePartController = () => {
  const queryClient = useQueryClient()

  const save = useMutation(
    async (variables: SavePartArguments) => {
      const { id, params } = variables

      try {
        const response = id
          ? await axios.patch(`/api/parts/${id}`, params)
          : await axios.post(`/api/parts`, params)

        const data: PartResource = response.data.data

        // await sleep(1000)

        return data
      } catch (exception: unknown) {
        console.log(`debug.api.save.error`, exception)
        const data: ValidationError = getErrors(exception)

        throw data
      }
    },
    {
      onError: (error: ValidationError) => {
        console.log(`debug.api.save.onerror.1`, error)
      },
      onSuccess: (data) => {
        console.log(`debug.api.save.success.1`, data)
        queryClient.invalidateQueries([`api.parts`])
      },
    },
  )

  //

  return {
    save,
  }
}
