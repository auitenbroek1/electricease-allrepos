import axios from 'axios'
import { FieldError } from 'react-hook-form'

const isValidationError = (
  exception: unknown,
): exception is ValidationError => {
  return (
    exception !== null && typeof exception === `object` && `errors` in exception
    // && typeof (exception as Record<string, unknown>).errors === `string`
  )
}

export const getErrors = (exception: unknown): ValidationError => {
  console.log(`debug.exception`, exception)

  if (axios.isAxiosError(exception)) {
    if (exception.response?.data) {
      const response = exception.response
      const data = response.data
      if (response.status === 422) {
        if (isValidationError(data)) return data
      }
      return {
        errors: { '*': [exception.message] },
        message: exception.message,
      }
    }
  }

  if (isValidationError(exception)) return exception

  try {
    return {
      errors: [],
      message: JSON.stringify(exception),
    }
  } catch {
    // fallback in case there's an error stringifying the maybeError
    // like with circular references for example.
    return {
      errors: [],
      message: ``,
    }
  }
}

interface CustomFormError<T> {
  name: keyof T
  error: FieldError
}

export const getFormErrors = <T,>(input: ValidationError) => {
  console.log(`debug.exception.input`, input)

  const errors: CustomFormError<T>[] = []
  for (const key in input.errors) {
    errors.push({
      name: key as keyof T,
      error: {
        message: input.errors[key].join(`, `),
        type: `custom`,
      },
    })
  }

  console.log(`debug.exception.output`, errors)

  return errors
}
