import { forwardRef } from 'react'

const TextareaRef = (props: any, ref: any) => {
  const { className, disabled, ...attributes } = props

  const rows = attributes?.defaultValue?.split(`\n`).length + 1

  return (
    <div
      className={`
        relative
        z-0
        flex
        w-full
        space-x-2
        rounded
        border
        border-gray-300
        bg-white
        px-2
        py-2
        text-sm
        focus-within:border-blue-600
        focus-within:outline-none
        focus-within:ring-2
        focus-within:ring-blue-200
        focus-within:ring-offset-1
        ${disabled ? `!bg-gray-50` : ``}
        ${disabled ? `!border-gray-200` : ``}
        ${disabled ? `!pointer-events-none` : ``}
        ${disabled ? `!text-gray-600` : ``}
      `}
    >
      <div className={`flex-1`}>
        <textarea
          className={`
            relative
            z-0
            w-full
            resize-y
            placeholder-gray-400
            invalid:border-red-600
            read-only:border-transparent
            read-only:bg-transparent
            read-only:text-inherit
            focus:outline-none invalid:focus:border-red-600
            invalid:focus:ring-red-200
          `}
          disabled={disabled}
          ref={ref}
          rows={rows}
          {...attributes}
        />
      </div>
    </div>
  )
}

export const Textarea = forwardRef(TextareaRef)
