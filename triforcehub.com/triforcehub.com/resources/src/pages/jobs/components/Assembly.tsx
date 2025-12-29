import { useContext, useEffect, useState } from 'react'

import TableBody from '@/components/Table/TableBody'
import TableBodyCell from '@/components/Table/TableBodyCell'
import FormNumber from '@/components/Form/FormNumber'

import Parts from './Parts'
import { ChevronDownIcon, ChevronRightIcon } from '@/components/Icons'

import useDebounce from '@/hooks/debounce'
import Cost from '@/components/Form/Cost'
import Labor from '@/components/Form/Labor'

import JobsContext from './../contexts/Job'
import AssemblyActions from './AssemblyActions'

import AssemblyContext from '../contexts/Assembly'
import axios from 'axios'
import { Form } from '@/components'
import { decimal } from '@/utilities/format'
import { Tooltip } from '@mantine/core'
import toast from 'react-hot-toast'

const Assembly = (props: any) => {
  const { index, data, focus, handleFocus } = props

  const { job, setJob }: any = useContext(JobsContext)

  const [mounted, setMounted] = useState(false)

  const [quantity, setQuantity] = useState<any>(data.quantity)
  const [excludeEnabled, setExcludeEnabled] = useState<any>(data.enabled)

  const quantity2 = useDebounce(quantity)

  const [labor_factor, setLaborFactor] = useState(data.labor_factor)
  const labor_factor2 = useDebounce(labor_factor)

  const [showParts, setShowParts] = useState(false)

  const toggle = () => {
    if (showParts) {
      setShowParts(false)
      handleFocus(null)
    } else {
      setShowParts(true)
      handleFocus(index)
    }
  }

  //

  const [errors, setErrors] = useState<any>([])

  useEffect(() => {
    if (!mounted) return

    const sync = async () => {
      console.log(`sync`, `here!`)

      const payload = {
        quantity: quantity2,
        labor_factor: labor_factor2,
        enabled: excludeEnabled,
      }

      try {
        const response1 = await axios.patch(
          `/api/jobs/assemblies/${data.id}`,
          payload,
        )
        console.log(response1)

        const response2 = await axios.get(`/api/jobs/${job.id}`)
        const data2 = response2.data.data

        setJob(data2)
        setErrors([])
        toast.success(`Saved!`)
      } catch (exception: any) {
        setErrors(exception.response?.data?.errors ?? [])
      }
    }

    sync()
  }, [quantity2, excludeEnabled, labor_factor2])

  useEffect(() => {
    setMounted(true)
  }, [])

  return (
    <>
      <AssemblyContext.Provider value={data}>
        <TableBody disabled={!(showParts || focus === null)}>
          <tr className={data.enabled === 0 ? `bg-yellow-50` : ``}>
            <TableBodyCell>
              <button
                className={`
                  group
                  -m-2
                  flex
                  h-9
                  w-9
                  items-center
                  justify-center
                  rounded-full
                  p-2
                  hover:bg-blue-50
                  focus:outline-none
                `}
                onClick={toggle}
                type={`button`}
              >
                {showParts ? (
                  <div
                    className={`h-4 w-4 text-blue-600 group-hover:text-blue-700`}
                  >
                    <ChevronDownIcon />
                  </div>
                ) : (
                  <div
                    className={`h-4 w-4 text-blue-600 group-hover:text-blue-700`}
                  >
                    <ChevronRightIcon />
                  </div>
                )}
              </button>
            </TableBodyCell>
            <TableBodyCell>
              <div className="space-y-1">
                {data.reference?.categories?.length > 0 ? (
                  <div className="text-xs">
                    {data.reference?.categories?.map((category: any) => (
                      <div key={category.id}>{category.name}</div>
                    ))}
                  </div>
                ) : null}
                <div className={`text-sm text-black`}>
                  {data.reference?.name}
                </div>
                {data.reference?.tags?.length > 0 ? (
                  <div className="flex space-x-2 text-xs">
                    {data.reference?.tags?.map((tag: any) => (
                      <div
                        className={`flex items-center space-x-2`}
                        key={tag.id}
                        title={tag.name}
                      >
                        <span
                          className={`block h-3 w-3 rounded-full`}
                          style={{ backgroundColor: tag.color }}
                        ></span>
                        <span>{tag.name}</span>
                      </div>
                    ))}
                  </div>
                ) : null}
              </div>
            </TableBodyCell>
            <TableBodyCell>
              <div className={`w-32 text-right`}>
                <Form.Text
                  disabled
                  prefix={`$`}
                  value={decimal(data.cost)}
                  type={`number`}
                />
              </div>
            </TableBodyCell>
            <TableBodyCell>
              <div className={`w-32 text-right`}>
                <Form.Text
                  disabled
                  value={decimal(data.labor)}
                  type={`number`}
                />
              </div>
            </TableBodyCell>
            <TableBodyCell>
              <div className={`w-32 text-right flex gap-2`}>
                <Form.Text
                  disabled={true}
                  value={decimal(data.quantity_digital)}
                  type={`number`}
                />
                <Form.Text
                  invalid={errors.quantity?.length}
                  messages={errors.quantity}
                  name={`quantity`}
                  onChange={(event: any) => setQuantity(event.target.value)}
                  value={decimal(quantity)}
                  type={`number`}
                />
              </div>
            </TableBodyCell>
            <TableBodyCell>
              <div className={`w-32 text-right`}>
                <Form.Text
                  disabled
                  prefix={`$`}
                  value={decimal(data.cost_total)}
                  type={`number`}
                />
              </div>
            </TableBodyCell>
            <TableBodyCell>
              <div className={`w-32 text-right`}>
                <Form.Text
                  disabled
                  value={decimal(data.labor_total)}
                  type={`number`}
                />
              </div>
            </TableBodyCell>
            <TableBodyCell>
              <div className={`w-32 text-right`}>
                <Form.Text
                  invalid={errors.labor_factor?.length}
                  messages={errors.labor_factor}
                  name={`labor_factor`}
                  onChange={(event: any) => setLaborFactor(event.target.value)}
                  step={0.01}
                  type={`number`}
                  value={decimal(labor_factor)}
                />
              </div>
            </TableBodyCell>
            <TableBodyCell>
              <Tooltip
                label={
                  data.enabled
                    ? `Included in the Material Summary`
                    : `Excluded from the Material Summary`
                }
                arrowSize={4}
                withArrow
              >
                <input
                  className={`cursor-pointer`}
                  defaultChecked={data.enabled ? true : false}
                  key={data.enabled}
                  onChange={(event: any) =>
                    setExcludeEnabled(event.target.checked)
                  }
                  type={`checkbox`}
                />
              </Tooltip>
            </TableBodyCell>
            <TableBodyCell>
              <AssemblyActions />
            </TableBodyCell>
          </tr>
        </TableBody>
        {showParts && (
          <Parts
            items={data.parts}
            noFooter
            screen={`material`}
          />
        )}
      </AssemblyContext.Provider>
    </>
  )
}

export default Assembly
