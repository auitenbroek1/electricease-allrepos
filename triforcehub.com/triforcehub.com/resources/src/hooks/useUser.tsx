import { useRef } from 'react'

import { useController } from './useController'

export const useUser = (props: any) => {
  const { initial } = props

  const { save } = useController({ endpoint: `/api/users` })

  //

  const name_ref = useRef<HTMLInputElement>(null)
  const email_ref = useRef<HTMLInputElement>(null)
  const password_ref = useRef<HTMLInputElement>(null)
  const password_confirmation_ref = useRef<HTMLInputElement>(null)

  //

  const asyncSave = async () => {
    const payload = {
      member_id: initial?.member_id,

      name: name_ref?.current?.value,
      email: email_ref?.current?.value,
      password: password_ref?.current?.value,
      password_confirmation: password_confirmation_ref?.current?.value,
    }

    console.log(`debug.user`, `save`, payload)

    return save(initial?.id, payload)
  }

  //

  return {
    name_ref,
    email_ref,
    password_ref,
    password_confirmation_ref,

    asyncSave,
  }
}
