type Props = {
  children: React.ReactNode
  selected?: boolean
  onClick?: any
  onDoubleClick?: any
}

const TableBodyRow = (props: Props) => {
  const { children, selected = false } = props

  return (
    <tr
      {...props}
      className={`
        hover:bg-yellow-50
        ${selected ? `bg-blue-50 hover:bg-blue-100` : ``}`}
    >
      {children}
    </tr>
  )
}

export default TableBodyRow
