import axios from 'axios'
import { Card, Form, PageSectionItem } from '@/components'
import JobsContext from '@/pages/jobs/contexts/Job'
import { useContext, useEffect, useState } from 'react'
import toast from 'react-hot-toast'

const WorkOrder = (props: any) => {
  const { data } = props

  const { job }: any = useContext(JobsContext)

  const [to, setTo] = useState<any>(``)
  const [cc, setCC] = useState<any>(``)
  const [subject, setSubject] = useState<any>(``)

  //

  const [attachments, setAttachments] = useState<any>([`work-order`])

  const attachmentOptions = [
    { id: `work-order`, name: `Work Order`, required: true },
    { id: `material-summary`, name: `Material Summary` },
  ]

  const handleAttachmentsChange = (event: any) => {
    const element = event.target
    const filtered = attachments.filter((attachment: string) => {
      return attachment !== element.value
    })
    console.log(filtered)
    if (element.checked) {
      filtered.push(element.value)
    }
    setAttachments([...filtered])
  }

  //

  const handleSend = async () => {
    const endpoint = `/api/jobs/${job.id}/${data.id}/work-order`

    const payload = {
      to,
      cc,
      subject,
      attachments,
    }

    try {
      const response = await axios.post(endpoint, payload)

      console.log(response?.data)

      toast.success(`Done!`)
    } catch (error: any) {
      console.log(error, error?.response)

      toast.error(error.message)
    }
  }

  useEffect(() => {
    if (!job.id) return
    setSubject(`Work Order for ${data.name} - ${job.name}`)
  }, [job?.id, data?.name])

  return (
    <PageSectionItem
      heading={data.name}
      openByDefault={true}
    >
      <Card.Columns number={1}>
        <Card.Column>
          <Card.Group>
            <Form.Controls cols={2}>
              <Form.Control
                label={`To`}
                field={
                  <Form.Input
                    onChange={(event: any) => setTo(event.target.value)}
                    type={`email`}
                    value={to}
                  />
                }
              />
              <Form.Control
                label={`CC`}
                field={
                  <Form.Input
                    onChange={(event: any) => setCC(event.target.value)}
                    type={`email`}
                    value={cc}
                  />
                }
              />
              <Form.Control
                cols={2}
                label={`Subject`}
                field={
                  <Form.Input
                    onChange={(event: any) => setSubject(event.target.value)}
                    type={`text`}
                    value={subject}
                  />
                }
              />
              <Form.Control
                label={`Attachments`}
                field={
                  <div className={`text-sm`}>
                    {attachmentOptions.map((attachmentOption) => (
                      <div key={attachmentOption.id}>
                        <label
                          className={`inline-flex items-center space-x-2 ${
                            attachmentOption.required
                              ? `pointer-events-none opacity-50`
                              : `cursor-pointer`
                          }`}
                        >
                          <input
                            defaultChecked={attachmentOption.required}
                            onChange={handleAttachmentsChange}
                            readOnly={attachmentOption.required}
                            type={`checkbox`}
                            value={attachmentOption.id}
                          />
                          <span>{attachmentOption.name}</span>
                        </label>
                      </div>
                    ))}
                  </div>
                }
              />
            </Form.Controls>
          </Card.Group>
          <hr />
          <Card.Group>
            <div className={`overflow-hidden`}>
              <div className={`-m-8 h-[600px]`}>
                <iframe
                  className={`h-full w-full`}
                  src={`/emails/jobs/${job.id}/${data.id}/work-order/attachments/work-order`}
                />
              </div>
            </div>
          </Card.Group>
          <hr />
          <Form.Buttons>
            <Form.Button
              onClick={handleSend}
              type={`primary`}
            >
              Send
            </Form.Button>
          </Form.Buttons>
        </Card.Column>
      </Card.Columns>
    </PageSectionItem>
  )
}

export default WorkOrder
