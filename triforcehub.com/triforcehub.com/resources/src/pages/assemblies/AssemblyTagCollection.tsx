import { useMemo } from 'react'
import { useNavigate } from 'react-router-dom'

import { Card } from '@/components/Card'
import { Collection } from '@/components/Collection'
import { FormButton, PageSectionIntro, PageSectionItems } from '@/components'

import { AssemblyTagActions } from './components/AssemblyTagActions'
import { PlusCircleIcon } from '@/components/Icons'

export const AssemblyTagCollection = () => {
  const navigate = useNavigate()

  const columns = useMemo(
    () => [
      {
        accessor: `id`,
        name: `#`,
      },
      {
        accessor: `name`,
        name: `Name`,
        width: `*`,
      },
      {
        accessor: `id`,
        name: ``,
        id: `actions`,
        render: () => <AssemblyTagActions />,
      },
    ],
    [],
  )

  const handleDoubleClick = (id: any) => {
    console.log(`handleDoubleClick`, id)
    navigate(`/app/assemblies/tags/${id}/edit`)
  }

  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Assembly Tags`}>
        {/* description */}
      </PageSectionIntro>
      <Card.Root>
        <Card.Main>
          <Collection.Root endpoint={`/api/assemblies/tags`}>
            <Collection.Header>
              <Collection.Filters />
              <Collection.Actions>
                <FormButton
                  onClick={() => navigate(`/app/assemblies/tags/create`)}
                >
                  <div className={`mr-2 h-5 w-5`}>
                    <PlusCircleIcon />
                  </div>
                  Create a New Tag
                </FormButton>
              </Collection.Actions>
            </Collection.Header>
            <Collection.Data
              columns={columns}
              handleDoubleClick={handleDoubleClick}
            />
            <Collection.Footer>
              <Collection.Pagination />
            </Collection.Footer>
          </Collection.Root>
        </Card.Main>
      </Card.Root>
    </PageSectionItems>
  )
}
