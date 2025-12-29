import { useContext, useState } from 'react'

import {
  Cog6ToothIcon,
  ExclamationTriangleIcon,
  TrashIcon,
} from '@/components/Icons'

import { Actions, Modal } from '@/components'

import JobContext from '../contexts/Job'
import AssemblyContext from '../contexts/Assembly'

import axios from 'axios'

const AssemblyActions = () => {
  const { job, setJob }: any = useContext(JobContext)
  const assembly: any = useContext(AssemblyContext)

  //

  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)

  //

  const handleEdit = async () => {
    console.log(`edit`)
  }

  //

  const handleDuplicate = async () => {
    console.log(`duplicate`)
  }

  //

  const handleMove = async () => {
    console.log(`move`)
  }

  //

  const handleDelete = async () => {
    const response1 = await axios.delete(`/api/jobs/assemblies/${assembly.id}`)
    console.log(response1)

    const response = await axios.get(`/api/jobs/${job.id}`)
    const data = response.data.data

    setIsDeleteModalOpen(false)

    setJob(data)
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
              hover:bg-brand-gradient-light
              hover:bg-opacity-10
              focus:outline-none
            `}
            type={`button`}
          >
            <div
              className={`
                h-5
                w-5
                text-brand-gradient-light
              `}
            >
              <Cog6ToothIcon />
            </div>
          </button>
        </Actions.Trigger>
        <Actions.Portal>
          <Actions.Group>
            <Actions.Title icon={<Cog6ToothIcon />}>
              Assembly Options
            </Actions.Title>
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
            <div className="text-lg font-medium leading-6">Delete assembly</div>
            <div className="mt-2">
              <p className="text-sm text-gray-500">
                Are you sure you want to delete this assembly?
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

export default AssemblyActions
