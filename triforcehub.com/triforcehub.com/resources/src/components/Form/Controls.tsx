export const Controls = (props: any) => {
  const {
    children,
    cols = 2,
  } = props

  return (
    <div
      className={`
        gap-8
        grid
        ${cols === 1 && `grid-cols-1`}
        ${cols === 2 && `grid-cols-2`}
        ${cols === 3 && `grid-cols-3`}
        ${cols === 4 && `grid-cols-4`}
      `}
    >
      {children}
    </div>
  )
}
