import { useContext, useRef, useState } from 'react'

import { ModalForm, FormText, FormButton } from '@/components'

import Phase from './Phase'
import JobContext from '../contexts/Job'
import axios from 'axios'

const Component = (props: any) => {
  const { data } = props

  const { job, setJob }: any = useContext(JobContext)

  //

  const addNewPhase = async (params: any) => {
    const { name, area, cost_code } = params

    console.log(`add new phase`, { name, area, cost_code })

    const response = await axios.post(`/api/jobs/phases`, {
      job_id: job.id,
      name,
      area,
      cost_code,
    })

    console.log(`response`, response)

    const response2 = await axios.get(`/api/jobs/${job.id}`)
    const data2 = response2.data.data

    setJob(data2)
  }

  const [isPhaseModalOpen, setIsPhaseModalOpen] = useState(false)

  const newPhaseNameInput: any = useRef(null)
  const newPhaseAreaInput: any = useRef(null)
  const newPhaseCodeInput: any = useRef(null)
  const showModal = () => {
    setIsPhaseModalOpen(true)
  }
  const hideModal = () => {
    setIsPhaseModalOpen(false)
  }
  const handleAddNewPhase = () => {
    const name = newPhaseNameInput?.current?.value
    const area = newPhaseAreaInput?.current?.value
    const cost_code = newPhaseCodeInput?.current?.value
    addNewPhase({ name, area, cost_code })
    setIsPhaseModalOpen(false)
  }

  //

  return (
    <>
      <div className={`space-y-4`}>
        {data.length > 1 &&
          data.map((item: any, index: number) => (
            <Phase
              key={item.uuid}
              item={item}
              withActions={true}
            />
          ))}
        {data.length === 1 &&
          data.map((item: any, index: number) => {
            if (data.length === 1) {
              item.open = true
            }
            return (
              <Phase
                item={item}
                key={index}
                withActions={false}
              />
            )
          })}
      </div>
      <div className={`flex items-center justify-center`}>
        <FormButton onClick={() => showModal()}>Add a New Phase</FormButton>
      </div>
      <ModalForm
        content={
          <div className={`space-y-4`}>
            <div>
              <FormText
                label={`Name`}
                ref={newPhaseNameInput}
              />
            </div>
            <div>
              <FormText
                label={`Area`}
                ref={newPhaseAreaInput}
              />
            </div>
            <div>
              <FormText
                label={`Cost Code`}
                ref={newPhaseCodeInput}
              />
            </div>
          </div>
        }
        description={``}
        onClose={hideModal}
        onSubmit={handleAddNewPhase}
        open={isPhaseModalOpen}
        title={`Add a new phase`}
      />
    </>
  )
}

export default Component
