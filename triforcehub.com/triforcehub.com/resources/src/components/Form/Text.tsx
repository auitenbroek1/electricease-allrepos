import { forwardRef, HTMLProps, ReactNode, Ref } from 'react'

import { Field } from './Field'

type PropsType = HTMLProps<HTMLInputElement> & {
  invalid?: boolean
  messages?: string[]
  prefix?: ReactNode
  suffix?: ReactNode
  type?: `email` | `number` | `text`
  valid?: boolean
}

type RefType = Ref<HTMLInputElement>

export const Text = forwardRef((props: PropsType, ref: RefType) => {
  const {
    invalid,
    messages,
    prefix,
    suffix,
    type = `text`,
    valid,
    ...input
  } = props

  return (
    <Field
      disabled={input.disabled}
      invalid={invalid}
      messages={messages}
      prefix={prefix}
      suffix={suffix}
      valid={valid}
    >
      <input
        className={`
          w-full
          appearance-none
          bg-transparent
          placeholder:text-inherit
          placeholder:opacity-30
          focus:outline-none
        `}
        ref={ref}
        type={type}
        {...input}
      />
    </Field>
  )
})

Text.displayName = `Text`
