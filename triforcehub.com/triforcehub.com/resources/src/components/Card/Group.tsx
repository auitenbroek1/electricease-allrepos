export const Group = (props: any) => {
  const { children } = props

  return (
    <div
      className={`
        space-y-4
      `}
    >
      {children}
    </div>
  )
}
