type TableProps = {
  children: React.ReactNode,
}

const Table = (props: TableProps) => {
  const {
    children
  } = props

  return (
    <div className={`overflow-x-scroll`}>
      <table
        {...props}
        className={`
          min-w-full
        `}
      >
        {children}
      </table>
    </div>
  )
}

export default Table
