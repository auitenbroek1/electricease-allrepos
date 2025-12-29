import { useEffect, useRef, useState } from 'react'

import { Input } from './Input'

const Cost = (props: any) => {
  const {
    disabled,
    onChange,
    value,
  } = props

  const useFormatted = false

  const [unit, setUnit] = useState('hour')

  const inputMinutes = useRef<HTMLInputElement>(null)
  const inputHours = useRef<HTMLInputElement>(null)
  const inputFormatted = useRef<HTMLInputElement>(null)

  const minutesToHours = (minutes: number): number => {
    let hours = minutes / 60
    hours = Number(hours.toFixed(2))
    return hours
  }

  const hoursToMinutes = (hours: number): number => {
    let minutes = hours * 60
    minutes = Math.round(minutes)
    return minutes
  }

  const format = (minutes: number): string => {
    const h = Math.floor(minutes / 60)
    const m = minutes - (h * 60)

    const formatted = []
    if (h > 0) formatted.push(h + 'h')
    if (m > 0) formatted.push(m + 'm')

    return formatted.join(' ')
  }

  //

  const handleMinutesChange = (event: any) => {
    const input = event.target.valueAsNumber
    const valid = event.target.validity.valid
    const message = event.target.validationMessage

    // console.log('input', input, valid, message)

    const minutes = input && valid ? input : 0

    onChange(minutes)
  }

  const handleHoursChange = (event: any) => {
    const input = event.target.valueAsNumber
    const valid = event.target.validity.valid
    const message = event.target.validationMessage

    // console.log('input', input, valid, message)

    const hours = input && valid ? input : 0
    const minutes = hoursToMinutes(hours)

    onChange(minutes)
  }

  useEffect(() => {
    const minutes = value ?? 0
    const hours = minutesToHours(minutes)
    const formatted = format(minutes)

    if (inputMinutes?.current) {
      inputMinutes.current.value = minutes.toString()
    }

    if (inputHours?.current) {
      inputHours.current.value = hours.toString()
    }

    if (inputFormatted?.current) {
      inputFormatted.current.value = formatted
    }
  }, [value])

  return (
    <div className={`relative w-32`}>
      <div
        className={`
          ${useFormatted ? (value ? 'opacity-0' : '') : ''}
          focus-within:opacity-100
          relative
          z-10
        `}
      >
        <div
          className={`
            ${unit === 'minute' ? '' : 'hidden'}
          `}
        >
          <Input
            disabled={disabled}
            min={0}
            onChange={handleMinutesChange}
            ref={inputMinutes}
            required
            step={1}
            type="number"
          />
        </div>
        <div
          className={`
            ${unit === 'hour' ? '' : 'hidden'}
          `}
        >
          <Input
            disabled={disabled}
            min={0}
            onChange={handleHoursChange}
            ref={inputHours}
            required
            step={0.01}
            type="number"
          />
        </div>
      </div>
      <div
        className={`
          absolute
          inset-0
          ${useFormatted ? '' : 'opacity-0'}
          pointer-events-none
          z-0
        `}
      >
        <Input
          disabled={disabled}
          readOnly
          ref={inputFormatted}
          tabIndex={-1}
          type={`text`}
        />
      </div>
    </div>
  )
}

export default Cost
