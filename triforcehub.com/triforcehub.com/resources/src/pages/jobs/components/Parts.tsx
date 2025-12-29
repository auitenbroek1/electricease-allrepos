import { useContext, useState } from 'react'

import PartsModal from './PartsModal'

import Table from '@/components/Table/Table'
import TableHeader from '@/components/Table/TableHeader'
import TableHeaderCell from '@/components/Table/TableHeaderCell'
import TableBody from '@/components/Table/TableBody'
import TableFooter from '@/components/Table/TableFooter'
import TableFooterCell from '@/components/Table/TableFooterCell'

import Part from './Part'
import NoParts from './NoParts'

import JobsContext from './../contexts/Job'
import PhaseContext from './../contexts/Phase'
import AssemblyContext from './../contexts/Assembly'
import axios from 'axios'

import { currency, number } from '@/utilities/format'

import { ToggleEnabled } from './ToggleEnabled'

export default function Parts(props: any) {
  const { items = [], screen = null, noFooter } = props

  //

  const { job, setJob }: any = useContext(JobsContext)
  const phase: any = useContext(PhaseContext)
  const assembly: any = useContext(AssemblyContext)

  const [isModalOpen, setIsModalOpen] = useState(false)

  const addParts = async () => {
    setIsModalOpen(true)
  }

  const setData = async (data: any) => {
    console.log(`update`, data)

    const payload = {
      phase: phase.id,
      assembly: assembly?.id,
      parts: data,
      labor_factor: job.labor_factor,
      enabled: assembly?.enabled,
    }

    const response = await axios.post(`/api/jobs/parts`, payload)
    console.log(response)

    //

    const response2 = await axios.get(`/api/jobs/${job.id}`)
    const data2 = response2.data.data

    setJob(data2)
  }

  //

  let material_total = 0
  let labor_total = 0

  for (const item of items) {
    material_total += Number(item.cost_total)
    labor_total += Number(item.labor_total)
  }

  //

  const header = () => {
    return (
      <TableHeader>
        <tr>
          <TableHeaderCell />
          <TableHeaderCell>Name</TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>Material</div>
          </TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>Labor</div>
          </TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>Qty / Adj Qty</div>
          </TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>Material Total</div>
          </TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>Labor Total</div>
          </TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>Labor Factor</div>
          </TableHeaderCell>
          <TableHeaderCell>
            <div className={`text-center`}>
              <ToggleEnabled parts={phase?.parts} />
            </div>
          </TableHeaderCell>
          <TableHeaderCell />
        </tr>
      </TableHeader>
    )
  }

  const body = () => {
    return (
      <TableBody>
        {items.map((item: any, index: any) => (
          <Part
            data={item}
            key={`${index}${item.id}${item.enabled}`}
            screen={screen}
          />
        ))}
        <tr>
          <td
            colSpan={9}
            className="px-4 py-4 text-center"
          >
            <button
              className={`
                relative
                inline-flex
                items-center
                rounded-md
                border
                border-transparent
                bg-blue-600
                px-4
                py-2
                text-sm
                font-medium
                text-white
                shadow-sm
                focus:outline-none
                focus:ring-2
                focus:ring-blue-600
                focus:ring-offset-2
              `}
              onClick={addParts}
              type="button"
            >
              + Add More Material
            </button>
          </td>
        </tr>
      </TableBody>
    )
  }

  const footer = () => {
    return (
      <TableFooter>
        <tr>
          <TableFooterCell />
          <TableFooterCell>Total</TableFooterCell>
          <TableFooterCell></TableFooterCell>
          <TableFooterCell></TableFooterCell>
          <TableFooterCell></TableFooterCell>
          <TableFooterCell>
            <div className={`px-2 text-right`}>
              {currency(material_total.toFixed(6))}
            </div>
          </TableFooterCell>
          <TableFooterCell>
            <div className={`px-2 text-right`}>
              {number(labor_total.toFixed(6))}
            </div>
          </TableFooterCell>
          <TableFooterCell />
          <TableFooterCell />
          <TableFooterCell />
        </tr>
      </TableFooter>
    )
  }

  return (
    <>
      <PartsModal
        open={isModalOpen}
        setOpen={setIsModalOpen}
        setData={setData}
      />

      {noFooter ? (
        body()
      ) : phase?.parts?.length ? (
        <Table>
          <colgroup>
            <col width={1} />
            <col width={`*`} />
            <col width={1} />
            <col width={1} />
            <col width={1} />
            <col width={1} />
            <col width={1} />
            <col width={1} />
            <col width={1} />
            <col width={1} />
          </colgroup>
          {header()}
          {body()}
          {footer()}
        </Table>
      ) : (
        <>
          <NoParts />
          <div className="mt-4 text-center">
            <button
              type="button"
              onClick={addParts}
              className="relative inline-flex items-center rounded-md border border-transparent bg-blue-600 px-4 py-2 text-sm font-medium text-white shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-600 focus:ring-offset-2"
            >
              Add Material
            </button>
          </div>
        </>
      )}
    </>
  )
}
