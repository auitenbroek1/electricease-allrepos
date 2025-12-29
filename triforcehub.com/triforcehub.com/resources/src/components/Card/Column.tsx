export const Column = (props: any) => {
  const { children, span = 1 } = props

  return (
    <div
      className={`
        space-y-8
        ${span === 1 ? `col-span-1` : ``}
        ${span === 2 ? `col-span-2` : ``}
        ${span === 3 ? `col-span-3` : ``}
        ${span === 4 ? `col-span-4` : ``}
      `}
    >
      {children}
    </div>
  )
}
