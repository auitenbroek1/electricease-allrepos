const Content = (props: any) => {
  const {
    children,
  } = props

  return (
    <div
      className={`
        bg-white
        px-4
        pt-5
        pb-4
        sm:p-6
        sm:pb-4
      `}
    >
      <div
        className={`
          sm:flex
          sm:items-start
        `}
      >
        {children}
      </div>
    </div>
  )
}

export default Content
