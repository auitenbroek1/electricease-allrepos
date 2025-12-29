import { useMemo } from 'react'
import { Link, useNavigate } from 'react-router-dom'

import { Card } from '@/components/Card'
import { Collection } from '@/components/Collection'
import { FormButton, PageSectionIntro, PageSectionItems } from '@/components'

import { PartCategoryActions } from './PartCategoryActions'
import { PlusCircleIcon } from '@/components/Icons'

export const PartCategoryCollection = () => {
  const navigate = useNavigate()

  const columns = useMemo(
    () => [
      {
        Header: `#`,
        accessor: `id`,
      },
      {
        Header: `Name`,
        accessor: `name`,
        width: `*`,
      },
      {
        Header: ``,
        accessor: `id`,
        id: `actions`,
        render: () => <PartCategoryActions />,
      },
    ],
    [],
  )

  const handleDoubleClick = (id: any) => {
    console.log(`handleDoubleClick`, id)
    navigate(`/app/parts/categories/${id}/edit`)
  }

  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Material Categories`}>
        {/* description */}
      </PageSectionIntro>
      <Card.Root>
        <Card.Main>
          <Collection.Root endpoint={`/api/parts/categories`}>
            <Collection.Header>
              <Collection.Filters />
              <Collection.Actions>
                <FormButton
                  onClick={() => navigate(`/app/parts/categories/create`)}
                >
                  <div className={`mr-2 h-5 w-5`}>
                    <PlusCircleIcon />
                  </div>
                  Create a New Category
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
