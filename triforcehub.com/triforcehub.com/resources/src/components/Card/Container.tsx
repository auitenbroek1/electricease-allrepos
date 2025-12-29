import { useCard } from './Context'

export const Container = (props: any) => {
  const { children } = props

  const { open } = useCard()

  return (
    <div
      className={`
        ${open ? `overflow-hidden` : ``}
        ${open ? `rounded` : ``}
        ${open ? `shadow` : ``}
      `}
    >
      {children}
    </div>
  )
}
