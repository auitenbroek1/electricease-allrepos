import { useContext } from 'react'

import {
  Form,
  Table,
  TableBody,
  TableBodyCell,
  TableBodyRow,
  TableFooter,
  TableFooterCell,
  TableFooterRow,
  TableHeader,
  TableHeaderCell,
  TableHeaderRow,
} from '@/components'

import { currency } from '@/utilities/format'

import { SummaryCrewsItem } from './SummaryCrewsItem'

import axios from 'axios'
import JobContext from '../contexts/Job'
import { useJob } from '@/data/Job'

export const SummaryCrews = (props: any) => {
  const { data } = props

  const columns = [
    {
      key: `name`,
      title: `Name`,
      renderFooterCell: (value: any) => {
        return `Totals`
      },
    },
    {
      key: `quantity`,
      title: `Quantity`,
      renderFooterCell: (value: any) => {
        return `${value}`
      },
    },
    {
      key: `hours`,
      title: `Hours`,
      renderFooterCell: (value: any) => {
        return `${value}`
      },
    },
    {
      key: `rate`,
      title: `Rate`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `cost`,
      title: `Subtotal`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `burden`,
      title: `Burden %`,
      renderFooterCell: (value: any) => {
        return `${value}%`
      },
    },
    {
      key: `burden_total`,
      title: `Burden Total`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `fringe`,
      title: `Fringe $`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `fringe_total`,
      title: `Fringe Total`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `rate_total`,
      title: `Full Rate`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `cost_total`,
      title: `Total`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `id`,
      title: null,
      renderFooterCell: () => {
        return null
      },
    },
  ]

  //

  const { job }: any = useContext(JobContext)
  const { reloadJob } = useJob()

  const handleNew = async () => {
    const payload = {
      job_id: job?.id,
      name: ``,
      quantity: 1,
      rate: 0,
      burden: 0,
      fringe: 0,
    }

    console.log(`debug.save`, payload)

    try {
      const response = await axios.post(`/api/jobs/crews`, payload)
      console.log(`debug.save`, response.data?.data)
      await reloadJob()
    } catch (exception: any) {
      console.log(`debug.save`, exception.response.data?.errors)
      // setErrors(exception.response.data?.errors)
    }
  }

  // if (!data) return null

  return (
    <Table>
      <colgroup>
        {columns.map((column, index) => (
          <col
            key={index}
            width={index === 0 ? `*` : 1}
          />
        ))}
      </colgroup>
      <TableHeader>
        <TableHeaderRow>
          {columns.map((column, index) => (
            <TableHeaderCell key={index}>
              <div
                className={`
                    ${index === 0 ? `` : `text-center`}
                  `}
              >
                {column.title}
              </div>
            </TableHeaderCell>
          ))}
        </TableHeaderRow>
      </TableHeader>
      <TableBody>
        {data?.items?.map((row: any) => (
          <SummaryCrewsItem
            data={row}
            key={row.uuid}
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
      <TableFooter>
        <TableFooterRow>
          {columns.map((column, index) => (
            <TableFooterCell key={index}>
              <div
                className={`
                    px-2
                    ${index === 0 ? `` : `text-right`}
                    ${index === 0 ? `w-64` : column.key === `id` ? `` : `w-32`}
                  `}
              >
                {column.renderFooterCell(data?.summary[column.key] ?? 0)}
              </div>
            </TableFooterCell>
          ))}
        </TableFooterRow>
      </TableFooter>
    </Table>
  )
}
