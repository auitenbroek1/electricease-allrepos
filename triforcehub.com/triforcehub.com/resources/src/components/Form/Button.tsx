export const Button = (props: any) => {
  const { children, onClick, type } = props

  return (
    <button
      className={`
        inline-flex
        min-w-[120px]
        items-center
        justify-center
        space-x-2
        rounded-md
        border
        border-transparent
        bg-blue-600
        px-4
        py-2
        text-sm
        font-medium
        text-white
        transition-all
        hover:bg-blue-700
        focus:outline-none
        focus:ring-2
        focus:ring-blue-600
        focus:ring-offset-2
        ${type === `secondary` ? `!border-gray-300` : ``}
        ${type === `secondary` ? `!bg-white` : ``}
        ${type === `secondary` ? `!text-gray-700` : ``}
      `}
      onClick={onClick}
      type={`button`}
    >
      {children}
    </button>
  )
}
