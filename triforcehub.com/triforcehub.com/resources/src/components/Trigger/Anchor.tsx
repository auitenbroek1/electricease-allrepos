import { NavLink } from 'react-router-dom'

const Anchor = (props: any) => {
  const { active, disabled, icon, iconAfter, onClick, to, title } = props

  const Body = ({ isActive }: any) => {
    return (
      <span
        className={`
          ${!disabled ? `cursor-pointer` : ``}
          flex
          items-center
          space-x-2
          whitespace-nowrap
          text-sm
          ${!disabled && !isActive ? `underline` : ``}
          ${isActive ? `text-brand-gradient-light` : `opacity-50`}
          ${isActive ? `hover:underline` : `hover:opacity-100`}
          ${disabled ? `pointer-events-none` : ``}
        `}
        onClick={onClick}
      >
        {icon && <span>{icon}</span>}
        <span>{title}</span>
        {iconAfter && <span>{iconAfter}</span>}
      </span>
    )
  }

  const Wrapper = () => {
    if (to && !disabled) {
      return (
        <NavLink to={to}>
          {({ isActive }: any) => <Body isActive={isActive} />}
        </NavLink>
      )
    }

    return <Body isActive={active} />
  }

  return <Wrapper />
}

export default Anchor
