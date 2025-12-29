import { useMemo } from 'react'

import { Collection as Collection2 } from '@/components/Collection'
import { PartFavorite } from '../../collection/components/PartFavorite'

export const Collection = (props: any) => {
  const { onSelectChange } = props

  const columns = useMemo(
    () => [
      {
        Header: ``,
        accessor: `id`,
        id: `selected`,
      },
      {
        accessor: `favorites`,
        name: ``,
        id: `favorites`,
        render: ({ data, value }: any) => (
          <PartFavorite
            data={data}
            disabled={true}
          />
        ),
      },
      {
        Header: `#`,
        accessor: `id`,
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
    ],
    [],
  )

  return (
    <Collection2.Root endpoint={`/api/parts`}>
      <Collection2.Header>
        <Collection2.Filters
          showMaterialFavorites={true}
          showMaterialCategories={true}
        />
      </Collection2.Header>
      <Collection2.Data
        columns={columns}
        onSelectChange={onSelectChange}
      />
      <Collection2.Footer>
        <Collection2.Pagination />
      </Collection2.Footer>
    </Collection2.Root>
  )
}
