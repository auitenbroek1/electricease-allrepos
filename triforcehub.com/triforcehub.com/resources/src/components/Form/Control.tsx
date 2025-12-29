import { Description } from './Description'
import { Error } from './Error'
import { Label } from './Label'

export const Control = (props: any) => {
  const { children, cols, description, error, errors, field, label, required } =
    props

  // TODO: remove cols
  // TODO: remove errors, keep error
  // TODO: replace space-y-2 with spacer

  return (
    <div
      className={`
        space-y-2
        ${cols === 1 ? `col-span-1` : ``}
        ${cols === 2 ? `col-span-2` : ``}
        ${cols === 3 ? `col-span-3` : ``}
        ${cols === 4 ? `col-span-4` : ``}
      `}
    >
      {label && (
        <Label>
          {label}
          {required ? (
            <span
              className={`ml-1 text-red-500`}
              title={`Required`}
            >
              *
            </span>
          ) : (
            ``
          )}
        </Label>
      )}
      {description && <Description>{description}</Description>}
      {field}
      {errors && <Error>{errors}</Error>}
      {error?.message && <Error>{error.message}</Error>}
      {children}
    </div>
  )
}
