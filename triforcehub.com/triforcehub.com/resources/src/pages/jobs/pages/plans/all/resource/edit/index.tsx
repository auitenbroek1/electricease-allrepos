import { useContext, useEffect, useRef, useState } from 'react'
import { useNavigate, useParams } from 'react-router-dom'

import axios from 'axios'

import JobContext from '@/pages/jobs/contexts/Job'

export const EditPage = (props: any) => {
  const { job }: any = useContext(JobContext)

  const params = useParams()
  const { id } = params

  const [plan, setPlan] = useState<any>()

  useEffect(() => {
    if (!job?.id) return

    console.log(`debug.ost.plan`, { job: job.id, plan: id })

    const fetchData = async () => {
      try {
        const response = await axios.get(`/api/jobs/${job.id}/plans/${id}`)
        const data = response.data.data
        console.log(`debug.ost.plan`, data)
        setPlan(data)
      } catch (exception) {
        console.log(`debug.ost.plan`, exception)
      }
    }

    fetchData()
  }, [job?.id, id])

  useEffect(() => {
    if (!job?.id) return
    if (!plan?.id) return

    console.log(`debug.plans.edit.mount`, `mount`, {
      job: job.id,
      plan: plan.id,
    })

    return () => {
      console.log(`debug.plans.edit.mount`, `umount`)
    }
  }, [job?.id, plan?.id])

  return (
    <div>
      <div>url: {plan?.upload?.url}</div>
    </div>
  )
}
