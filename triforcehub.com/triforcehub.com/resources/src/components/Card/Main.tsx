import { useCard } from './Context'

export const Main = (props: any) => {
  const { children } = props

  const { open } = useCard()

  return (
    <div
      className={`
        relative
        z-0
        ${open ? `block` : `hidden`}
        space-y-8
        bg-white
        p-8
      `}
    >
      {children}
    </div>
  )
}
