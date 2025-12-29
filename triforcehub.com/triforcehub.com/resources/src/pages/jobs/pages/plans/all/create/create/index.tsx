import { useContext, useEffect, useRef, useState } from 'react'
import { useNavigate } from 'react-router-dom'

import { useAuth } from '@/contexts/AuthContext'

import { Card, Form } from '@/components'
import { PageSectionIntro, PageSectionItems } from '@/components'
import axios from 'axios'

import JobContext from '@/pages/jobs/contexts/Job'

import { Upload } from '../../create/create/components/Upload'

export const CreatePage = (props: any) => {
  const { job }: any = useContext(JobContext)

  //

  const data: any = {}

  const name_ref = useRef<HTMLInputElement>(null)
  const upload_id_ref = useRef<HTMLInputElement>(null)

  const [errors, setErrors] = useState<any>({})

  //

  const [show_new_uploader, set_show_new_uploader] = useState<boolean>(false)

  const { user } = useAuth()

  useEffect(() => {
    console.log(`test`, user)
    set_show_new_uploader(true)
    // if ([1, 2, 4, 53, 55, 90, 303, 307, 315, 324, 352, 366].includes(user.member_id)) {
    //   set_show_new_uploader(true)
    // }
  }, [user?.member_id])

  //

  const navigate = useNavigate()

  const onCancelClick = () => {
    navigate(`/app/jobs/all/${job.id}/plans/all`)
  }

  const onSaveClick = async () => {
    const name = name_ref?.current?.value ?? ``
    const job_id = job.id
    const upload_id = upload_id_ref?.current?.value ?? ``

    const payload = {
      job_id,
      name,
      upload_id,
    }

    console.log(`debug.form`, payload)

    try {
      const response: any = await axios.post(
        `/api/jobs/${job.id}/plans`,
        payload,
      )
      const data2 = response?.data?.data
      console.log(`debug.form`, `success!!!`, data2)
      setErrors(null)
      navigate(`/app/jobs/all/${job.id}/plans/all/${data2.id}/takeoff`)
    } catch (errors: any) {
      const response = errors?.response?.data
      console.log(`debug.form`, `error!!!`, response)
      setErrors(response.errors)
    }
  }

  if (!job?.id) return null

  //

  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Plan Details`}>
        Upload a plan to this bid.
      </PageSectionIntro>
      {show_new_uploader ? (
        <Upload job={job} />
      ) : (
        <Card.Root key={data.id}>
          <Card.Main>
            <form
              autoComplete={`off`}
              className={`m-0`}
            >
              <Card.Columns number={2}>
                <Card.Column>
                  <Card.Group>
                    <Form.Controls cols={1}>
                      <Form.Control
                        errors={errors?.logo_id}
                        field={
                          <Form.Upload
                            data={data.logo}
                            ref={upload_id_ref}
                          />
                        }
                        label={`Plan`}
                      />
                    </Form.Controls>
                  </Card.Group>
                </Card.Column>
                <Card.Column>
                  <Card.Group>&nbsp;</Card.Group>
                </Card.Column>
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
      )}
    </PageSectionItems>
  )
}
