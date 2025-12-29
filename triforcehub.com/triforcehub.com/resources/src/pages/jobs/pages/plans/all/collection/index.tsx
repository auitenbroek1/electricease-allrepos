import { useContext, useEffect, useMemo } from 'react'
import { Link, useNavigate, useParams } from 'react-router-dom'

import { Card } from '@/components/Card'
import { Collection } from '@/components/Collection'
import { FormButton, PageSectionIntro, PageSectionItems } from '@/components'

import { PlanActions } from './components/PlanActions'
import { PlusCircleIcon } from '@/components/Icons'

import JobContext from '@/pages/jobs/contexts/Job'

export const CollectionRoutes = () => {
  const { job }: any = useContext(JobContext)

  const params = useParams()

  console.log(`debug.routes`, params)

  //

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
        render: ({ data }: any) => {
          return data?.upload?.name
        },
      },
      {
        Header: ``,
        accessor: `id`,
        id: `actions`,
        render: () => <PlanActions />,
      },
    ],
    [],
  )

  const handleDoubleClick = (id: any) => {
    console.log(`handleDoubleClick`, id)
    navigate(`/app/jobs/all/${job.id}/plans/all/${id}/takeoff`)
  }

  if (!job) return null

  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Plans`}>
        Upload and manage bid plans.
      </PageSectionIntro>
      <Card.Root>
        <Card.Main>
          <Collection.Root endpoint={`/api/jobs/${job.id}/plans`}>
            <Collection.Header>
              <Collection.Filters />
              <Collection.Actions>
                <FormButton
                  onClick={() =>
                    navigate(`/app/jobs/all/${job.id}/plans/all/create`)
                  }
                >
                  <div className={`mr-2 h-5 w-5`}>
                    <PlusCircleIcon />
                  </div>
                  Create a New Plan
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
