import { useMemo } from 'react'
import { useNavigate } from 'react-router-dom'

import { Form, PageSectionIntro, PageSectionItems } from '@/components'

import { Collection } from '@/components/Collection'
import { Card } from '@/components/Card'
import { PlusCircleIcon } from '@/components/Icons'

import { MemberActions } from './components/MemberActions'

const NameCell = (props: any) => {
  const { value, data } = props

  return (
    <div className={`${data.enabled ? `` : `opacity-30`}`}>
      <div className={`flex items-center space-x-4`}>
        <div className={``}>
          <div className="font-semibold text-black">{value}</div>
          <div className="">{data.email}</div>
        </div>
        {data.principal && <Tag className="text-blue-500">Principal</Tag>}
        {!data.enabled && <Tag className="text-red-500">Canceled</Tag>}
      </div>
    </div>
  )
}

const UsersCell = (props: any) => {
  const { value, data } = props

  if (!value) return null

  return (
    <div className={`${data.enabled ? `` : `opacity-30`}`}>
      <div className={`space-y-2`}>
        {value.map((user: any) => (
          <div
            className={`${user.enabled ? `` : `opacity-30`}`}
            key={user.uuid}
          >
            <div className="font-semibold text-black">{user.name}</div>
            <div>{user.email}</div>
          </div>
        ))}
      </div>
    </div>
  )
}

const StripeCell = (props: any) => {
  const { value, data } = props

  if (!value) return null

  return (
    <div className={`${data.enabled ? `` : `opacity-30`}`}>
      <div>{value}</div>
    </div>
  )
}

export const MemberCollection = () => {
  const navigate = useNavigate()

  const columns = useMemo(
    () => [
      {
        Header: `#`,
        accessor: `id`,
      },
      {
        Header: `Member`,
        accessor: `name`,
        width: `*`,
        render: ({ value, data }: any) => (
          <NameCell
            value={value}
            data={data}
          />
        ),
      },
      {
        Header: `Users`,
        accessor: `users`,
        width: `*`,
        render: ({ value, data }: any) => (
          <UsersCell
            value={value}
            data={data}
          />
        ),
      },
      {
        Header: `Stripe`,
        accessor: `customer`,
        width: `*`,
        render: ({ value, data }: any) => (
          <StripeCell
            value={value}
            data={data}
          />
        ),
      },
      {
        accessor: `id`,
        name: ``,
        id: `actions`,
        render: () => <MemberActions />,
      },
    ],
    [],
  )

  const handleDoubleClick = (id: any) => {
    console.log(`handleDoubleClick`, id)
    navigate(`/app/members/all/${id}/edit`)
  }

  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Member List`}>
        Create new members, edit existing members.
      </PageSectionIntro>
      <Card.Root>
        <Card.Main>
          <Collection.Root endpoint={`/api/members`}>
            <Collection.Header>
              <Collection.Filters />
              <Collection.Actions>
                <Form.Button
                  onClick={() => navigate(`/app/members/all/create`)}
                >
                  <div className={`mr-2 h-5 w-5`}>
                    <PlusCircleIcon />
                  </div>
                  Create a New Member
                </Form.Button>
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

const Tag = (props: any) => {
  const { children, className } = props

  return (
    <div className={className}>
      <div
        className={`-m-px flex items-center justify-center rounded-full border border-current bg-current py-0.5 px-2`}
      >
        <span className={`whitespace-nowrap text-xs text-white`}>
          {children}
        </span>
      </div>
    </div>
  )
}
