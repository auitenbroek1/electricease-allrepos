import {
  Table,
  TableBody,
  TableFooter,
  TableFooterCell,
  TableFooterRow,
  TableHeader,
  TableHeaderCell,
  TableHeaderRow,
} from '@/components'

import { currency } from '@/utilities/format'

import { SummaryAdjustmentsItem } from './SummaryAdjustmentsItem'

export const SummaryAdjustments = (props: any) => {
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
      key: `cost`,
      title: `Actual Cost`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `override`,
      title: `Override`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `overhead`,
      title: `Overhead %`,
      renderFooterCell: (value: any) => {
        return `${value}%`
      },
    },
    {
      key: `overhead_total`,
      title: `Overhead Total`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `profit`,
      title: `Profit %`,
      renderFooterCell: (value: any) => {
        return `${value}%`
      },
    },
    {
      key: `profit_total`,
      title: `Profit Total`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `tax`,
      title: `Tax %`,
      renderFooterCell: (value: any) => {
        return `${value}%`
      },
    },
    {
      key: `tax_total`,
      title: `Tax Total`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
    {
      key: `cost_total`,
      title: `Grand Total`,
      renderFooterCell: (value: any) => {
        return currency(value)
      },
    },
  ]

  if (!data) return null

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
        {data?.items?.map((row: any, index: number) => (
          <SummaryAdjustmentsItem
            data={row}
            key={index}
          />
        ))}
      </TableBody>
      <TableFooter>
        <TableFooterRow>
          {columns.map((column, index) => (
            <TableFooterCell key={index}>
              <div
                className={`
                    px-2
                    ${index === 0 ? `` : `text-right`}
                    ${index === 0 ? `w-64` : `w-32`}
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
