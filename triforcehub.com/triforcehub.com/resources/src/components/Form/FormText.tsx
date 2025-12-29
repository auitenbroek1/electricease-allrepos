import { forwardRef } from 'react'

import { Label } from './Label'

const FormText = (props: any, ref: any) => {
  const {
    disabled,
    label,
    name,
    onChange,
    placeholder,
    type = 'text',
    value
  } = props

  return (
    <div className={`
      space-y-2
    `}>
      {label ?
        <Label>
          {label}
        </Label>
        : null
      }
      <div>
        <input
          autoComplete={`off`}
          className={`
            block
            border
            border-gray-300
            disabled:bg-gray-50
            disabled:border-gray-200
            disabled:text-gray-600
            focus:border-blue-500
            focus:outline-none
            focus:ring-2
            focus:ring-blue-500
            focus:ring-offset-1
            placeholder-gray-400
            px-2
            py-2
            rounded
            text-sm
            w-full
          `}
          defaultValue={value}
          disabled={disabled}
          name={name}
          onChange={onChange}
          placeholder={placeholder}
          ref={ref}
          type={type}
        />
      </div>
    </div>
  )
}

const FormText2 = forwardRef(FormText)

export default FormText2
