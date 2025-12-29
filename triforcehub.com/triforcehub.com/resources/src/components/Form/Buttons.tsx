export const Buttons = (props: any) => {
  const {
    children,
  } = props

  return (
    <div
      className={`
          flex
          items-center
          justify-center
          space-x-8
        `}
    >
      {children}
    </div>
  )
}
