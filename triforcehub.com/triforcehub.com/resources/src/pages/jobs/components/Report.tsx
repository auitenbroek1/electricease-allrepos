import {
  PageSectionIntro,
  PageSectionItem,
  PageSectionItems,
  Table,
  TableBody,
  TableBodyCell,
  TableBodyRow,
  TableFooter,
  TableFooterCell,
  TableFooterRow,
} from '@/components'

import { currency } from '@/utilities/format'

import ReportActions from './ReportActions'

export const Report = (props: any) => {
  const { data } = props

  return (
    <PageSectionItem
      heading={data.name}
      openByDefault={true}
      // actions={<ReportActions />}
    >
      <Table>
        <colgroup>
          <col width={`*`} />
          <col width={1} />
        </colgroup>
        <TableBody>
          {data.items.map((item: any, index: any) => (
            <TableBodyRow key={index}>
              <TableBodyCell>
                <div className={`${item.bold ? `font-semibold` : ``}`}>
                  {item.label}
                </div>
              </TableBodyCell>
              <TableBodyCell>
                <div
                  className={`w-32 text-right ${
                    item.bold ? `font-semibold` : ``
                  }`}
                >
                  {currency(item.value)}
                </div>
              </TableBodyCell>
            </TableBodyRow>
          ))}
        </TableBody>
      </Table>
    </PageSectionItem>
  )
}
