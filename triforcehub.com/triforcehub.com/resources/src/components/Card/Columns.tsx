export const Columns = (props: any) => {
  const { children, number = 1 } = props

  return (
    <div
      className={`
        grid
        gap-8
        ${number === 1 ? `grid-cols-1` : ``}
        ${number === 2 ? `grid-cols-2` : ``}
      `}
    >
      {children}
    </div>
  )
}
