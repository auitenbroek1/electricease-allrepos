export const Description = (props: any) => {
  const { children } = props

  return (
    <label
      className={`
        block
        text-xs
        font-normal
        leading-none
        text-gray-500
    `}
    >
      {children}
    </label>
  )
}
