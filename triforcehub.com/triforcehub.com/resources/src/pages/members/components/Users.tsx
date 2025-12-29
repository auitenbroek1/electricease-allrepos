import { Form } from '@/components'
import { useState } from 'react'

import { useMemberContext } from '../contexts/MemberContext'

import { User } from './User'

export const Users = () => {
  const { member } = useMemberContext()

  const [showNewForm, setShowNewForm] = useState(false)

  if (!member) return null

  return (
    <div className="space-y-4">
      {member?.users?.map((user: any) => (
        <User
          data={user}
          key={user.uuid}
        />
      ))}
      {showNewForm ? (
        <User
          data={{ member_id: member?.id }}
          onCancel={() => setShowNewForm(false)}
        />
      ) : (
        <div className={`mt-4 flex justify-center`}>
          <Form.Button onClick={() => setShowNewForm(true)}>
            Add a New User
          </Form.Button>
        </div>
      )}
    </div>
  )
}
