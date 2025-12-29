import axios from 'axios'
import debounce from 'lodash/debounce'
import { useContext, useMemo, useRef } from 'react'

import JobContext from '../contexts/Job'
import { useJob } from '@/data/Job'

export const SummarySettings = () => {
  const { job }: any = useContext(JobContext)
  const { reloadJob } = useJob()

  const material_ref = useRef<any>()
  const tax_ref = useRef<any>()

  const save = async () => {
    console.log(`debug.save`)

    const payload = {
      exclude_material_subtotal_from_total: material_ref?.current?.checked,
      hide_tax_in_proposal: tax_ref?.current?.checked,
    }

    console.log(`debug.save`, `update`, payload)
    // return

    try {
      const response = await axios.patch(
        `/api/jobs/${job.id}/settings`,
        payload,
      )
      console.log(`debug.save`, response.data.data)
      await reloadJob()
    } catch (exception: any) {
      console.log(`debug.save`, exception.response.data.errors)
      // setErrors(exception.response.data.errors)
    }
  }

  const sync = useMemo(() => debounce(() => save(), 1000), [job?.id])

  return (
    <div>
      {/* {JSON.stringify(job?.settings)} */}
      <div>
        <label className={`inline-flex cursor-pointer items-center space-x-2`}>
          <input
            defaultChecked={
              job?.settings?.exclude_material_subtotal_from_total ? true : false
            }
            ref={material_ref}
            type={`checkbox`}
            onChange={sync}
          />
          <span>Exclude Material Subtotal From Total</span>
        </label>
      </div>
      <div>
        <label className={`inline-flex cursor-pointer items-center space-x-2`}>
          <input
            defaultChecked={job?.settings?.hide_tax_in_proposal ? true : false}
            ref={tax_ref}
            type={`checkbox`}
            onChange={sync}
          />
          <span>Hide Tax in Proposal</span>
        </label>
      </div>
    </div>
  )
}
