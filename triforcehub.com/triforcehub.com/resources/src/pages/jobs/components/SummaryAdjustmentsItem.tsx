import axios from 'axios'
import debounce from 'lodash/debounce'
import toast from 'react-hot-toast'
import { useContext, useMemo, useRef, useState } from 'react'

import JobContext from '../contexts/Job'
import { useJob } from '@/data/Job'

import { Form, TableBodyCell, TableBodyRow } from '@/components'

export const SummaryAdjustmentsItem = (props: any) => {
  const { data } = props

  const { job }: any = useContext(JobContext)
  const { reloadJob } = useJob()

  const override_ref = useRef<any>()
  const overhead_ref = useRef<any>()
  const profit_ref = useRef<any>()
  const tax_ref = useRef<any>()

  const [errors, setErrors] = useState<any>([])

  const save = async () => {
    console.log(`debug.save`)
    // if (!validate()) {
    //   return
    // }

    // setLoading(true)

    // console.log('errors', errors)
    // if (Object.keys(errors)) return

    const payload = {
      job_id: job?.id,
      slug: data?.slug,
      override: override_ref?.current?.value,
      overhead: overhead_ref?.current?.value,
      profit: profit_ref?.current?.value,
      tax: tax_ref?.current?.value,
      enabled: true,
    }

    console.log(`debug.save`, payload)

    if (data.id) {
      console.log(`debug.update`)
      try {
        const response = await axios.patch(
          `/api/jobs/adjustments/${data.id}`,
          payload,
        )
        console.log(`debug.save`, response.data.data)
        setErrors([])
        toast.success(`Saved!`)
        await reloadJob()
      } catch (exception: any) {
        console.log(`debug.save`, exception.response.data.errors)
        setErrors(exception.response?.data?.errors ?? [])
      }
    } else {
      console.log(`debug.store`)
      try {
        const response = await axios.post(`/api/jobs/adjustments`, payload)
        console.log(`debug.save`, response.data.data)
        setErrors([])
        toast.success(`Saved!`)
        await reloadJob()
      } catch (exception: any) {
        console.log(`debug.save`, exception.response.data.errors)
        setErrors(exception.response?.data?.errors ?? [])
      }
    }

    // setLoading(false)
  }

  const sync = useMemo(() => debounce(() => save(), 1000), [data.id])

  return (
    <TableBodyRow>
      <TableBodyCell>{data.name}</TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            disabled={true}
            prefix={`$`}
            value={data.cost}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            defaultValue={data.override}
            invalid={errors?.override?.length}
            messages={errors?.override}
            name={`override`}
            onChange={sync}
            placeholder={data.cost}
            prefix={`$`}
            ref={override_ref}
            step={0.01}
            type={`number`}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            defaultValue={data.overhead}
            invalid={errors?.overhead?.length}
            messages={errors?.overhead}
            name={`overhead`}
            onChange={sync}
            placeholder={`0.00`}
            prefix={`%`}
            ref={overhead_ref}
            step={0.01}
            type={`number`}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            disabled={true}
            prefix={`$`}
            value={data.overhead_total}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            defaultValue={data.profit}
            invalid={errors?.profit?.length}
            messages={errors?.profit}
            name={`profit`}
            onChange={sync}
            placeholder={`0.00`}
            prefix={`%`}
            ref={profit_ref}
            step={0.01}
            type={`number`}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            disabled={true}
            prefix={`$`}
            value={data.profit_total}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            defaultValue={data.tax}
            invalid={errors?.tax?.length}
            messages={errors?.tax}
            name={`tax`}
            onChange={sync}
            placeholder={`0.0000`}
            prefix={`%`}
            ref={tax_ref}
            step={0.0001}
            type={`number`}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            disabled={true}
            prefix={`$`}
            value={data.tax_total}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            disabled={true}
            prefix={`$`}
            value={data.cost_total}
          />
        </div>
      </TableBodyCell>
    </TableBodyRow>
  )
}
