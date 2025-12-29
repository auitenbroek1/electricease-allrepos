import { Form } from '@/components/Form'
import { Card } from '@/components/Card'
import FormText from '@/components/Form/FormText'
import FormButton from '@/components/Form/FormButton'
import { ArrowDownTrayIcon } from '@/components/Icons'
import { useEffect, useRef, useState } from 'react'
import axios from 'axios'

import { MemberProvider } from '@/pages/members/contexts/MemberContext'
import { Details } from '../members/components/Details'
import { PageSectionIntro, PageSectionItems } from '@/components'

import { useAuth } from '@/contexts/AuthContext'

export const ProfileIndex = () => {
  const { user } = useAuth()

  const [success, setSuccess] = useState<any>(``)
  const [errors, setErrors] = useState<any>([])

  const password1Ref = useRef<HTMLInputElement>(null)
  const password2Ref = useRef<HTMLInputElement>(null)

  const onSaveClick = async () => {
    const payload = {
      password1: password1Ref?.current?.value,
      password2: password2Ref?.current?.value,
    }

    console.log(`debug.save`, payload)

    try {
      const response = await axios.patch(`/api/profile`, payload)
      console.log(response.data)
      if (response.data.success) {
        setSuccess(`Your password has been updated!`)
        setErrors([])
      } else {
        setErrors([`An error occurred, please try again later.`])
        setSuccess(``)
      }
    } catch {
      setErrors([`An error occurred, please try again later.`])
      setSuccess(``)
    }
  }

  if (!user?.member_id) return null

  //

  return (
    <>
      <PageSectionItems>
        <PageSectionIntro heading={`Member Details`}>
          General information about the member account.
        </PageSectionIntro>
        <MemberProvider id={user?.member_id}>
          <Details profile={true} />
        </MemberProvider>
      </PageSectionItems>
      <div className={`mt-8`}></div>
      <PageSectionItems>
        <PageSectionIntro heading={`User Details`}>
          Login information for your personal account.
        </PageSectionIntro>
        <Card.Root>
          <Card.Main>
            <Card.Columns>
              <Card.Column>
                <Card.Group>
                  <Form.Controls cols={1}>
                    <Form.Control>
                      <FormText
                        disabled
                        label={`Name`}
                        value={user.name}
                      />
                    </Form.Control>
                  </Form.Controls>
                  <Form.Controls cols={1}>
                    <Form.Control>
                      <FormText
                        disabled
                        label={`Email`}
                        value={user.email}
                      />
                    </Form.Control>
                  </Form.Controls>
                </Card.Group>
                <Card.Group>
                  <Form.Controls>
                    <Form.Control>
                      <FormText
                        label={`Password`}
                        ref={password1Ref}
                        type={`password`}
                      />
                    </Form.Control>
                    <Form.Control>
                      <FormText
                        label={`Confirm Password`}
                        ref={password2Ref}
                        type={`password`}
                      />
                    </Form.Control>
                  </Form.Controls>
                </Card.Group>
                {success && (
                  <div className={`text-center text-green-600`}>{success}</div>
                )}
                {errors &&
                  errors.map((error: any, index: number) => (
                    <div
                      className={`text-center text-red-600`}
                      key={index}
                    >
                      {error}
                    </div>
                  ))}
              </Card.Column>
            </Card.Columns>

            <hr />

            <Form.Buttons>
              <FormButton onClick={onSaveClick}>
                <span className="h-5 w-5">
                  <ArrowDownTrayIcon />
                </span>
                <span>Save</span>
              </FormButton>
            </Form.Buttons>
          </Card.Main>
        </Card.Root>
      </PageSectionItems>
    </>
  )
}
