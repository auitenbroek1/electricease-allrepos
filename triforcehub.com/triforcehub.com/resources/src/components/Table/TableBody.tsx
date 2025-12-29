type TableBodyProps = {
  children: React.ReactNode,
  disabled?: boolean,
}

const TableBody = (props: TableBodyProps) => {
  const {
    children,
    disabled = false
  } = props

  return (
    <tbody
      {...props}
      className={`
        border-gray-200
        border-t
        divide-gray-200
        divide-y
        ${disabled ? 'opacity-30' : ''}
      `}
    >
      {children}
    </tbody>
  )
}

export default TableBody
