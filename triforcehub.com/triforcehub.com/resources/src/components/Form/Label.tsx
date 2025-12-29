export const Label = (props: any) => {
  const { children } = props

  return (
    <label
      className={`
        block
        text-xs
        font-normal
        uppercase
        leading-none
        tracking-wide
    `}
    >
      {children}
    </label>
  )
}
