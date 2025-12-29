import { ReactNode } from 'react'

import { FieldInvalidIcon, FieldValidIcon } from '../Icons'
import { Tooltip } from '../Tooltip'

type PropsType = {
  children?: ReactNode
  disabled?: boolean
  invalid?: boolean
  messages?: string[]
  prefix?: ReactNode
  suffix?: ReactNode
  valid?: boolean
}

export const Field = (props: PropsType) => {
  const { children, disabled, invalid, messages, prefix, suffix, valid } = props

  const red = invalid && !disabled
  const green = valid && !invalid && !disabled

  return (
    <div
      className={`
        rounded
        border
        border-gray-300 ${disabled && `!border-gray-50`}
        bg-white ${disabled && `!bg-gray-50`}
        pointer-events-auto ${disabled && `!pointer-events-none`}
        text-inherit ${disabled && `!text-gray-600`} ${red && `!text-red-600`}
        text-sm
        focus-within:border-blue-600 ${red && `focus-within:!border-red-600`}
        focus-within:ring-2
        focus-within:ring-blue-200 ${red && `focus-within:!ring-red-200`}
        focus-within:ring-offset-1
      `}
    >
      <div className="-m-px">
        <div className="flex space-x-1 py-1.5 px-2">
          {prefix && (
            <>
              <div className="pointer-events-none">{prefix}</div>
            </>
          )}
          <div className="w-full">{children}</div>
          {red && (
            <Tooltip messages={messages}>
              <div className="h-5 w-5">
                <FieldInvalidIcon />
              </div>
            </Tooltip>
          )}
          {green && (
            <Tooltip messages={messages}>
              <div className="h-5 w-5 text-green-600">
                <FieldValidIcon />
              </div>
            </Tooltip>
          )}
          {suffix && (
            <>
              <div className="pointer-events-none">{suffix}</div>
            </>
          )}
        </div>
      </div>
    </div>
  )
}
