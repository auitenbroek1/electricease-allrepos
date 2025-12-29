import axios from 'axios'
import { useContext, useRef } from 'react'

import JobsContext from '@/pages/jobs/contexts/Job'

export const useJob = () => {
  const { job, setJob } = useContext<any>(JobsContext)

  const number_ref = useRef<HTMLInputElement>(null)
  const name_ref = useRef<HTMLInputElement>(null)
  const description_ref = useRef<HTMLInputElement>(null)
  const temporary_power_ref = useRef<HTMLInputElement>(null)
  const temporary_lighting_ref = useRef<HTMLInputElement>(null)
  const sqft_ref = useRef<HTMLInputElement>(null)
  const labor_factor_ref = useRef<HTMLInputElement>(null)

  const job_status_id_ref = useRef(null)

  const bid_due_date_ref = useRef<HTMLInputElement>(null)
  const job_starting_date_ref = useRef<HTMLInputElement>(null)
  const job_completion_date_ref = useRef<HTMLInputElement>(null)
  const winning_contractor_ref = useRef<HTMLInputElement>(null)
  const winning_amount_ref = useRef<HTMLInputElement>(null)

  const reloadJob = async () => {
    const response = await axios.get(`/api/jobs/${job.id}`)
    const data = response.data.data

    console.log(`reloadJob`, data)

    setJob(data)
  }

  return {
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

    reloadJob,
  }
}
