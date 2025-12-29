import { Card, Form } from '@/components'

import { useUser } from '@/hooks'
import { useCard } from '@/components/Card/Context'
import { useState } from 'react'

import { useMemberContext } from '../contexts/MemberContext'

import axios from 'axios'
import { useAuth } from '@/contexts/AuthContext'
import { useNavigate } from 'react-router-dom'

export const User = (props: any) => {
  const { data, onCancel } = props

  return (
    <Card.Root open={data.id ? false : true}>
      <Card.HeaderNew>
        <div className="flex items-center space-x-4">
          <div>{data.name ?? `New User`}</div>
          {data.administrator && (
            <div>
              <div
                className={`-m-px flex items-center justify-center rounded-full border border-gray-400 px-2 py-0.5`}
              >
                <div className={`whitespace-nowrap text-xs`}>Administrator</div>
              </div>
            </div>
          )}
          {data.principal && (
            <div>
              <div
                className={`-m-px flex items-center justify-center rounded-full border border-red-500 bg-red-500 py-0.5 px-2`}
              >
                <span className={`whitespace-nowrap text-xs text-white`}>
                  Principal
                </span>
              </div>
            </div>
          )}
        </div>
      </Card.HeaderNew>
      <Card.Main>
        <UserForm
          data={data}
          onCancel={onCancel}
        />
      </Card.Main>
    </Card.Root>
  )
}

const UserForm = (props: any) => {
  const { data, onCancel } = props

  const { setOpen } = useCard()

  const { reloadMember } = useMemberContext()

  const {
    name_ref,
    email_ref,
    password_ref,
    password_confirmation_ref,
    asyncSave,
  } = useUser({ initial: data })

  const [errors, setErrors] = useState<any>({})

  const handleCancel = async () => {
    onCancel && onCancel()
    setOpen(false)
  }

  const handleSave = async () => {
    try {
      await asyncSave()
      console.log(`debug.user`, `success!!!`)
      onCancel && onCancel()
      reloadMember()
    } catch (errors) {
      console.log(`debug.user`, `error!!!`, errors)
      setErrors(errors)
    }
  }

  //

  const navigate = useNavigate()
  const { user, refetch } = useAuth()

  const impersonate = async () => {
    try {
      await axios.post(`/api/impersonation`, {
        subject: data.id,
      })
      navigate(`/app/home`)
      await refetch()
    } catch (error) {
      console.log(error)
    }
  }

  //

  return (
    <form
      autoComplete={`off`}
      className={`m-0`}
    >
      <Card.Columns number={2}>
        <Card.Column>
          <Card.Group>
            <Card.Header title={`Contact Information`}>
              Contact information for this user.
            </Card.Header>
            <hr />
            <Form.Controls cols={1}>
              <Form.Control
                errors={errors?.name}
                field={
                  <Form.Input
                    defaultValue={data.name}
                    ref={name_ref}
                  />
                }
                label={`Name`}
                required={true}
              />
            </Form.Controls>
            <Form.Controls cols={1}>
              <Form.Control
                errors={errors?.email}
                field={
                  <Form.Input
                    defaultValue={data.email}
                    ref={email_ref}
                  />
                }
                label={`Email Address`}
                required={true}
              />
            </Form.Controls>
          </Card.Group>
        </Card.Column>
        <Card.Column>
          <Card.Group>
            <Card.Header title={`Login Information`}>
              Login information for this user.
            </Card.Header>
            <hr />
            <Form.Controls cols={1}>
              <Form.Control
                errors={errors?.password}
                field={
                  <Form.Input
                    defaultValue={data.password}
                    ref={password_ref}
                    type={`password`}
                  />
                }
                label={`Password`}
                required={true}
              />
            </Form.Controls>
            <Form.Controls cols={1}>
              <Form.Control
                errors={errors?.password_confirmation}
                field={
                  <Form.Input
                    defaultValue={data.password_confirmation}
                    ref={password_confirmation_ref}
                    type={`password`}
                  />
                }
                label={`Confirm Password`}
                required={true}
              />
            </Form.Controls>
          </Card.Group>
        </Card.Column>
        <Card.Column span={2}>
          <hr />
          <Form.Buttons>
            {data.id && !data.administrator && user.administrator ? (
              <Form.Button
                onClick={impersonate}
                type={`secondary`}
              >
                Impersonate
              </Form.Button>
            ) : null}
            <Form.Button
              onClick={handleCancel}
              type={`secondary`}
            >
              Cancel
            </Form.Button>
            <Form.Button
              onClick={handleSave}
              type={`primary`}
            >
              Save
            </Form.Button>
          </Form.Buttons>
        </Card.Column>
      </Card.Columns>
    </form>
  )
}
