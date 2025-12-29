import { useRef } from 'react'

import { useController } from './useController'

export const useMember = (props: any) => {
  const { initial } = props

  const { save } = useController({ endpoint: `/api/members` })

  //

  const name_ref = useRef<HTMLInputElement>(null)
  const email_ref = useRef<HTMLInputElement>(null)
  const address1_ref = useRef<HTMLInputElement>(null)
  const address2_ref = useRef<HTMLInputElement>(null)
  const city_ref = useRef<HTMLInputElement>(null)
  const state_id_ref = useRef<HTMLInputElement>(null)
  const zip_ref = useRef<HTMLInputElement>(null)
  const mobile_ref = useRef<HTMLInputElement>(null)
  const office_ref = useRef<HTMLInputElement>(null)
  const logo_id_ref = useRef<HTMLInputElement>(null)

  const feature_digital_takeoff_enabled_ref = useRef<HTMLInputElement>(null)
  const feature_linear_with_drops_enabled_ref = useRef<HTMLInputElement>(null)
  const feature_auto_count_enabled_ref = useRef<HTMLInputElement>(null)

  //

  const asyncSave = async () => {
    const payload = {
      name: name_ref?.current?.value,
      email: email_ref?.current?.value,
      address1: address1_ref?.current?.value,
      address2: address2_ref?.current?.value,
      city: city_ref?.current?.value,
      state_id: state_id_ref?.current?.value,
      zip: zip_ref?.current?.value,
      mobile: mobile_ref?.current?.value,
      office: office_ref?.current?.value,
      logo_id: logo_id_ref?.current?.value,

      feature_digital_takeoff_enabled:
        feature_digital_takeoff_enabled_ref?.current?.checked ?? true,
      feature_linear_with_drops_enabled:
        feature_linear_with_drops_enabled_ref?.current?.checked ?? false,
      feature_auto_count_enabled:
        feature_auto_count_enabled_ref?.current?.checked ?? true,
    }

    console.log(`debug.member`, `save`, payload)

    return save(initial?.id, payload)
  }

  //

  return {
    name_ref,
    email_ref,
    address1_ref,
    address2_ref,
    city_ref,
    state_id_ref,
    zip_ref,
    mobile_ref,
    office_ref,
    logo_id_ref,

    feature_digital_takeoff_enabled_ref,
    feature_linear_with_drops_enabled_ref,
    feature_auto_count_enabled_ref,

    asyncSave,
  }
}
