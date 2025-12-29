import { useMemo } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { PlusCircleIcon } from '@/components/Icons'

import { Card } from '@/components/Card'

import { FormButton } from '@/components'

import { Collection as Collection2 } from '@/components/Collection'

import { PartActions } from './PartActions'
import { PartFavorite } from './PartFavorite'
import { currency, number } from '@/utilities/format'

export const Collection = (props: any) => {
  const navigate = useNavigate()

  const columns = useMemo(
    () => [
      {
        accessor: `favorites`,
        name: ``,
        id: `favorites`,
        render: ({ data, value }: any) => (
          <PartFavorite
            key={data.id}
            data={data}
          />
        ),
      },
      {
        accessor: `id`,
        name: `#`,
      },
      {
        accessor: `categories`,
        name: `Categories`,
        render: ({ value }: any) => {
          // return JSON.stringify(value)
          const names = []
          if (value) {
            for (const item of value) {
              names.push(item?.name)
            }
          }
          return names.join(`, `)
        },
      },
      {
        accessor: `name`,
        name: `Name`,
        width: `*`,
        render: ({ data, value }: any) => {
          return (
            <div className={`flex space-x-4`}>
              <div>{value}</div>
              <div className={`flex space-x-4 text-xs`}>
                {data?.tags?.map((tag: any) => (
                  <div
                    className={`flex items-center space-x-2`}
                    key={tag.id}
                    title={tag.name}
                  >
                    <span
                      className={`block h-4 w-4 rounded-full`}
                      style={{ backgroundColor: tag.color }}
                    ></span>
                    <span>{tag.name}</span>
                  </div>
                ))}
              </div>
            </div>
          )
        },
      },
      {
        accessor: `cost`,
        name: `Cost`,
        render: ({ value }: any) => {
          return currency(value)
        },
      },
      {
        accessor: `labor`,
        name: `Labor`,
        render: ({ value }: any) => {
          return number(value)
        },
      },
      {
        accessor: `id`,
        name: ``,
        id: `actions`,
        render: () => <PartActions />,
      },
    ],
    [],
  )

  const handleDoubleClick = (id: any) => {
    console.log(`handleDoubleClick`, id)
    navigate(`/app/parts/all/${id}/edit`)
  }

  return (
    <Collection2.Root endpoint={`/api/parts`}>
      <Collection2.Header>
        <Collection2.Filters
          showMaterialFavorites={true}
          showMaterialCategories={true}
        />
        <Collection2.Actions>
          <FormButton onClick={() => navigate(`/app/parts/all/create`)}>
            <div className={`mr-2 h-5 w-5`}>
              <PlusCircleIcon />
            </div>
            Create New Material
          </FormButton>
        </Collection2.Actions>
      </Collection2.Header>
      <Collection2.Data
        columns={columns}
        handleDoubleClick={handleDoubleClick}
      />
      <Collection2.Footer>
        <Collection2.Pagination />
      </Collection2.Footer>
    </Collection2.Root>
  )
}
