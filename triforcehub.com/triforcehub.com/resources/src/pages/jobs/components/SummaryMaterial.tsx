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

export const SummaryMaterial = (props: any) => {
  const { data } = props

  if (!data) return null

  return (
    <Table>
      <colgroup>
        <col width={`*`} />
        <col width={1} />
        <col width={1} />
        <col width={1} />
        <col width={1} />
      </colgroup>
      <TableHeader>
        <TableHeaderRow>
          <TableHeaderCell>Phase</TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>Material</div>
          </TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>Material %</div>
          </TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>Labor Hours</div>
          </TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>Labor %</div>
          </TableHeaderCell>
        </TableHeaderRow>
      </TableHeader>
      <TableBody>
        {data?.items?.map((item: any) => (
          <TableBodyRow key={item.uuid}>
            <TableBodyCell>{item.name}</TableBodyCell>
            <TableBodyCell>
              <div className={`w-32 text-right`}>
                <Form.Input
                  disabled={true}
                  prefix={`$`}
                  value={item.cost}
                />
              </div>
            </TableBodyCell>
            <TableBodyCell>
              <div className={`w-32 text-right`}>
                <Form.Input
                  disabled={true}
                  prefix={`%`}
                  value={item.cost_percent}
                />
              </div>
            </TableBodyCell>
            <TableBodyCell>
              <div className={`w-32 text-right`}>
                <Form.Input
                  disabled={true}
                  value={item.labor}
                />
              </div>
            </TableBodyCell>
            <TableBodyCell>
              <div className={`w-32 text-right`}>
                <Form.Input
                  disabled={true}
                  prefix={`%`}
                  value={item.labor_percent}
                />
              </div>
            </TableBodyCell>
          </TableBodyRow>
        ))}
      </TableBody>
      <TableFooter>
        <TableFooterRow>
          <TableFooterCell>Total</TableFooterCell>
          <TableFooterCell>
            <div className={`px-2 text-right`}>
              {currency(data?.summary?.cost)}
            </div>
          </TableFooterCell>
          <TableFooterCell>
            <div className={`px-2 text-right`}>
              {data?.summary?.cost_percent}%
            </div>
          </TableFooterCell>
          <TableFooterCell>
            <div className={`px-2 text-right`}>{data?.summary?.labor}</div>
          </TableFooterCell>
          <TableFooterCell>
            <div className={`px-2 text-right`}>
              {data?.summary?.labor_percent}%
            </div>
          </TableFooterCell>
        </TableFooterRow>
      </TableFooter>
    </Table>
  )
}
