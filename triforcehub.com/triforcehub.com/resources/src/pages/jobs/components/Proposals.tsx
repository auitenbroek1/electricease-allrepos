import { useContext } from 'react'
import JobsContext from '@/pages/jobs/contexts/Job'
import Proposal from './Proposal'

const Proposals = () => {
  const { job }: any = useContext(JobsContext)

  if (!job) return null

  const customers = job.parent?.customers ?? job.customers

  return (
    <>
      {customers.map((customer: any) => (
        <Proposal
          data={customer}
          key={customer.uuid}
        />
      ))}
    </>
  )
}

export default Proposals
