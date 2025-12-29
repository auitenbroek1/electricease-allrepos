import { Card, Form } from '@/components'
import { useMember } from '@/hooks'
import { useNavigate } from 'react-router-dom'
import { States } from '@/data/States'
import { useMemberContext } from '../contexts/MemberContext'
import { useState } from 'react'

import { useAuth } from '@/contexts/AuthContext'

export const Details = (props: any) => {
  const { profile } = props

  const { member: data } = useMemberContext()

  const { user } = useAuth()
  const is_admin = user?.member_id === 1

  const {
    name_ref,
    email_ref,
    address1_ref,
    address2_ref,
    city_ref,
    state_id_ref,
    zip_ref,
    mobile_ref,
    office_ref,
    logo_id_ref,

    feature_digital_takeoff_enabled_ref,
    feature_linear_with_drops_enabled_ref,
    feature_auto_count_enabled_ref,

    asyncSave,
  } = useMember({ initial: data })

  const [errors, setErrors] = useState<any>({})

  //

  const navigate = useNavigate()

  const onCancelClick = () => {
    if (profile) return navigate(`/app`)
    navigate(`/app/members/all`)
  }

  const onSaveClick = async () => {
    try {
      const response: any = await asyncSave()
      console.log(`debug.user`, `success!!!`, response)
      setErrors(null)
      navigate(`/app/members/all/${response.id}/edit`)
    } catch (errors) {
      console.log(`debug.user`, `error!!!`, errors)
      setErrors(errors)
    }
  }

  //

  return (
    <Card.Root key={data.id}>
      <Card.Main>
        <form
          autoComplete={`off`}
          className={`m-0`}
        >
          <Card.Columns number={2}>
            <Card.Column>
              <Card.Group>
                <Card.Header title={`Account Information`}>
                  Account information for this member.
                </Card.Header>
                <hr />
                <Form.Controls cols={1}>
                  <Form.Control
                    errors={errors?.name}
                    field={
                      <Form.Input
                        defaultValue={data?.name}
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
                        defaultValue={data?.email}
                        disabled={data?.customer ? true : false}
                        ref={email_ref}
                        type={`email`}
                      />
                    }
                    label={`Email`}
                    required={true}
                  />
                </Form.Controls>
                <Form.Controls cols={1}>
                  <Form.Control
                    errors={errors?.logo_id}
                    field={
                      <Form.Upload
                        data={data.logo}
                        max_width={600}
                        ref={logo_id_ref}
                      />
                    }
                    label={`Logo`}
                  />
                </Form.Controls>
              </Card.Group>
            </Card.Column>
            <Card.Column>
              <Card.Group>
                <Card.Header title={`Contact Information`}>
                  Contact information for this member.
                </Card.Header>
                <hr />
                <Form.Controls>
                  <Form.Control
                    errors={errors?.address1}
                    field={
                      <Form.Input
                        defaultValue={data?.address1}
                        ref={address1_ref}
                      />
                    }
                    label={`Address 1`}
                    required={true}
                  />
                  <Form.Control
                    errors={errors?.address2}
                    field={
                      <Form.Input
                        defaultValue={data?.address2}
                        ref={address2_ref}
                      />
                    }
                    label={`Address 2`}
                  />
                </Form.Controls>
                <Form.Controls cols={3}>
                  <Form.Control
                    errors={errors?.city}
                    field={
                      <Form.Input
                        defaultValue={data?.city}
                        ref={city_ref}
                      />
                    }
                    label={`City`}
                    required={true}
                  />
                  <Form.Control
                    errors={errors?.state_id}
                    field={
                      <Form.Select
                        defaultValue={data?.state_id}
                        options={States}
                        ref={state_id_ref}
                      />
                    }
                    label={`State`}
                    required={true}
                  />
                  <Form.Control
                    errors={errors?.zip}
                    field={
                      <Form.Input
                        defaultValue={data?.zip}
                        ref={zip_ref}
                      />
                    }
                    label={`ZIP`}
                    required={true}
                  />
                </Form.Controls>
                <Form.Controls>
                  <Form.Control
                    errors={errors?.office}
                    field={
                      <Form.Input
                        defaultValue={data?.office}
                        ref={office_ref}
                      />
                    }
                    label={`Office`}
                    required={true}
                  />
                  <Form.Control
                    errors={errors?.mobile}
                    field={
                      <Form.Input
                        defaultValue={data?.mobile}
                        ref={mobile_ref}
                      />
                    }
                    label={`Mobile`}
                  />
                </Form.Controls>
              </Card.Group>
            </Card.Column>
            {!is_admin && data?.customer && (
              <>
                <Card.Column>
                  <Card.Group>
                    <Card.Header title={`Subscription Information`}>
                      Subscription information for this member.
                    </Card.Header>
                    <hr />
                    <div className="space-y-2">
                      <div>
                        <a
                          className="leading-none text-red-500 underline text-sm"
                          href="/app/billing"
                        >
                          Billing Portal &rarr;
                        </a>
                      </div>
                    </div>
                  </Card.Group>
                </Card.Column>
                <Card.Column></Card.Column>
              </>
            )}
            {is_admin && (
              <>
                <Card.Column>
                  <Card.Group>
                    <Card.Header title={`Features`}>
                      Manage features for this member.
                    </Card.Header>
                    <hr />
                    <Form.Controls cols={1}>
                      <div className="space-y-2">
                        <label className="flex cursor-pointer items-center space-x-2">
                          <span>
                            <input
                              defaultChecked={
                                data?.feature_digital_takeoff_enabled
                                  ? true
                                  : false
                              }
                              ref={feature_digital_takeoff_enabled_ref}
                              type={`checkbox`}
                            />
                          </span>
                          <span className="text-sm">Digital Takeoff</span>
                        </label>
                        <label className="flex cursor-pointer items-center space-x-2">
                          <span>
                            <input
                              defaultChecked={
                                data?.feature_linear_with_drops_enabled
                                  ? true
                                  : false
                              }
                              ref={feature_linear_with_drops_enabled_ref}
                              type={`checkbox`}
                            />
                          </span>
                          <span className="text-sm">Linear with Drops</span>
                        </label>
                        <label className="flex cursor-pointer items-center space-x-2">
                          <span>
                            <input
                              defaultChecked={
                                data?.feature_auto_count_enabled ? true : false
                              }
                              ref={feature_auto_count_enabled_ref}
                              type={`checkbox`}
                            />
                          </span>
                          <span className="text-sm">Auto Count</span>
                        </label>
                      </div>
                    </Form.Controls>
                  </Card.Group>
                </Card.Column>
                <Card.Column></Card.Column>
              </>
            )}
            <Card.Column span={2}>
              <hr />
              <Form.Buttons>
                <Form.Button
                  onClick={onCancelClick}
                  type={`secondary`}
                >
                  Cancel
                </Form.Button>
                <Form.Button
                  onClick={onSaveClick}
                  type={`primary`}
                >
                  Save
                </Form.Button>
              </Form.Buttons>
            </Card.Column>
          </Card.Columns>
        </form>
      </Card.Main>
    </Card.Root>
  )
}
