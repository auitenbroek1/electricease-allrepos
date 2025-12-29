import { useContext, useEffect, useMemo, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import JobContext from '../contexts/Job'

import debounce from 'lodash/debounce'

import { Form, FormButton, FormSelect, FormToggle } from '@/components'

import { useController } from '@/hooks/useController'

import { useJob } from '@/data/Job'
import toast from 'react-hot-toast'

export default function General() {
  const navigate = useNavigate()

  const { job, setJob }: any = useContext(JobContext)

  //

  const [status, setStatus] = useState<any>(null)
  const [statusOptions, setStatusOptions] = useState<any>(null)
  const [action, setAction] = useState<any>(null)
  const [actionOptions, setActionOptions] = useState<any>(null)

  const updateStatusDropdowns = (value: any) => {
    console.log(`updateStatusDropdowns`, value)

    job_status_id_ref.current = value

    const statuses = [
      {
        id: 1,
        parent_id: null,
        name: `Bidding`,
      },
      {
        id: 2,
        parent_id: null,
        name: `Won`,
      },
      {
        id: 3,
        parent_id: null,
        name: `Lost`,
      },
      {
        id: 4,
        parent_id: 1,
        name: `Quotes Requested`,
      },
      {
        id: 5,
        parent_id: 1,
        name: `Quotes Received`,
      },
      {
        id: 6,
        parent_id: 1,
        name: `Bid Submitted`,
      },
      {
        id: 7,
        parent_id: 1,
        name: `Rebid`,
      },
      {
        id: 8,
        parent_id: 2,
        name: `Value Engineering`,
      },
      {
        id: 9,
        parent_id: 2,
        name: `Walkthrough`,
      },
      {
        id: 10,
        parent_id: 2,
        name: `Site / Groundwork`,
      },
      {
        id: 11,
        parent_id: 2,
        name: `Rough-in`,
      },
      {
        id: 12,
        parent_id: 2,
        name: `Trim`,
      },
      {
        id: 13,
        parent_id: 2,
        name: `Punchlist`,
      },
      {
        id: 14,
        parent_id: 2,
        name: `Project Clouseout`,
      },
      {
        id: 15,
        parent_id: 2,
        name: `Waiting on Rentention`,
      },
      {
        id: 16,
        parent_id: 2,
        name: `Complete`,
      },
    ]

    //

    const statusOptions = statuses
      .filter((option: any) => option.parent_id === null)
      .map((option) => {
        return {
          key: option.id,
          value: option.name,
        }
      })

    setStatusOptions(statusOptions)

    let selectedStatusOptionID: any = null
    for (const option of statuses) {
      if (option.id == value) {
        if (option.parent_id === null) {
          selectedStatusOptionID = option.id
        } else {
          selectedStatusOptionID = option.parent_id
        }
        continue
      }
    }

    setStatus(selectedStatusOptionID)

    //

    const actionOptions = statuses
      .filter((option: any) => option.parent_id == selectedStatusOptionID)
      .map((option) => {
        return {
          key: option.id,
          value: option.name,
        }
      })

    setActionOptions(actionOptions)

    let selectedActionOptionID: any = null
    for (const option of statuses) {
      if (option.id == value) {
        if (option.parent_id) {
          selectedActionOptionID = option.parent_id
        } else {
          selectedActionOptionID = actionOptions?.length
            ? actionOptions[0].key
            : null
        }
        continue
      }
    }

    setAction(selectedActionOptionID)
  }

  const handleStatusChange = (event: any) => {
    const value = event.target.value
    updateStatusDropdowns(value)
    debouncedSave()
  }

  //

  const {
    number_ref,
    name_ref,
    description_ref,
    temporary_power_ref,
    temporary_lighting_ref,
    sqft_ref,
    labor_factor_ref,

    job_status_id_ref,

    bid_due_date_ref,
    job_starting_date_ref,
    job_completion_date_ref,
    winning_contractor_ref,
    winning_amount_ref,
  } = useJob()

  const { save } = useController({ endpoint: `/api/jobs` })

  const saveOnly = async () => {
    return new Promise((resolve, reject) => {
      const payload = {
        number: number_ref?.current?.value,
        name: name_ref?.current?.value,
        description: description_ref?.current?.value,
        temporary_power: temporary_power_ref?.current?.value,
        temporary_lighting: temporary_lighting_ref?.current?.value,
        sqft: sqft_ref?.current?.value,
        labor_factor: labor_factor_ref?.current?.value,

        job_status_id: job_status_id_ref?.current,

        bid_due_date: bid_due_date_ref?.current?.value,
        job_starting_date: job_starting_date_ref?.current?.value,
        job_completion_date: job_completion_date_ref?.current?.value,
        winning_contractor: winning_contractor_ref?.current?.value,
        winning_amount: winning_amount_ref?.current?.value,
      }

      // console.log(`saveOnly`, payload)

      save(job.id, payload)
        .then((data: any) => {
          setJob(data)
          if (!job.id) {
            navigate(`/app/jobs/all/${data.id}/edit`)
          }
          // updateRefs(data)
          // setErrors([])
          return resolve(data)
        })
        .catch((errors) => {
          // setErrors(errors)
          return reject(errors)
        })
    })
  }

  /*
  const debouncedSave = useMemo(
    () => debounce(() => saveOnly(), 1000),
    [job?.id],
  )

  const onSave = () => {
    toast.success(`Saved!`)
  }
  */

  const debouncedSave = () => {
    console.log(`debug.debouncedSave`, `disabled`)
  }

  const onSave = () => {
    saveOnly()
  }

  useEffect(() => {
    if (!job) return
    if (!job.status) return

    updateStatusDropdowns(job.status.id)
  }, [job?.id])

  //

  if (!job) return null

  const context_job_id = String(job.id)
  const { id: route_job_id } = useParams()

  console.log(`debug.bid`, context_job_id, route_job_id)

  if (context_job_id && route_job_id) {
    if (context_job_id !== route_job_id) {
      return null
    }
  }

  return (
    <div
      className="space-y-8"
      key={job.id}
    >
      <div className="grid grid-cols-2 gap-8">
        <div className="">
          <div className="space-y-4">
            <div>
              <h3 className="text-lg font-medium leading-6">Job Information</h3>
              <p className="mt-1 text-sm text-gray-500">Basic job details</p>
            </div>
            <hr />
            <div className="grid grid-cols-2 gap-4">
              <div className="">
                <Form.Control label={`Number`}>
                  <Form.Input
                    defaultValue={job.number}
                    onChange={debouncedSave}
                    ref={number_ref}
                  />
                </Form.Control>
              </div>
              <div className="">
                <Form.Control label={`Type`}>
                  <div
                    className={`flex h-[38px] items-center text-sm leading-none`}
                  >
                    {job?.type?.name ?? `Base Bid`}
                  </div>
                </Form.Control>
              </div>
            </div>
            <div className="grid grid-cols-1 gap-4">
              <div className="">
                <Form.Control
                  label={`Name`}
                  field={
                    <Form.Input
                      defaultValue={job.name}
                      onChange={debouncedSave}
                      ref={name_ref}
                    />
                  }
                  required
                />
              </div>
            </div>
            <div className="grid grid-cols-1 gap-4">
              <div className="">
                <Form.Control label={`Description`}>
                  <Form.Input
                    defaultValue={job.description}
                    onChange={debouncedSave}
                    ref={description_ref}
                  />
                </Form.Control>
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div className="">
                <FormToggle
                  defaultValue={job.temporary_power?.toString() === `1` ? 1 : 0}
                  label={`Temporary Power`}
                  onChange={debouncedSave}
                  ref={temporary_power_ref}
                />
              </div>
              <div className="">
                <FormToggle
                  defaultValue={
                    job.temporary_lighting?.toString() === `1` ? 1 : 0
                  }
                  label={`Temporary Lighting`}
                  onChange={debouncedSave}
                  ref={temporary_lighting_ref}
                />
              </div>
            </div>
            <div className="grid grid-cols-2 gap-4">
              <div className="">
                <Form.Control label={`Square Footage`}>
                  <Form.Input
                    defaultValue={job.sqft}
                    onChange={debouncedSave}
                    ref={sqft_ref}
                    type={`number`}
                  />
                </Form.Control>
              </div>
              <div className="">
                <Form.Control label={`Labor Factor`}>
                  <Form.Input
                    defaultValue={job.labor_factor}
                    onChange={debouncedSave}
                    ref={labor_factor_ref}
                    type={`number`}
                  />
                </Form.Control>
              </div>
            </div>
          </div>
        </div>
        <div className="">
          <div className="space-y-4">
            <div>
              <h3 className="text-lg font-medium leading-6">Progress</h3>
              <p className="mt-1 text-sm text-gray-500">
                Track the status of a job
              </p>
            </div>
            <hr />
            <div className="grid grid-cols-2 gap-4">
              <div className="">
                {/* {status} */}
                {status && (
                  <FormSelect
                    label={`Status`}
                    onChange={handleStatusChange}
                    options={statusOptions}
                    value={status}
                  />
                )}
              </div>
              <div className="">
                {/* {action} */}
                {action && (
                  <FormSelect
                    label={`Action`}
                    onChange={handleStatusChange}
                    options={actionOptions}
                    value={action}
                  />
                )}
              </div>
            </div>
            <div
              className={`grid grid-cols-2 gap-4 ${
                status == 1 ? `grid` : `hidden`
              }`}
            >
              <div className="">
                <Form.Control label={`Bid Due Date`}>
                  <Form.Input
                    defaultValue={job.bid_due_date}
                    onChange={debouncedSave}
                    ref={bid_due_date_ref}
                    type={`date`}
                  />
                </Form.Control>
              </div>
              <div className=""></div>
            </div>
            <div
              className={`grid grid-cols-2 gap-4 ${
                status == 2 ? `grid` : `hidden`
              }`}
            >
              <div className="">
                <Form.Control label={`Job Starting Date`}>
                  <Form.Input
                    defaultValue={job.job_starting_date}
                    onChange={debouncedSave}
                    ref={job_starting_date_ref}
                    type={`date`}
                  />
                </Form.Control>
              </div>
              <div className="">
                <Form.Control label={`Job Completion Date`}>
                  <Form.Input
                    defaultValue={job.job_completion_date}
                    onChange={debouncedSave}
                    ref={job_completion_date_ref}
                    type={`date`}
                  />
                </Form.Control>
              </div>
            </div>
            <div
              className={`grid grid-cols-2 gap-4 ${
                status == 3 ? `grid` : `hidden`
              }`}
            >
              <div className="">
                <Form.Control label={`Winning Contractor`}>
                  <Form.Input
                    defaultValue={job.winning_contractor}
                    onChange={debouncedSave}
                    ref={winning_contractor_ref}
                  />
                </Form.Control>
              </div>
              <div className="">
                <Form.Control label={`Winning Amount`}>
                  <Form.Input
                    defaultValue={job.winning_amount}
                    onChange={debouncedSave}
                    ref={winning_amount_ref}
                    type={`number`}
                  />
                </Form.Control>
              </div>
            </div>
          </div>
        </div>
      </div>
      <hr />
      <div className="flex items-center justify-center">
        <FormButton onClick={onSave}>Save</FormButton>
      </div>
    </div>
  )
}
