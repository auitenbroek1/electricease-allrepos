import { forwardRef, useState } from 'react'

import { Switch } from '@headlessui/react'

import { Label } from './Label'

function classNames(...classes: any) {
  return classes.filter(Boolean).join(` `)
}

const FormToggle = forwardRef(function FormToggle(props: any, ref: any) {
  const { defaultValue, disabled, label, onChange } = props

  // console.log(`debug.toggle`, defaultValue)

  const [checked, setChecked] = useState(defaultValue ?? 0)

  // console.log(`debug.toggle.checked`, checked)

  const toggle = () => {
    if (checked) {
      setChecked(0)
    } else {
      setChecked(1)
    }
    onChange && onChange()
  }

  return (
    <div
      className={`
        space-y-2
        ${disabled ? `pointer-events-none` : ``}
      `}
    >
      {label ? <Label>{label}</Label> : null}
      <div className={`flex h-[38px] items-center`}>
        <input
          defaultChecked={checked}
          defaultValue={checked}
          className={`hidden`}
          onChange={onChange}
          ref={ref}
          type={`checkbox`}
          // value={checked ? 1 : 0}
        />
        <Switch
          checked={checked}
          onChange={toggle}
          className={classNames(
            checked ? `bg-blue-600` : `bg-gray-200`,
            `relative inline-flex h-6 w-11 flex-shrink-0 cursor-pointer rounded-full border-2 border-transparent transition-colors duration-200 ease-in-out focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2`,
            disabled ? `bg-blue-100` : ``,
          )}
        >
          <span
            className={classNames(
              checked ? `translate-x-5` : `translate-x-0`,
              `pointer-events-none inline-block h-5 w-5 transform rounded-full bg-white shadow ring-0 transition duration-200 ease-in-out`,
            )}
          />
        </Switch>
      </div>
    </div>
  )
})

export default FormToggle
