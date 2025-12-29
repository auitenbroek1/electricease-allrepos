export const Footer = (props: any) => {
  const {
    children
  } = props

  return (
    <div
      className={`
        flex
        items-center
        justify-between
      `}
    >
      {children}
    </div>
  )
}
