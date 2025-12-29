const PageSectionIntro = (props: any) => {
  const { children, heading } = props

  return (
    <div
      className={`
        space-y-2
        border-b
        border-gray-300
        pb-4
      `}
    >
      <h2
        className={`
          text-lg
          font-medium
          leading-none
        `}
      >
        {heading}
      </h2>
      {children && (
        <div
          className={`
          text-sm
          text-gray-500
        `}
        >
          {children}
        </div>
      )}
    </div>
  )
}

export default PageSectionIntro
