import { useMemo } from 'react'

import { Collection } from '@/components/Collection'
import { AssemblyFavorite } from './AssemblyFavorite'

export const AssemblyModalCollection = (props: any) => {
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
          <AssemblyFavorite
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
    <Collection.Root endpoint={`/api/assemblies`}>
      <Collection.Header>
        <Collection.Filters
          showAssemblyFavorites={true}
          showAssemblyCategories={true}
        />
      </Collection.Header>
      <Collection.Data
        columns={columns}
        onSelectChange={onSelectChange}
      />
      <Collection.Footer>
        <Collection.Pagination />
      </Collection.Footer>
    </Collection.Root>
  )
}
