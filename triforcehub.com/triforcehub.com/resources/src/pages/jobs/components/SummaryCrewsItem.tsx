import axios from 'axios'
import debounce from 'lodash/debounce'
import toast from 'react-hot-toast'
import { useContext, useMemo, useRef, useState } from 'react'

import JobContext from '../contexts/Job'
import { useJob } from '@/data/Job'

import { Form, TableBodyCell, TableBodyRow } from '@/components'
import { TrashIcon } from '@/components/Icons'

export const SummaryCrewsItem = (props: any) => {
  const { data } = props

  const { job }: any = useContext(JobContext)
  const { reloadJob } = useJob()

  const name_ref = useRef<any>()
  const quantity_ref = useRef<any>()
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
      quantity: quantity_ref?.current?.value,
      rate: rate_ref?.current?.value,
      burden: burden_ref?.current?.value,
      fringe: fringe_ref?.current?.value,
    }

    console.log(`debug.save`, payload)

    if (data?.id) {
      console.log(`debug.update`)
      try {
        const response = await axios.patch(
          `/api/jobs/crews/${data?.id}`,
          payload,
        )
        console.log(`debug.save`, response.data?.data)
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
        const response = await axios.post(`/api/jobs/crews`, payload)
        console.log(`debug.save`, response.data?.data)
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

  const handleDelete = async () => {
    const response = await axios.delete(`/api/jobs/crews/${data?.id}`)
    console.log(`delete`, response.data.data)
    toast.success(`Saved!`)
    reloadJob()
  }

  const sync = useMemo(() => debounce(() => save(), 1000), [data?.id])

  return (
    <TableBodyRow>
      <TableBodyCell>
        <div className={`w-64`}>
          <Form.Text
            defaultValue={data?.name}
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
            defaultValue={data?.quantity}
            invalid={errors?.quantity?.length}
            messages={errors?.quantity}
            name={`quantity`}
            onChange={sync}
            ref={quantity_ref}
            step={0.01}
            type={`number`}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            disabled={true}
            type={`number`}
            value={data?.hours}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            defaultValue={data?.rate}
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
            value={data?.cost}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            defaultValue={data?.burden}
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
            value={data?.burden_total}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            defaultValue={data?.fringe}
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
            value={data?.fringe_total}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            disabled={true}
            prefix={`$`}
            value={data?.rate_total}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        <div className={`w-32 text-right`}>
          <Form.Text
            disabled={true}
            prefix={`$`}
            value={data?.cost_total}
          />
        </div>
      </TableBodyCell>
      <TableBodyCell>
        {data?.id && (
          <button
            onClick={handleDelete}
            type={`button`}
          >
            <div className={`h-5 w-5`}>
              <TrashIcon />
            </div>
          </button>
        )}
      </TableBodyCell>
    </TableBodyRow>
  )
}
