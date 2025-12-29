import { api } from '@/api'
import { PageSectionIntro, PageSectionItems } from '@/components'
import { useQuery } from '@tanstack/react-query'
import { useParams } from 'react-router-dom'

// import { Details } from './components/Details'
// import { Details } from './components/DetailsNew'

export const AssembliesPage = () => {
  const { id }: any = useParams()

  const { data: part } = useQuery([`parts`, { id }], () => api.parts.show(id), {
    enabled: !!id,
    placeholderData: null,
    refetchOnWindowFocus: false,
    select: (data): Partial<PartResource> => {
      return {
        assemblies: data?.assemblies,
        categories: data?.categories,
        cost: data?.cost,
        description: data?.description,
        labor: data?.labor,
        name: data?.name,
        tags: data?.tags,
      }
    },
  })

  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Material Assemblies`}>
        Assemblies this material is assigned to
      </PageSectionIntro>
      <div>
        {part?.assemblies?.map((assembly: any, index: number) => (
          <div
            className="flex justify-between"
            key={index}
          >
            <div>{assembly.name}</div>
            <div>{assembly.quantity}</div>
          </div>
        ))}
      </div>
    </PageSectionItems>
  )
}
