import { useContext } from 'react'

import JobContext from '../contexts/Job'

import { Customers } from './Customers'
import { Locations } from './Locations'

export default function Client() {
  const { job }: any = useContext(JobContext)

  // console.log(job?.customers)
  // console.log(job?.locations)

  return (
    <div
      className="space-y-8"
      key={job?.id}
    >
      <div className="grid grid-cols-2 gap-8">
        <div className="">
          <Customers data={job?.customers} />
        </div>
        <div className="">
          <Locations data={job?.locations} />
        </div>
      </div>
    </div>
  )
}
