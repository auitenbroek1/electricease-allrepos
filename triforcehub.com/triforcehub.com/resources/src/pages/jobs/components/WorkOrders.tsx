import { useContext } from 'react'
import JobsContext from '@/pages/jobs/contexts/Job'
import WorkOrder from './WorkOrder'

const WorkOrders = () => {
  const { job }: any = useContext(JobsContext)

  if (!job) return null

  const customers = job.parent?.customers ?? job.customers

  return (
    <>
      {customers.map((customer: any) => (
        <WorkOrder
          data={customer}
          key={customer.uuid}
        />
      ))}
    </>
  )
}

export default WorkOrders
