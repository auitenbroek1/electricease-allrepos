import { useContext } from 'react'

import Table from '@/components/Table/Table'
import TableHeader from '@/components/Table/TableHeader'
import TableHeaderCell from '@/components/Table/TableHeaderCell'
import TableBody from '@/components/Table/TableBody'

import { Form, TableBodyCell, TableBodyRow } from '@/components'

import { LaborItem } from './LaborItem'

import axios from 'axios'
import JobContext from '../contexts/Job'
import { useJob } from '@/data/Job'

export const LaborList = () => {
  const { job } = useContext<any>(JobContext)
  const { reloadJob } = useJob()

  const handleNew = async () => {
    const payload = {
      job_id: job.id,
      name: ``,
      hours: 0,
      rate: 0,
      enabled: 1,
    }

    console.log(`debug.save`, payload)

    try {
      const response = await axios.post(`/api/jobs/labor`, payload)
      console.log(`debug.save`, response.data?.data)
      await reloadJob()
    } catch (exception: any) {
      console.log(`debug.save`, exception.response.data?.errors)
      // setErrors(exception.response.data?.errors)
    }
  }

  if (!job) return null
  if (!job.labors) return null

  return (
    <Table>
      <colgroup>
        <col width={1} />
        <col width={1} />
        <col width={1} />
        <col width={1} />
        <col width={`*`} />
        <col width={1} />
      </colgroup>
      <TableHeader>
        <tr>
          <TableHeaderCell></TableHeaderCell>
          <TableHeaderCell>Name</TableHeaderCell>
          <TableHeaderCell>Hours</TableHeaderCell>
          <TableHeaderCell>Rate</TableHeaderCell>
          <TableHeaderCell>Notes</TableHeaderCell>
          <TableHeaderCell></TableHeaderCell>
        </tr>
      </TableHeader>
      <TableBody>
        {job?.labors?.map((item: any) => (
          <LaborItem
            data={item}
            key={item.uuid}
          />
        ))}
        <TableBodyRow>
          <TableBodyCell colSpan={99}>
            <Form.Buttons>
              <Form.Button onClick={handleNew}>Add New</Form.Button>
            </Form.Buttons>
          </TableBodyCell>
        </TableBodyRow>
      </TableBody>
    </Table>
  )
}
