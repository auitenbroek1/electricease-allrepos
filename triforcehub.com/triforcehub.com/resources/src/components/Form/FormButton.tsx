const FormButton = (props: any) => {
  const {
    children,
    onClick,
    type = 'primary',
  } = props

  let classes = `
    bg-white
    border
    border-gray-300
    focus:outline-none
    focus:ring-2
    focus:ring-blue-600
    focus:ring-offset-2
    font-medium
    hover:bg-gray-50
    inline-flex
    justify-center
    min-w-[120px]
    px-4
    py-2
    rounded-md
    shadow-sm
    text-base_ text-sm
    text-gray-700
  `

  if (type === 'primary') {
    classes = `
      bg-blue-600
      border
      border-transparent
      focus:outline-none
      focus:ring-2
      focus:ring-blue-600
      focus:ring-offset-2
      font-medium
      hover:bg-blue-700
      inline-flex
      items-center
      justify-center
      min-w-[120px]
      px-4
      py-2
      rounded-md
      shadow-sm
      space-x-2
      text-base_ text-sm
      text-white
    `
  }

  return (
    <button
      className={classes}
      onClick={onClick}
      type={`button`}
    >
      {children}
    </button>
  )
}

export default FormButton
