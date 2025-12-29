type Props = {
  children: React.ReactNode,
}

const TableHeaderRow = (props: Props) => {
  const {
    children
  } = props

  return (
    <tr
      {...props}
    >
      {children}
    </tr>
  )
}

export default TableHeaderRow
