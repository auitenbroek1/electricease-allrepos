import { useContext } from 'react'

import Phases from './Phases'

import JobContext from './../contexts/Job'

const Takeoff = () => {
  const { job }: any = useContext(JobContext)

  if (!job) return null
  if (!job.phases) return null

  return (
    <>
      <Phases data={job.phases} />
    </>
  )
}

export default Takeoff
