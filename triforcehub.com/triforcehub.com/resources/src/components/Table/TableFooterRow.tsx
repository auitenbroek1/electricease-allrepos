type Props = {
  children: React.ReactNode,
}

const TableFooterRow = (props: Props) => {
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

export default TableFooterRow
