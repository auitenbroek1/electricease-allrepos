import { useMemo } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { PlusCircleIcon } from '@/components/Icons'

import { Card } from '@/components/Card'

import { FormButton, PageSectionIntro, PageSectionItems } from '@/components'

import { Collection } from '@/components/Collection'

import { AssemblyActions } from './components/AssemblyActions'
import { AssemblyFavorite } from './components/AssemblyFavorite'
import { currency, number } from '@/utilities/format'

export const AssemblyCollection = (props: any) => {
  const navigate = useNavigate()

  const columns = useMemo(
    () => [
      {
        accessor: `favorites`,
        name: ``,
        id: `favorites`,
        render: ({ data, value }: any) => (
          <AssemblyFavorite
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
        render: () => <AssemblyActions />,
      },
    ],
    [],
  )

  const handleDoubleClick = (id: any) => {
    console.log(`handleDoubleClick`, id)
    navigate(`/app/assemblies/all/${id}/edit`)
  }

  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Assembly Vault`}>
        {/* description */}
      </PageSectionIntro>
      <Card.Root>
        <Card.Main>
          <Collection.Root endpoint={`/api/assemblies`}>
            <Collection.Header>
              <Collection.Filters
                showAssemblyFavorites={true}
                showAssemblyCategories={true}
              />
              <Collection.Actions>
                <FormButton
                  onClick={() => navigate(`/app/assemblies/all/create`)}
                >
                  <div className={`mr-2 h-5 w-5`}>
                    <PlusCircleIcon />
                  </div>
                  Create a New Assembly
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
