type FormNumberProps = {
  decimals?: number,
  disabled?: boolean,
  onChange?: any,
  prefix?: string,
  value?: any,
}

const FormNumber = (props: FormNumberProps) => {
  const {
    decimals = 0,
    disabled = false,
    onChange,
    prefix,
    value,
  } = props

  const placeholder = Number(0).toFixed(decimals)
  const step = (1 / Math.pow(10, decimals)).toFixed(decimals)

  const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    if (onChange) {
      onChange(event.target.value)
    }
  }

  return (
    <div className="relative w-32">
      {prefix ?
        <div className="absolute inset-y-0 left-0 pl-2 flex items-center pointer-events-none">
          <span className="text-gray-500 sm:text-sm">
            {prefix}
          </span>
        </div> :
        null
      }
      <input
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
          text-right
          text-sm
          w-full
        `}
        disabled={disabled}
        min={0}
        onChange={handleChange}
        placeholder={placeholder}
        step={step}
        type={`number`}
        value={value}
      />
    </div>
  )
}


export default FormNumber
