import { useContext } from 'react'

import JobContext from '@/pages/jobs/contexts/Job'

import { Phase } from './Phase'

export const Takeoff = () => {
  const { job }: any = useContext(JobContext)

  if (!job) return null
  if (!job.phases) return null

  return (
    <div className="space-y-4">
      {job.phases.map((phase: any, index: number) => (
        <Phase
          key={index}
          phase={phase}
        />
      ))}
    </div>
  )
}
