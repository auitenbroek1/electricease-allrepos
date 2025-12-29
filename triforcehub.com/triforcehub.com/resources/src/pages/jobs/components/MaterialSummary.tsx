import { useContext, useEffect, useMemo, useState } from 'react'

import {
  Form,
  Table,
  TableHeader,
  TableHeaderRow,
  TableHeaderCell,
  TableBody,
  TableBodyRow,
  TableBodyCell,
} from '@/components'

import JobContext from '../contexts/Job'
import { useAuth } from '@/contexts/AuthContext'
import axios from 'axios'
import chunk from 'lodash/chunk'
import debounce from 'lodash/debounce'
import { decimal } from '@/utilities/format'
import toast from 'react-hot-toast'

import { ArrowPathIcon, ArrowRightIcon } from '@/components/Icons'

export const MaterialSummary = () => {
  const { job, setJob }: any = useContext(JobContext)
  const [rawData, setRawData]: any = useState(null)
  const { user } = useAuth()

  if (!job) return null
  if (!job.phases) return null

  const data: any = {}

  const fetchData = async () => {
    const responseSummary = await axios.get(`/api/jobs/${job.id}/summary`)
    await setRawData(responseSummary?.data)
    return
    const phases = responseSummary?.data?.data?.phases

    for (const phase of phases) {
      for (const assembly of phase.assemblies) {
        for (const part of assembly.parts) {
          const item = format(part, assembly.quantity)
          append(item)
        }
      }
      for (const part of phase.parts) {
        const item = format(part, 1)
        append(item)
      }
    }
  }

  useEffect(() => {
    if (job) {
      fetchData()
    }
    //
  }, [job])

  const format = (part: any, multiplier: any) => {
    const item: any = {}

    // TODO: use uuid instead
    item.key = part.reference.id
    item.name = part.reference.name
    item.description = part.reference.description
    item.cost = part.cost
    item.quantity = part.quantity * multiplier
    item.cost_total = part.cost * item.quantity
    item.upcs = part.reference.upcs.map((upc: any) => upc.name)

    // console.log(
    //   `debug.material`,
    //   item.name,
    //   item.cost,
    //   item.quantity,
    //   item.cost_total,
    //   item.upcs,
    // )

    return item
  }

  const append = (item: any) => {
    if (data[item.key]) {
      data[item.key].quantity += item.quantity
      data[item.key].cost_total += item.cost_total
      data[item.key].cost = data[item.key].cost_total / data[item.key].quantity
    } else {
      data[item.key] = item
    }
    setRawData(data)
  }

  // console.log(`debug.data`, data)

  //

  const [loading, setLoading] = useState<any>(null)
  const [loadingAll, setLoadingAll] = useState<any>(null)

  //

  const [prices, setPrices] = useState<any>({})

  const updatePrices = async (distributor: any, items: any) => {
    return new Promise(async (resolve, reject) => {
      console.log(`debug.price.updatePrices`, distributor, items)
      //
      if (items.length === 1) {
        setPrices((previous: any) => {
          const updated = { ...previous }
          if (!updated[distributor.id]) {
            updated[distributor.id] = {}
          }
          updated[distributor.id][items[0].key] = null
          return updated
        })
        setLoading({ distributor, key: items[0].key })
      }
      //
      const payload = Object.values(items).map((item: any) => {
        return {
          key: item.key,
          quantity: item.quantity,
          upcs: item.upcs,
        }
      })
      try {
        const response = await axios.post(
          `/api/distributors/${distributor.id}/prices`,
          payload,
        )
        const prices = response.data
        console.log(`debug.price.updatePrices`, prices)
        setPrices((previous: any) => {
          const updated = { ...previous }
          if (!updated[distributor.id]) {
            updated[distributor.id] = {}
          }
          for (const price of prices) {
            updated[distributor.id][price.key] = price.price
          }
          console.log(`debug.price`, updated)
          return updated
        })
        setLoading(null)
        return resolve(prices)
      } catch (error) {
        setLoading(null)
        return reject(`oops`)
      }
    })
  }

  const updateAllPrices = async (distributor: any) => {
    console.log(`debug.price.updateAllPrices`, distributor)
    setPrices((previous: any) => {
      const updated = { ...previous }
      updated[distributor.id] = {}
      return updated
    })
    setLoadingAll({ distributor })
    const items = Object.values(rawData).map((item: any) => item)
    const batches = chunk(items, 6)
    // option 1
    for (const batch of batches) {
      console.log(`debug.price.batch`, batch)
      try {
        const response = await updatePrices(distributor, batch)
        console.log(`debug.price.updateAllPrices`, response)
      } catch (error) {
        console.log(`debug.price.updateAllPrices`, error)
      }
      console.log(`debug.price.done`)
    }
    setLoadingAll(null)
    return
    // option 2
    const promises = batches.map((batch) => updatePrices(distributor, batch))
    try {
      const response = await Promise.allSettled(promises)
      console.log(`debug.price.updateAllPrices`, response)
    } catch (error) {
      console.log(`debug.price.updateAllPrices`, error)
    }
  }

  //

  const [savingPrices, setSavingPrices] = useState(false)
  const [errors, setErrors] = useState<any>([])

  const savePrices = async (items: any, input: any, distributor: any) => {
    console.log(`debug.price.save`, { items, input, distributor })
    let payload = []
    if (items && distributor !== null) {
      payload = items.map((item: any) => {
        return {
          key: item.key,
          price: prices?.[distributor]?.[item.key] ?? 0,
        }
      })
    }
    if (input) {
      payload = [input]
    }
    console.log(`debug.price.save`, payload)
    // return
    //
    try {
      setSavingPrices(true)
      const response = await axios.post(`/api/jobs/${job.id}/prices`, payload)
      console.log(response?.data)
      //
      const response2 = await axios.get(`/api/jobs/${job.id}`)
      const data2 = response2.data.data
      setJob(data2)
      setErrors([])
      toast.success(`Saved!`)
      await fetchData()
    } catch (exception: any) {
      setErrors(exception.response?.data?.errors ?? [])
    } finally {
      setSavingPrices(false)
    }
  }

  const savePricesDebounced = useMemo(
    () => debounce((item) => savePrices(null, item, null), 1000),
    [],
  )

  //

  const fingerprint = Date.now()
  const distributors = user?.member?.distributors.filter(
    (distributor: any) => distributor.enabled,
  )

  // give everyone national pricing for now
  if (distributors?.length === 0 || user?.member?.id === 1) {
    distributors.push({
      id: 0,
      name: `National`,
      enabled: true,
    })
  }

  console.log(`debug.distributors`, user?.member?.distributors)
  console.log(`debug.distributors`, distributors)

  const priceToShow = (distributor: any, key: any) => {
    if (typeof prices[distributor]?.[key] === `undefined`) {
      return null
    }
    if (prices[distributor]?.[key] === null) {
      return null
    }
    return decimal(prices[distributor][key])
  }

  //

  return (
    <div className={`${savingPrices ? `opacity-50 pointer-events-none` : ``}`}>
      <Table>
        <colgroup>
          <col width={1} />
          <col width={`*`} />
          {distributors.map((distributor: any) => (
            <col
              key={distributor.id}
              width={1}
            />
          ))}
          <col width={1} />
          <col width={1} />
          <col width={1} />
        </colgroup>
        <TableHeader>
          <TableHeaderRow>
            <TableHeaderCell>Name</TableHeaderCell>
            <TableHeaderCell>{/* Description */}</TableHeaderCell>
            {distributors.map((distributor: any) => (
              <TableHeaderCell key={distributor.id}>
                <div className={`flex items-center justify-start space-x-2`}>
                  <div className={`w-32 text-center`}>{distributor.name}</div>
                  <div
                    className={`h-5 w-5 ${
                      loadingAll?.distributor?.id === distributor.id
                        ? `pointer-events-none animate-spin`
                        : null
                    }`}
                  >
                    <button
                      onClick={() => updateAllPrices(distributor)}
                      type={`button`}
                    >
                      <ArrowPathIcon />
                    </button>
                  </div>
                  <div className={`h-5 w-5`}>
                    <button
                      onClick={() =>
                        savePrices(Object.values(rawData), null, distributor.id)
                      }
                      type={`button`}
                    >
                      <div className={`h-5 w-5`}>
                        <ArrowRightIcon />
                      </div>
                    </button>
                  </div>
                </div>
              </TableHeaderCell>
            ))}
            <TableHeaderCell>
              <div className={`w-32 text-center`}>Cost</div>
            </TableHeaderCell>
            <TableHeaderCell>
              <div className={`w-32 text-center`}>Quantity</div>
            </TableHeaderCell>
            <TableHeaderCell>
              <div className={`w-32 text-center`}>Cost Total</div>
            </TableHeaderCell>
          </TableHeaderRow>
        </TableHeader>
        <TableBody>
          {rawData &&
            Object.values(rawData).map((item: any) => (
              <TableBodyRow key={item.key}>
                <TableBodyCell>{item.name}</TableBodyCell>
                <TableBodyCell>{/* {item.description} */}</TableBodyCell>
                {distributors.map((distributor: any) => (
                  <TableBodyCell key={distributor.id}>
                    <div
                      className={`flex items-center justify-start space-x-2`}
                    >
                      <div className={`w-32 text-right`}>
                        <Form.Input
                          defaultValue={priceToShow(distributor.id, item.key)}
                          disabled={true}
                          prefix={`$`}
                        />
                      </div>
                      <div
                        className={`h-5 w-5 ${
                          (loading?.distributor === distributor &&
                            loading?.key === item.key) ||
                          loadingAll?.distributor?.id === distributor.id
                            ? `pointer-events-none animate-spin`
                            : null
                        }`}
                      >
                        {item.upcs.length ? (
                          <button
                            onClick={() => updatePrices(distributor, [item])}
                            type={`button`}
                          >
                            <ArrowPathIcon />
                          </button>
                        ) : null}
                      </div>
                      <div className={`h-5 w-5`}>
                        <button
                          onClick={() =>
                            savePrices([item], null, distributor.id)
                          }
                          type={`button`}
                        >
                          <div className={`h-5 w-5`}>
                            <ArrowRightIcon />
                          </div>
                        </button>
                      </div>
                    </div>
                  </TableBodyCell>
                ))}
                <TableBodyCell key={fingerprint}>
                  <div className={`w-32 text-right`}>
                    <Form.Input
                      defaultValue={savingPrices ? null : decimal(item.cost)}
                      onChange={(event: any) =>
                        savePricesDebounced({
                          key: item.key,
                          price: event.target.value,
                        })
                      }
                      prefix={`$`}
                      step={`0.01`}
                      type={`number`}
                    />
                  </div>
                </TableBodyCell>
                <TableBodyCell>
                  <div className={`w-32 text-right`}>
                    <Form.Input
                      disabled={true}
                      value={decimal(item.quantity)}
                    />
                  </div>
                </TableBodyCell>
                <TableBodyCell>
                  <div className={`w-32 text-right`}>
                    <Form.Input
                      disabled={true}
                      prefix={`$`}
                      value={decimal(item.cost_total)}
                    />
                  </div>
                </TableBodyCell>
              </TableBodyRow>
            ))}
        </TableBody>
      </Table>
    </div>
  )
}
