type TableBodyCellPorps = {
  children?: React.ReactNode
  colSpan?: number
}

const TableBodyCell = (props: TableBodyCellPorps) => {
  const { children, colSpan = 1 } = props

  return (
    <td
      {...props}
      className={`
        whitespace-nowrap
        px-2
        py-4
        text-left
        text-sm
        text-gray-500
        first-of-type:pl-4
        last-of-type:pr-4
      `}
      colSpan={colSpan}
    >
      {children}
    </td>
  )
}

export default TableBodyCell
