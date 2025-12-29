export const Root = (props: any) => {
  const { children, cols } = props

  const classes = []

  if (cols) {
    if (cols === 1) classes.push(`grid-cols-1`)
    if (cols === 2) classes.push(`grid-cols-2`)
    if (cols === 3) classes.push(`grid-cols-3`)
    if (cols === 4) classes.push(`grid-cols-4`)
  }

  return <div className={`grid gap-8 ${classes.join(` `)}`}>{children}</div>
}
