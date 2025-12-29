export const Header = (props: any) => {
  const { children, title } = props

  return (
    <div
      className={`
        space-y-2
      `}
    >
      <div
        className={`
          text-lg
          font-medium
          leading-none
        `}
      >
        {title}
      </div>
      <div
        className={`
          text-sm
          text-gray-500
        `}
      >
        {children}
      </div>
    </div>
  )
}
