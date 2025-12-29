import { DetailsForm } from './DetailsForm'

import { useDetailsQuery } from '../hooks/useDetailsQuery'

export const Details = () => {
  const { part, categories, tags, isLoading } = useDetailsQuery()

  console.log(`debug.api.details`, { isLoading })

  if (isLoading) return <div>loading...</div>

  return (
    <DetailsForm
      key={isLoading ? `placeholder` : `loaded`}
      part={part}
      categories={categories}
      tags={tags}
    />
  )
}
