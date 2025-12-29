import { useEffect, useRef, useState } from 'react'

import { Input } from './Input'

const Cost = (props: any) => {
  const {
    disabled,
    onChange,
    value,
  } = props

  const [unit, setUnit] = useState('dollar')

  const inputCents = useRef<HTMLInputElement>(null)
  const inputDollars = useRef<HTMLInputElement>(null)
  const inputFormatted = useRef<HTMLInputElement>(null)

  const centsToDollars = (cents: number): number => {
    let dollars = cents / 100
    dollars = Number(dollars.toFixed(2))
    return dollars
  }

  const dollarsToCents = (dollars: number): number => {
    let cents = dollars * 100
    cents = Math.round(cents)
    return cents
  }

  const format = (dollars: number): string => {
    const locales = [
      'en-US'
    ]

    const options = {
      currency: 'USD',
      style: 'currency',
    }

    const formatted = new Intl.NumberFormat(locales, options).format(dollars)

    return formatted
  }

  //

  const handleCentsChange = (event: any) => {
    const input = event.target.valueAsNumber
    const valid = event.target.validity.valid
    const message = event.target.validationMessage

    // console.log('input', input, valid, message)

    const cents = input && valid ? input : 0

    onChange(cents)
  }

  const handleDollarsChange = (event: any) => {
    const input = event.target.valueAsNumber
    const valid = event.target.validity.valid
    const message = event.target.validationMessage

    // console.log('input', input, valid, message)

    const dollars = input && valid ? input : 0
    const cents = dollarsToCents(dollars)

    onChange(cents)
  }

  useEffect(() => {
    const cents = value ?? 0
    const dollars = centsToDollars(cents)
    const formatted = format(dollars)

    if (inputCents?.current) {
      inputCents.current.value = cents.toString()
    }

    if (inputDollars?.current) {
      inputDollars.current.value = dollars.toString()
    }

    if (inputFormatted?.current) {
      inputFormatted.current.value = formatted
    }
  }, [value])

  return (
    <div className={`relative w-32`}>
      <div
        className={`
          ${value ? 'opacity-0' : ''}
          focus-within:opacity-100
          relative
          z-10
        `}
      >
        <div
          className={`
            ${unit === 'cent' ? '' : 'hidden'}
          `}
        >
          <Input
            disabled={disabled}
            min={0}
            onChange={handleCentsChange}
            ref={inputCents}
            required
            step={1}
            type="number"
          />
        </div>
        <div
          className={`
            ${unit === 'dollar' ? '' : 'hidden'}
          `}
        >
          <Input
            disabled={disabled}
            min={0}
            onChange={handleDollarsChange}
            ref={inputDollars}
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
