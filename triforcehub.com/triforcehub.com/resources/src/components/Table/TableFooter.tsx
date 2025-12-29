type TableFooterProps = {
  children: React.ReactNode,
}

const TableFooter = (props: TableFooterProps) => {
  const {
    children,
  } = props

  return (
    <tfoot
      {...props}
      className={`
        bg-yellow-100
      `}
    >
      {children}
    </tfoot>
  )
}

export default TableFooter
