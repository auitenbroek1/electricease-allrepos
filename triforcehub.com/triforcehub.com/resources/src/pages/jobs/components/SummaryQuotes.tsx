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

export const SummaryQuotes = (props: any) => {
  const { data } = props

  if (!data) return null

  return (
    <Table>
      <colgroup>
        <col width={1} />
        <col width={`*`} />
        <col width={1} />
      </colgroup>
      <TableHeader>
        <TableHeaderRow>
          <TableHeaderCell>Name</TableHeaderCell>
          <TableHeaderCell>Notes</TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>Cost</div>
          </TableHeaderCell>
        </TableHeaderRow>
      </TableHeader>
      <TableBody>
        {data.items?.map((item: any) => (
          <TableBodyRow key={item.uuid}>
            <TableBodyCell>
              <div className={`w-64`}>{item.name}</div>
            </TableBodyCell>
            <TableBodyCell>
              <div className={`max-w-xl truncate`}>{item.notes}</div>
            </TableBodyCell>
            <TableBodyCell>
              <div className={`w-32 text-right`}>
                <Form.Input
                  defaultValue={item.cost}
                  disabled={true}
                  prefix={`$`}
                />
              </div>
            </TableBodyCell>
          </TableBodyRow>
        ))}
      </TableBody>
      <TableFooter>
        <TableFooterRow>
          <TableFooterCell>Total</TableFooterCell>
          <TableFooterCell></TableFooterCell>
          <TableFooterCell>
            <div className={`px-2 text-right`}>
              {currency(data.summary.cost)}
            </div>
          </TableFooterCell>
        </TableFooterRow>
      </TableFooter>
    </Table>
  )
}
