export const Page = (props: any) => {
  const { children } = props

  return (
    <main className={`relative z-0`}>
      <div className="p-8">{children}</div>
    </main>
  )
}
