import { forwardRef } from 'react'

import { Label } from './Label'

const FormSelect = (props: any, ref: any) => {
  const { disabled, label, onChange, options, placeholder, value } = props

  return (
    <div
      className={`
      space-y-2
    `}
    >
      {label ? <Label>{label}</Label> : null}
      <div>
        <select
          className={`
            block
            border
            border-gray-300
            disabled:bg-gray-50
            disabled:border-gray-200
            disabled:text-gray-600
            focus:border-sky-600
            focus:outline-none
            focus:ring-2
            focus:ring-offset-1
            focus:ring-sky-200
            placeholder-gray-400
            px-2
            py-2
            rounded
            text-sm
            w-full
          `}
          defaultValue={value}
          disabled={disabled}
          onChange={onChange}
          // @ts-ignore
          placeholder={placeholder}
          ref={ref}
        >
          {options.map((item: any) => (
            <option
              key={item.key}
              value={item.key}
            >
              {item.value}
            </option>
          ))}
        </select>
      </div>
    </div>
  )
}

const FormSelect2 = forwardRef(FormSelect)

export default FormSelect2
