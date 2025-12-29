const Buttons = (props: any) => {
  const {
    children,
  } = props

  return (
    <div
      className={`
        bg-gray-50
        flex
        justify-center
        px-4
        py-3
        sm:px-6
      `}
    >
      {children}
    </div>
  )
}

export default Buttons
