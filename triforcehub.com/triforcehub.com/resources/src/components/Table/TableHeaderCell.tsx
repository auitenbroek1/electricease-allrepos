type TableHeaderCellPorps = {
  children?: React.ReactNode,
}

const TableHeaderCell = (props: TableHeaderCellPorps) => {
  const {
    children
  } = props

  return (
    <th
      {...props}
      className={`
        first-of-type:pl-4
        font-normal
        last-of-type:pr-4
        px-2
        py-4
        text-gray-500
        text-left
        text-xs
        tracking-wide
        uppercase
        whitespace-nowrap
      `}
    >
      {children}
    </th>
  )
}

export default TableHeaderCell
