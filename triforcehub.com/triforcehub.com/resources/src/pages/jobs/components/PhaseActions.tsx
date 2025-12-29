import { useContext, useRef, useState } from 'react'

import {
  Cog6ToothIcon,
  DocumentDuplicateIcon,
  ExclamationTriangleIcon,
  PencilSquareIcon,
  TrashIcon,
} from '@/components/Icons'

import JobContext from '../contexts/Job'
import PhaseContext from '../contexts/Phase'

import { Actions, ModalForm, FormText, Modal } from '@/components'

import axios from 'axios'

const Component = () => {
  const { job, setJob }: any = useContext(JobContext)
  const phase: any = useContext(PhaseContext)

  // rename

  const addNewPhase = async (params: any) => {
    const { name, area, cost_code } = params

    console.log(`add new phase`, { name, area, cost_code })

    const response = await axios.patch(`/api/jobs/phases/${phase.id}`, {
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
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)

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

  // Added Funciton to create duplicate for the selected Phase
  const handleDuplicate = async () => {
    console.log(`duplicate`)
    const response = await axios.post(`/api/jobs/phases/${phase.id}/duplicate`)

    const response2 = await axios.get(`/api/jobs/${job.id}`)
    const data2 = response2.data.data

    setJob(data2)
  }

  //

  // delete

  const handleDelete = async () => {
    const response = await axios.delete(`/api/jobs/phases/${phase.id}`)
    console.log(`response`, response)

    const response2 = await axios.get(`/api/jobs/${job.id}`)
    const data2 = response2.data.data

    setJob(data2)
  }

  return (
    <>
      <Actions.Root>
        <Actions.Trigger>
          <button
            className={`
              group
              -m-2
              rounded-full
              p-2
              hover:bg-white
              hover:bg-opacity-10
              focus:outline-none
            `}
            type={`button`}
          >
            <div
              className={`
                h-5
                w-5
                text-white
                opacity-80
                group-hover:opacity-100
              `}
            >
              <Cog6ToothIcon />
            </div>
          </button>
        </Actions.Trigger>
        <Actions.Portal>
          <Actions.Group>
            <Actions.Title icon={<Cog6ToothIcon />}>
              Phase Options
            </Actions.Title>
          </Actions.Group>
          <Actions.Group>
            <Actions.Item
              icon={<PencilSquareIcon />}
              onClick={showModal}
            >
              Edit
            </Actions.Item>
          </Actions.Group>
          <Actions.Group>
            <Actions.Item
              icon={<DocumentDuplicateIcon />}
              onClick={handleDuplicate}
            >
              Duplicate
            </Actions.Item>
          </Actions.Group>
          <Actions.Group>
            <Actions.Item
              danger={true}
              icon={<TrashIcon />}
              onClick={() => setIsDeleteModalOpen(true)}
            >
              Delete
            </Actions.Item>
          </Actions.Group>
        </Actions.Portal>
      </Actions.Root>
      <ModalForm
        content={
          <div className={`space-y-4`}>
            <div>
              <FormText
                label={`Name`}
                ref={newPhaseNameInput}
                value={phase.name}
              />
            </div>
            <div>
              <FormText
                label={`Area`}
                ref={newPhaseAreaInput}
                value={phase.area}
              />
            </div>
            <div>
              <FormText
                label={`Cost Code`}
                ref={newPhaseCodeInput}
                value={phase.cost_code}
              />
            </div>
          </div>
        }
        description={``}
        onClose={hideModal}
        onSubmit={handleAddNewPhase}
        open={isPhaseModalOpen}
        title={`Edit this phase`}
      />

      <Modal.Root
        open={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
      >
        <Modal.Content>
          <div className="mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full bg-red-100 sm:mx-0 sm:h-10 sm:w-10">
            <div className="h-6 w-6 text-red-600">
              <ExclamationTriangleIcon />
            </div>
          </div>
          <div className="mt-3 text-center sm:mt-0 sm:ml-4 sm:text-left">
            <div className="text-lg font-medium leading-6">Delete phase</div>
            <div className="mt-2">
              <p className="text-sm text-gray-500">
                Are you sure you want to delete this phase?
                {/* All of your data will be permanently removed. */}
                {` `}
                This action cannot be undone.
              </p>
            </div>
          </div>
        </Modal.Content>
        <Modal.Buttons>
          <button
            type="button"
            className="mt-3 inline-flex w-full justify-center rounded-md border border-gray-300 bg-white px-4 py-2 text-base font-medium text-gray-700 shadow-sm hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
            onClick={() => setIsDeleteModalOpen(false)}
          >
            Cancel
          </button>
          <button
            type="button"
            className="inline-flex w-full justify-center rounded-md border border-transparent bg-red-600 px-4 py-2 text-base font-medium text-white shadow-sm hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2 sm:ml-3 sm:w-auto sm:text-sm"
            onClick={handleDelete}
          >
            Delete
          </button>
        </Modal.Buttons>
      </Modal.Root>
    </>
  )
}

export default Component
