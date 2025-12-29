// @ts-nocheck
// https://github.com/lights0123/fractions

import { range as _range } from 'lodash'

function reduce(numerator, denominator) {
  function gcd(a, b) {
    return b ? gcd(b, a % b) : a
  }

  gcd = gcd(numerator, denominator)
  return [numerator / gcd, denominator / gcd]
}

const superscript = {
  '0': `⁰`,
  '1': `¹`,
  '2': `²`,
  '3': `³`,
  '4': `⁴`,
  '5': `⁵`,
  '6': `⁶`,
  '7': `⁷`,
  '8': `⁸`,
  '9': `⁹`,
  '+': `⁺`,
  '-': `⁻`,
  '=': `⁼`,
  '(': `⁽`,
  ')': `⁾`,
  a: `ᵃ`,
  b: `ᵇ`,
  c: `ᶜ`,
  d: `ᵈ`,
  e: `ᵉ`,
  f: `ᶠ`,
  g: `ᵍ`,
  h: `ʰ`,
  i: `ⁱ`,
  j: `ʲ`,
  k: `ᵏ`,
  l: `ˡ`,
  m: `ᵐ`,
  n: `ⁿ`,
  o: `ᵒ`,
  p: `ᵖ`,
  r: `ʳ`,
  s: `ˢ`,
  t: `ᵗ`,
  u: `ᵘ`,
  v: `ᵛ`,
  w: `ʷ`,
  x: `ˣ`,
  y: `ʸ`,
  z: `ᶻ`,
  ' ': ` `,
}

const subscript = {
  '0': `₀`,
  '1': `₁`,
  '2': `₂`,
  '3': `₃`,
  '4': `₄`,
  '5': `₅`,
  '6': `₆`,
  '7': `₇`,
  '8': `₈`,
  '9': `₉`,
  '+': `₊`,
  '-': `₋`,
  '=': `₌`,
  '(': `₍`,
  ')': `₎`,
  a: `ₐ`,
  e: `ₑ`,
  h: `ₕ`,
  i: `ᵢ`,
  j: `ⱼ`,
  k: `ₖ`,
  l: `ₗ`,
  m: `ₘ`,
  n: `ₙ`,
  o: `ₒ`,
  p: `ₚ`,
  r: `ᵣ`,
  s: `ₛ`,
  t: `ₜ`,
  u: `ᵤ`,
  v: `ᵥ`,
  x: `ₓ`,
  ' ': ` `,
}

const fractions = {
  '1/2': `½`,
  '1/3': `⅓`,
  '2/3': `⅔`,
  '1/4': `¼`,
  '3/4': `¾`,
  '1/5': `⅕`,
  '2/5': `⅖`,
  '3/5': `⅗`,
  '4/5': `⅘`,
  '1/6': `⅙`,
  '5/6': `⅚`,
  '1/7': `⅐`,
  '1/8': `⅛`,
  '3/8': `⅜`,
  '5/8': `⅝`,
  '7/8': `⅞`,
  '1/9': `⅑`,
  '1/10': `⅒`,
}

const slash = `⁄`

function getFraction(numerator, denominator) {
  numerator = numerator.trim()
  denominator = denominator.trim()

  function map(num, den) {
    if (fractions[num + `/` + den]) return fractions[num + `/` + den]
    let numOut = ``,
      denOut = ``
    num.split(``).forEach(function (val) {
      const correspondingNum = superscript[val]
      if (!correspondingNum) throw new Error()
      numOut += correspondingNum
    })
    den.split(``).forEach(function (val) {
      const correspondingNum = subscript[val]
      if (!correspondingNum) throw new Error()
      denOut += correspondingNum
    })
    return numOut + slash + denOut
  }

  const orig = map(numerator, denominator)
  let simp = ``
  if (/^\d+$/.test(numerator) && /^\d+$/.test(denominator)) {
    simp = reduce(numerator, denominator)
    simp = map(simp[0].toString(), simp[1].toString())
  }
  if (simp === orig) simp = ``
  return [orig, simp]
}

//

const all_fractions = []

const range = _range(1, 65).reverse()

for (const numerator of range) {
  for (const denominator of range) {
    const fraction = getFraction(String(numerator), String(denominator))
    all_fractions.push({ key: fraction[0], value: numerator / denominator })
  }
}

// console.log(`debug.fractions`, all_fractions)

const fraction_to_decimal = (input: string): string => {
  console.log(`debug.units`, `fraction_to_decimal`, { input })
  try {
    input = input.replace(`,`, ``)
    for (const fraction of all_fractions) {
      if (input.endsWith(fraction.key)) {
        console.log(`debug.units`, `fraction_to_decimal`, { input, fraction })
        const whole = input.replace(fraction.key, ``)
        const decimal = fraction.value
        const output = String(Number(whole) + decimal)
        console.log(`debug.units`, `fraction_to_decimal`, {
          whole,
          decimal,
          output,
        })
        return output
      }
    }
  } catch (error) {
    console.log(`debug.units`, `fraction_to_decimal`, error)
  }
  console.log(`debug.units`, `fraction_to_decimal`, { input })
  return input
}

const parse_into_units = (contents: string) => {
  const input = contents.split(`-`)
  console.log(`debug.units`, input)
  let feet = 0
  let inches = 0

  if (input.length === 2) {
    feet = fraction_to_decimal(input[0]?.replace(`'`, ``))
    inches = fraction_to_decimal(input[1]?.replace(`"`, ``))
    console.log(`debug.units`, { feet, inches })
  } else {
    const unit = input[0]
    feet = unit.includes(` ft`) ? unit.replace(` ft`, ``) : null
    if (feet) {
      feet = fraction_to_decimal(feet)
    }
    inches = unit.includes(`"`) ? unit.replace(`"`, ``) : null
    if (inches) {
      inches = fraction_to_decimal(inches)
    }
    console.log(`debug.units`, { feet, inches })
  }

  const output = {
    feet: Number(feet ?? 0),
    inches: Number(inches ?? 0),
  }

  console.log(`debug.units`, { output })

  return output
}

export const get_quantity_from_units = (contents) => {
  try {
    const units = parse_into_units(contents)
    console.log(`debug.units`, `get_quantity_from_units`, units)
    let quantity = 0
    quantity += units.feet
    quantity += units.inches / 12
    console.log(`debug.units`, `get_quantity_from_units`, quantity)
    return quantity
  } catch (error) {
    console.log(`debug.units`, error)
  }
  return 0
}
