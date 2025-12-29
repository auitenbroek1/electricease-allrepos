import { useMemo } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { PlusCircleIcon } from '@/components/Icons'

import { Card } from '@/components/Card'
import { FormButton, PageSectionIntro, PageSectionItems } from '@/components'
import { Collection } from '@/components/Collection'
import { JobActions } from './components/JobActions'

import { currency } from '@/utilities/format'

export const JobCollection = (props: any) => {
  const { onSelectChange } = props

  const navigate = useNavigate()

  const columns = useMemo(
    () => [
      {
        Header: ``,
        accessor: `id`,
        id: `toggle`,
      },
      {
        Header: `#`,
        accessor: `id`,
      },
      {
        Header: `Status`,
        accessor: `status`,
        render: ({ value, data }: any) => {
          const items = []
          if (data?.status?.parent) {
            items.push(data.status.parent.name)
          }
          if (value?.name) {
            items.push(value.name)
          }
          return items.join(` / `)
        },
      },
      {
        Header: `Number`,
        accessor: `number`,
      },
      {
        Header: `Name`,
        accessor: `name`,
        width: `*`,
      },
      {
        Header: `Customer`,
        accessor: `customers`,
        render: ({ data }: any) => {
          const customers = data?.customers?.map(
            (customer: any) => customer.name,
          )
          if (customers?.length) {
            return customers.join(`, `)
          }
          return null
        },
        renderChild: ({ data }: any) => {
          const customers = data?.parent?.customers?.map(
            (customer: any) => customer.name,
          )
          if (customers?.length) {
            return customers.join(`, `)
          }
          return null
        },
      },
      {
        Header: `Proposal Sent`,
        accessor: `proposal_sent_at`,
        render: ({ value }: any) => {
          return value ?? `N/A`
        },
      },
      {
        Header: `Temporary Power`,
        accessor: `temporary_power`,
        render: ({ value }: any) => {
          return value ? `Yes` : `No`
        },
      },
      {
        Header: `Temporary Lighting`,
        accessor: `temporary_lighting`,
        render: ({ value }: any) => {
          return value ? `Yes` : `No`
        },
      },
      {
        Header: `Square Footage`,
        accessor: `sqft`,
        render: ({ value }: any) => {
          return value ?? `N/A`
        },
      },
      {
        Header: `Base Bid Price`,
        accessor: `summary`,
        render: ({ data }: any) => {
          const price = data?.summary?.adjustments?.summary?.cost_total ?? 0
          if (price) {
            return currency(price)
          }
          return `N/A`
        },
      },
      {
        Header: `Square Foot Price`,
        render: ({ data }: any) => {
          const price = data?.summary?.adjustments?.summary?.cost_total ?? 0
          const sqft = data?.sqft ?? 0
          // console.log(price, sqft)
          if (price && sqft) {
            return currency(price / sqft)
          }
          return `N/A`
        },
      },
      {
        Header: ``,
        accessor: `id`,
        id: `actions`,
        render: () => <JobActions />,
        renderChild: () => <JobActions child={true} />,
      },
    ],
    [],
  )

  const handleDoubleClick = (id: any) => {
    console.log(`handleDoubleClick`, id)
    navigate(`/app/jobs/all/${id}/edit`)
  }

  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Bid List`}>
        Create new job, edit previous jobs, duplicate existing job, or add
        change orders to existing jobs.
      </PageSectionIntro>
      <Card.Root>
        <Card.Main>
          <Collection.Root endpoint={`/api/jobs`}>
            <Collection.Header>
              <Collection.Filters />
              <Collection.Actions>
                <FormButton onClick={() => navigate(`/app/jobs/all/create`)}>
                  <div className={`mr-2 h-5 w-5`}>
                    <PlusCircleIcon />
                  </div>
                  Create a New Bid
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
