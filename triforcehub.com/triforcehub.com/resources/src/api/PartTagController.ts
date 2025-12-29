import axios from "axios"

export const PartTagController = {
  index: async (params: any): Promise<PartTagResource[]> => {
    const response = await axios.request({
      url: `/api/parts/tags`,
      method: `get`,
      params,
    })

    return response?.data?.data
  },
}
