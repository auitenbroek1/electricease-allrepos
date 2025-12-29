import { forwardRef } from 'react'

const InputRef = (props: any, ref: any) => {
  const { className, disabled, prefix, suffix, ...attributes } = props

  return (
    <div
      className={`
        relative
        z-0
        flex
        h-9
        w-full
        items-center
        space-x-2
        rounded
        border
        border-gray-300
        bg-white
        px-2
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
      {prefix && (
        <div
          className={`
            pointer-events-none
          `}
        >
          {prefix}
        </div>
      )}
      <div className={`flex-1`}>
        <input
          className={`
            relative
            z-0
            w-full
            placeholder-gray-400
            invalid:border-red-600
            read-only:border-transparent
            read-only:bg-transparent
            read-only:text-inherit
            focus:outline-none
            invalid:focus:border-red-600 invalid:focus:ring-red-200
          `}
          disabled={disabled}
          ref={ref}
          {...attributes}
        />
      </div>
      {suffix && (
        <div
          className={`
            pointer-events-none
          `}
        >
          {suffix}
        </div>
      )}
    </div>
  )
}

export const Input = forwardRef(InputRef)
