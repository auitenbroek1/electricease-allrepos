export const Column = (props: any) => {
  const { children, span } = props

  const classes = []

  if (span) {
    if (span === 1) classes.push(`col-span-1`)
    if (span === 2) classes.push(`col-span-2`)
    if (span === 3) classes.push(`col-span-3`)
    if (span === 4) classes.push(`col-span-4`)
  }

  return <div className={`${classes.join(` `)}`}>{children}</div>
}
