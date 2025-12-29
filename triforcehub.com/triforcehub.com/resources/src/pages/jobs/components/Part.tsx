import { useCallback, useContext, useEffect, useState } from 'react'

import FormNumber from '@/components/Form/FormNumber'
import TableBodyCell from '@/components/Table/TableBodyCell'

import JobsContext from './../contexts/Job'
import PhaseContext from './../contexts/Phase'
import AssemblyContext from '../contexts/Assembly'

import PartContext from '../contexts/Part'
import PartActions from './PartActions'

import useDebounce from '@/hooks/debounce'
import Cost from '@/components/Form/Cost'
import Labor from '@/components/Form/Labor'

import axios from 'axios'
import { Form } from '@/components'
import { decimal } from '@/utilities/format'
import { Tooltip } from '@mantine/core'
import toast from 'react-hot-toast'

const Part = (props: any) => {
  const { data, screen } = props

  const { job, setJob }: any = useContext(JobsContext)
  const phase: any = useContext(PhaseContext)
  const assembly: any = useContext(AssemblyContext)

  //

  const [mounted, setMounted] = useState(false)

  const [cost, setCost] = useState(data.cost)
  const cost2 = useDebounce(cost)

  const [labor, setLaborHours] = useState(data.labor)
  const labor2 = useDebounce(labor)

  const [excludeEnabled, setExcludeEnabled] = useState<any>(data.enabled)

  const [quantity, setQuantity] = useState(data.quantity)
  const quantity2 = useDebounce(quantity)

  const [labor_factor, setLaborFactor] = useState(data.labor_factor)
  const labor_factor2 = useDebounce(labor_factor)

  // console.log(`assembly data`, data)

  //

  const [errors, setErrors] = useState<any>([])

  useEffect(() => {
    if (!mounted) return

    const sync = async () => {
      console.log(`sync`, `here!`)

      const payload = {
        cost: cost2,
        labor: labor2,
        quantity: quantity2,
        labor_factor: labor_factor2,
        enabled: excludeEnabled,
      }

      console.log(`sync`, payload)

      try {
        const response1 = await axios.patch(
          `/api/jobs/parts/${data.id}`,
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
  }, [cost2, labor2, excludeEnabled, quantity2, labor_factor2])

  useEffect(() => {
    setMounted(true)
  }, [])

  const withAssemblyQuantity = (input: any) => {
    // console.log(`withAssemblyQuantity`, input, assembly)
    let output = Number(input)
    if (assembly?.quantity) {
      output = output * assembly.quantity
    }
    return decimal(output)
  }

  const is_wrapped = assembly?.uuid ? true : false

  return (
    <PartContext.Provider value={data}>
      <tr className={data.enabled === 0 ? `bg-yellow-50` : ``}>
        <TableBodyCell>
          <div className={`w-5`}></div>
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
            <div className={`text-sm text-black`}>{data.reference?.name}</div>
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
              invalid={errors.cost?.length}
              messages={errors.cost}
              name={`cost`}
              onChange={(event: any) => setCost(event.target.value)}
              prefix={`$`}
              type={`number`}
              value={decimal(cost)}
            />
          </div>
        </TableBodyCell>
        <TableBodyCell>
          <div className={`w-32 text-right`}>
            <Form.Text
              invalid={errors.labor?.length}
              messages={errors.labor}
              name={`labor`}
              onChange={(event: any) => setLaborHours(event.target.value)}
              type={`number`}
              value={decimal(labor)}
            />
          </div>
        </TableBodyCell>
        <TableBodyCell>
          <div className={`w-32 text-right flex gap-2`}>
            {is_wrapped === false && (
              <Form.Text
                disabled={true}
                type={`number`}
                value={decimal(data.quantity_digital)}
              />
            )}
            <Form.Text
              invalid={errors.quantity?.length}
              messages={errors.quantity}
              name={`quantity`}
              onChange={(event: any) => setQuantity(event.target.value)}
              type={`number`}
              value={decimal(quantity)}
            />
          </div>
        </TableBodyCell>
        <TableBodyCell>
          <div className={`w-32 text-right`}>
            <Form.Text
              disabled={true}
              prefix={`$`}
              type={`number`}
              value={withAssemblyQuantity(data.cost_total)}
            />
          </div>
        </TableBodyCell>
        <TableBodyCell>
          <div className={`w-32 text-right`}>
            <Form.Text
              disabled={true}
              type={`number`}
              value={withAssemblyQuantity(data.labor_total)}
            />
          </div>
        </TableBodyCell>
        <TableBodyCell>
          <div className={`w-32 text-right`}>
            {assembly?.uuid ? (
              <Form.Text
                disabled={true}
                type={`number`}
                value={decimal(assembly?.labor_factor)}
              />
            ) : (
              <Form.Text
                invalid={errors.labor_factor?.length}
                messages={errors.labor_factor}
                name={`labor_factor`}
                onChange={(event: any) => setLaborFactor(event.target.value)}
                step={0.01}
                type={`number`}
                value={decimal(labor_factor)}
              />
            )}
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
              className={
                screen === `material` ? `pointer-events-none` : `cursor-pointer`
              }
              defaultChecked={data.enabled ? true : false}
              disabled={screen === `material` ? true : false}
              key={data.enabled}
              onChange={(event: any) => setExcludeEnabled(event.target.checked)}
              type={`checkbox`}
            />
          </Tooltip>
        </TableBodyCell>
        <TableBodyCell>
          <PartActions />
        </TableBodyCell>
      </tr>
    </PartContext.Provider>
  )
}

export default Part
