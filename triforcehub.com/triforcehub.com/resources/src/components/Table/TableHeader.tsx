type TableHeaderProps = {
  children: React.ReactNode,
}

const TableHeader = (props: TableHeaderProps) => {
  const {
    children,
  } = props

  return (
    <thead
      {...props}
      className={`
        bg-gray-100
      `}
    >
      {children}
    </thead>
  )
}

export default TableHeader
