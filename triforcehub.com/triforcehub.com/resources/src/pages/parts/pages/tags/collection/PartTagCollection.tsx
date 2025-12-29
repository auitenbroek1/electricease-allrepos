import { useMemo } from 'react'
import { useNavigate } from 'react-router-dom'

import { Card } from '@/components/Card'
import { Collection } from '@/components/Collection'
import { FormButton, PageSectionIntro, PageSectionItems } from '@/components'

import { PartTagActions } from './PartTagActions'
import { PlusCircleIcon } from '@/components/Icons'

export const PartTagCollection = () => {
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
        render: () => <PartTagActions />,
      },
    ],
    [],
  )

  const handleDoubleClick = (id: any) => {
    console.log(`handleDoubleClick`, id)
    navigate(`/app/parts/tags/${id}/edit`)
  }

  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Material Tags`}>
        {/* description */}
      </PageSectionIntro>
      <Card.Root>
        <Card.Main>
          <Collection.Root endpoint={`/api/parts/tags`}>
            <Collection.Header>
              <Collection.Filters />
              <Collection.Actions>
                <FormButton onClick={() => navigate(`/app/parts/tags/create`)}>
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
