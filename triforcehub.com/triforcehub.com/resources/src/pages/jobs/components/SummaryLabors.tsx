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

import { SummaryLaborsItem } from './SummaryLaborsItem'

export const SummaryLabors = (props: any) => {
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
  ]

  if (!data) return null

  return (
    <Table>
      <colgroup>
        {columns.map((column, index) => (
          <col key={index} width={index === 0 ? `*` : 1} />
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
          <SummaryLaborsItem data={row} key={row.uuid} />
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
