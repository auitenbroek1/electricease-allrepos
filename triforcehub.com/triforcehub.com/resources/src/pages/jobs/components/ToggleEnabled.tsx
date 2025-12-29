import { forwardRef, useContext, useEffect, useRef } from 'react'
import axios from 'axios'
import { Tooltip } from '@mantine/core'
import JobsContext from './../contexts/Job'
import toast from 'react-hot-toast'

const CHECKBOX_STATES = {
  Checked: `Checked`,
  Indeterminate: `Indeterminate`,
  Empty: `Empty`,
}

export const ToggleEnabled = (props: any) => {
  const { assemblies, parts } = props

  //

  const items = assemblies ?? parts

  // console.log(`test`, { assemblies, parts, items })

  const all = items.every((item: any) => item.enabled)
  const none = items.every((item: any) => !item.enabled)
  const some = items.some((item: any) => item.enabled) && !all

  let checked = CHECKBOX_STATES.Indeterminate
  if (all) checked = CHECKBOX_STATES.Checked
  if (none) checked = CHECKBOX_STATES.Empty

  // console.log(`test`, { all, some, none, checked })

  //

  const { job, setJob }: any = useContext(JobsContext)

  const handleChange = async () => {
    const key = assemblies ? `assemblies` : `parts`
    const endpoint = `/api/jobs/${key}/enabled`
    const enabled = checked === CHECKBOX_STATES.Empty ? true : false

    const payload = items.map((item: any) => {
      return {
        id: item.id,
        enabled,
      }
    })

    try {
      const response1 = await axios.post(endpoint, payload)
      console.log(`test`, response1)

      const response2 = await axios.get(`/api/jobs/${job.id}`)
      const data2 = response2.data.data

      setJob(data2)
      // setErrors([])
      toast.success(`Saved!`)
    } catch (exception: any) {
      // setErrors(exception.response?.data?.errors ?? [])
    }
  }

  return (
    <div
      className="text-xs normal-case"
      key={checked}
    >
      <Checkbox
        onChange={handleChange}
        value={checked}
      />
    </div>
  )
}

const Checkbox = forwardRef((props: any, ref: any) => {
  const { value, onChange } = props
  // console.log(`test.value`, value)

  const checkboxRef = useRef<any>()

  useEffect(() => {
    if (value === CHECKBOX_STATES.Checked) {
      checkboxRef.current.checked = true
      checkboxRef.current.indeterminate = false
    } else if (value === CHECKBOX_STATES.Empty) {
      checkboxRef.current.checked = false
      checkboxRef.current.indeterminate = false
    } else if (value === CHECKBOX_STATES.Indeterminate) {
      checkboxRef.current.checked = false
      checkboxRef.current.indeterminate = true
    }
  }, [value])

  let label =
    value === CHECKBOX_STATES.Checked
      ? `All Included in the Material Summary`
      : `All Excluded from the Material Summary`

  if (value === CHECKBOX_STATES.Indeterminate) {
    label = `Some Included in the Material Summary`
  }

  return (
    <Tooltip
      arrowSize={4}
      label={label}
      withArrow
    >
      <input
        className={`cursor-pointer`}
        onChange={onChange}
        ref={checkboxRef}
        type="checkbox"
      />
    </Tooltip>
  )
})

Checkbox.displayName = `Checkbox`
