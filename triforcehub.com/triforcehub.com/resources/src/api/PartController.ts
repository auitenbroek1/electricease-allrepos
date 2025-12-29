import axios from 'axios'

// import { sleep } from '@/helpers/sleep'

export const PartController = {
  show: async (id: string): Promise<PartResource> => {
    console.log(`debug.api`, id)

    const response = await axios.get(`/api/parts/${id}`)
    // await sleep(1000)

    return response?.data?.data
  },

  //

  placeholder: (data: PartResource | null): Partial<PartResource> => {
    return {
      assemblies: data?.assemblies,
      categories: data?.categories,
      cost: data?.cost,
      description: data?.description,
      id: data?.id,
      labor: data?.labor,
      name: data?.name,
      tags: data?.tags,
    }
  },
}
