import { useContext } from 'react'

import { ArrowDownTrayIcon, Cog6ToothIcon } from '@/components/Icons'

import { Actions } from '@/components'

import JobContext from '../contexts/Job'

export const MaterialSummaryActions = () => {
  const { job }: any = useContext(JobContext)

  if (!job.id) return null

  //

  const exportAsExcel = async () => {
    window.open(
      `/downloads/jobs/${job.id}/reports/material-summary/xlsx`,
      `_blank`,
    )
  }

  const exportAsPDF = async () => {
    window.open(
      `/downloads/jobs/${job.id}/reports/material-summary/pdf`,
      `_blank`,
    )
  }

  //

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
              `}
            >
              <Cog6ToothIcon />
            </div>
          </button>
        </Actions.Trigger>
        <Actions.Portal>
          <Actions.Group>
            <Actions.Title icon={<Cog6ToothIcon />}>
              Material Summary Options
            </Actions.Title>
          </Actions.Group>
          <Actions.Group>
            <Actions.Item
              icon={<ArrowDownTrayIcon />}
              onClick={exportAsExcel}
            >
              Export as Excel
            </Actions.Item>
            {/*
            <Actions.Item
              icon={<ArrowDownTrayIcon />}
              onClick={exportAsPDF}
            >
              Export as PDF
            </Actions.Item>
            */}
          </Actions.Group>
        </Actions.Portal>
      </Actions.Root>
    </>
  )
}
