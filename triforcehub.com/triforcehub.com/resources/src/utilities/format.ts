export const currency = (value: any) => {
  return new Intl.NumberFormat(`en-US`, {
    currency: `USD`,
    maximumFractionDigits: 6,
    minimumSignificantDigits: 1,
    style: `currency`,
  }).format(normalize(value))
}

export const decimal = (value: any) => {
  return number(value).replaceAll(`,`, ``)
}

export const normalize = (value: any) => {
  let output = value ?? 0
  if (typeof value === `number`) {
    output = output.toFixed(6)
  }
  return output
}

export const number = (value: any) => {
  return new Intl.NumberFormat(`en-US`, {
    maximumFractionDigits: 6,
    minimumSignificantDigits: 1,
  }).format(normalize(value))
}
