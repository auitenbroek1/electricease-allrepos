const Group = (props: any) => {
  const {
    children
  } = props

  return (
    <div
      className={`
        p-1
      `}
    >
      {children}
    </div>
  )
}

export default Group
