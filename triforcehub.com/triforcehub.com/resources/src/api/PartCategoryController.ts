import axios from "axios"

export const PartCategoryController = {
  index: async (params: any): Promise<PartCategoryResource[]> => {
    const response = await axios.request({
      url: `/api/parts/categories`,
      method: `get`,
      params,
    })

    return response?.data?.data
  },
}
