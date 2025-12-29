import { useQuery } from '@tanstack/react-query'
import { useEffect, useState } from 'react'

import { api } from '@/api'
import { useParams } from 'react-router-dom'

export const useDetailsQuery = () => {
  const { id } = useParams()

  // #region part

  const {
    data: partData,
    isPlaceholderData: isPlaceholderPart,
    isFetching: isFetchingPart,
  } = useQuery(
    [`api.parts`, { id }],
    () => {
      if (id) return api.parts.show(id)
      throw `oops`
    },
    {
      enabled: !!id,
      placeholderData: null,
      refetchOnWindowFocus: false,
      select: api.parts.placeholder,
    },
  )

  const part = partData || api.parts.placeholder(null)

  // #endregion

  // #region categories

  const categoriesPlaceholderData: PartCategoryResource[] = []

  const {
    data: categoriesData,
    isPlaceholderData: isCategoriesPlaceholderData,
  } = useQuery(
    [`api.parts.categories`, { size: 999 }],
    () => {
      return api.parts.categories.index({ size: 999 })
    },
    {
      placeholderData: categoriesPlaceholderData,
      refetchOnWindowFocus: false,
      // staleTime: Infinity,
    },
  )

  const categories = categoriesData ?? categoriesPlaceholderData

  // #endregion

  // #region tags

  const tagsPlaceholderData: PartTagResource[] = []

  const { data: tagsData, isPlaceholderData: isTagsPlaceholderData } = useQuery(
    [`api.parts.tags`, { size: 999 }],
    () => {
      return api.parts.tags.index({ size: 999 })
    },
    {
      placeholderData: tagsPlaceholderData,
      refetchOnWindowFocus: false,
      // staleTime: Infinity,
    },
  )

  const tags = tagsData ?? tagsPlaceholderData

  // #endregion

  const [isLoading, setIsLoading] = useState(true)

  useEffect(() => {
    setIsLoading(
      (isPlaceholderPart && isFetchingPart) ||
        isCategoriesPlaceholderData ||
        isTagsPlaceholderData,
    )
  }, [
    isPlaceholderPart,
    isFetchingPart,
    isCategoriesPlaceholderData,
    isTagsPlaceholderData,
  ])

  // if (isPlaceholderData && !isFetching) // not loading
  // if (isPlaceholderData && isFetching) // loading

  //

  return {
    part,
    categories,
    tags,
    isLoading,
  }
}
