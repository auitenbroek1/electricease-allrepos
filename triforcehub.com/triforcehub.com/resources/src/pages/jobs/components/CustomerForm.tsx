import { Form, FormSelect } from '@/components'
import { useJobCustomer } from '@/data/JobCustomer'

export const CustomerForm = (props: any) => {
  const { data } = props

  const {
    name_ref,
    email_ref,
    address1_ref,
    address2_ref,
    city_ref,
    state_id_ref,
    zip_ref,
    mobile_ref,
    office_ref,

    sync,
    loading,
    errors,
  } = useJobCustomer(data)

  const states = [
    { value: ``, key: `` },
    { value: `Alabama`, key: `1` },
    { value: `Alaska`, key: `2` },
    { value: `Arizona`, key: `3` },
    { value: `Arkansas`, key: `4` },
    { value: `California`, key: `5` },
    { value: `Colorado`, key: `6` },
    { value: `Connecticut`, key: `7` },
    { value: `Delaware`, key: `8` },
    { value: `District of Columbia`, key: `9` },
    { value: `Florida`, key: `10` },
    { value: `Georgia`, key: `11` },
    { value: `Hawaii`, key: `12` },
    { value: `Idaho`, key: `13` },
    { value: `Illinois`, key: `14` },
    { value: `Indiana`, key: `15` },
    { value: `Iowa`, key: `16` },
    { value: `Kansas`, key: `17` },
    { value: `Kentucky`, key: `18` },
    { value: `Louisiana`, key: `19` },
    { value: `Maine`, key: `20` },
    { value: `Maryland`, key: `21` },
    { value: `Massachusetts`, key: `22` },
    { value: `Michigan`, key: `23` },
    { value: `Minnesota`, key: `24` },
    { value: `Mississippi`, key: `25` },
    { value: `Missouri`, key: `26` },
    { value: `Montana`, key: `27` },
    { value: `Nebraska`, key: `28` },
    { value: `Nevada`, key: `29` },
    { value: `New Hampshire`, key: `30` },
    { value: `New Jersey`, key: `31` },
    { value: `New Mexico`, key: `32` },
    { value: `New York`, key: `33` },
    { value: `North Carolina`, key: `34` },
    { value: `North Dakota`, key: `35` },
    { value: `Ohio`, key: `36` },
    { value: `Oklahoma`, key: `37` },
    { value: `Oregon`, key: `38` },
    { value: `Pennsylvania`, key: `39` },
    { value: `Rhode Island`, key: `40` },
    { value: `South Carolina`, key: `41` },
    { value: `South Dakota`, key: `42` },
    { value: `Tennessee`, key: `43` },
    { value: `Texas`, key: `44` },
    { value: `Utah`, key: `45` },
    { value: `Vermont`, key: `46` },
    { value: `Virginia`, key: `47` },
    { value: `Washington`, key: `48` },
    { value: `West Virginia`, key: `49` },
    { value: `Wisconsin`, key: `50` },
    { value: `Wyoming`, key: `51` },
    { value: `British Columbia`, key: `52` },
    { value: `Ontario`, key: `53` },
    { value: `Newfoundland and Labrador`, key: `54` },
    { value: `Nova Scotia`, key: `55` },
    { value: `Prince Edward Island`, key: `56` },
    { value: `New Brunswick`, key: `57` },
    { value: `Quebec`, key: `58` },
    { value: `Manitoba`, key: `59` },
    { value: `Saskatchewan`, key: `60` },
    { value: `Alberta`, key: `61` },
    { value: `Northwest Territories`, key: `62` },
    { value: `Nunavut`, key: `63` },
    { value: `Yukon Territory`, key: `64` },
    { value: `Puerto Rico`, key: `65` },
  ]

  return (
    <div className="space-y-4">
      <div className="grid grid-cols-1 gap-4">
        <div className="">
          <Form.Control
            label={`Customer Name`}
            field={
              <Form.Input
                defaultValue={data?.name}
                ref={name_ref}
              />
            }
            errors={errors?.name}
          />
        </div>
      </div>
      <div className="grid grid-cols-1 gap-4">
        <div className="">
          <Form.Control
            label={`Email`}
            field={
              <Form.Input
                defaultValue={data?.email}
                ref={email_ref}
              />
            }
            errors={errors?.email}
          />
        </div>
      </div>
      <div className="grid grid-cols-2 gap-4">
        <div className="">
          <Form.Control
            label={`Address 1`}
            field={
              <Form.Input
                defaultValue={data?.address1}
                ref={address1_ref}
              />
            }
            errors={errors?.address1}
          />
        </div>
        <div className="">
          <Form.Control
            label={`Address 2`}
            field={
              <Form.Input
                defaultValue={data?.address2}
                ref={address2_ref}
              />
            }
            errors={errors?.address2}
          />
        </div>
      </div>
      <div className="grid grid-cols-4 gap-4">
        <div className="col-span-2">
          <Form.Control
            label={`City`}
            field={
              <Form.Input
                defaultValue={data?.city}
                ref={city_ref}
              />
            }
            errors={errors?.city}
          />
        </div>
        <div className="">
          <FormSelect
            label={`State`}
            options={states}
            ref={state_id_ref}
            value={data?.state?.id}
          />
        </div>
        <div className="">
          <Form.Control
            label={`ZIP`}
            field={
              <Form.Input
                defaultValue={data?.zip}
                ref={zip_ref}
              />
            }
            errors={errors?.zip}
          />
        </div>
      </div>
      <div className="grid grid-cols-2 gap-4">
        <div className="">
          <Form.Control
            label={`Mobile`}
            field={
              <Form.Input
                defaultValue={data?.mobile}
                ref={mobile_ref}
              />
            }
            errors={errors?.mobile}
          />
        </div>
        <div className="">
          <Form.Control
            label={`Office`}
            field={
              <Form.Input
                defaultValue={data?.office}
                ref={office_ref}
              />
            }
            errors={errors?.office}
          />
        </div>
      </div>
      <Form.Buttons>
        <Form.Button onClick={sync}>Save</Form.Button>
      </Form.Buttons>
    </div>
  )
}
