const Title = (props: any) => {
  const {
    children,
    icon,
  } = props

  return (
    <div>
      <button
        className={`
          flex
          group
          items-center
          opacity-75
          px-3
          py-2
          space-x-2
          w-full
        `}
        disabled={true}
        type={`button`}
      >
        <span
          className={`
            h-5
            text-gray-500
            w-5
        `}
        >
          {icon}
        </span>
        <span
          className={`
            text-gray-700
            text-xs
            uppercase
          `}
        >
          {children}
        </span>
      </button>
    </div>
  )
}

export default Title
