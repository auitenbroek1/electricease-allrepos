export const Error = (props: any) => {
  const { children } = props

  return (
    <div
      className={`
        block
        text-xs
        font-normal
        leading-none
        text-red-500
    `}
    >
      {children}
    </div>
  )
}
