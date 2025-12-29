import axios from 'axios'
import debounce from 'lodash/debounce'
import toast from 'react-hot-toast'
import { useContext, useMemo, useRef, useState } from 'react'

import JobContext from '../contexts/Job'
import { useJob } from '@/data/Job'

import { Form, TableBodyCell, TableBodyRow } from '@/components'

export const SummaryLaborsItem = (props: any) => {
  const { data } = props

  const { job }: any = useContext(JobContext)
  const { reloadJob } = useJob()

  const name_ref = useRef<any>()
  const hours_ref = useRef<any>()
  const rate_ref = useRef<any>()
  const burden_ref = useRef<any>()
  const fringe_ref = useRef<any>()

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
      name: name_ref?.current?.value,
      hours: hours_ref?.current?.value,
      rate: rate_ref?.current?.value,
      burden: burden_ref?.current?.value,
      fringe: fringe_ref?.current?.value,
    }

    console.log(`debug.save`, payload)

    if (data.id) {
      console.log(`debug.update`)
      try {
        const response = await axios.patch(
          `/api/jobs/labor/${data.id}`,
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
      // console.log(`debug.store`)
      // try {
      //   const response = await axios.post(`/api/jobs/labor`, payload)
      //   console.log(`debug.save`, response.data?.data)
      //   setErrors([])
      //   toast.success(`Saved!`)
      //   await reloadJob()
      // } catch (exception: any) {
      //   console.log(`debug.save`, exception.response.data.errors)
      //   setErrors(exception.response?.data?.errors ?? [])
      // }
    }

    // setLoading(false)
  }

  const sync = useMemo(() => debounce(() => save(), 1000), [data.id])

  return (
    <TableBodyRow>
      <TableBodyCell>
        <div className={`w-64`}>
          <Form.Text
            defaultValue={data.name}
            invalid={errors?.name?.length}
            messages={errors?.name}
            name={`name`}
            onChange={sync}
            ref={name_ref}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            defaultValue={data.hours}
            invalid={errors?.hours?.length}
            messages={errors?.hours}
            name={`hours`}
            onChange={sync}
            ref={hours_ref}
            step={0.01}
            type={`number`}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            defaultValue={data.rate}
            invalid={errors?.rate?.length}
            messages={errors?.rate}
            name={`rate`}
            onChange={sync}
            prefix={`$`}
            ref={rate_ref}
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
            value={data.cost}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            defaultValue={data.burden}
            invalid={errors?.burden?.length}
            messages={errors?.burden}
            name={`burden`}
            onChange={sync}
            prefix={`%`}
            ref={burden_ref}
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
            value={data.burden_total}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            defaultValue={data.fringe}
            invalid={errors?.fringe?.length}
            messages={errors?.fringe}
            name={`fringe`}
            onChange={sync}
            prefix={`$`}
            ref={fringe_ref}
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
            value={data.fringe_total}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            disabled={true}
            prefix={`$`}
            value={data.rate_total}
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
