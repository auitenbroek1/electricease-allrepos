type TableFooterCellPorps = {
  children?: React.ReactNode
}

const TableFooterCell = (props: TableFooterCellPorps) => {
  const { children } = props

  return (
    <td
      {...props}
      className={`
        font-nromal
        whitespace-nowrap
        px-2
        py-4
        text-left
        text-sm
        tracking-wide
        first-of-type:pl-4
        last-of-type:pr-4
      `}
    >
      {children}
    </td>
  )
}

export default TableFooterCell
