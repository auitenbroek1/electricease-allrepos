import { useContext, useEffect, useState } from 'react'

import { AssemblyModal } from '@/pages/assemblies/components/AssemblyModal'

import NoAssemblies from './NoAssemblies'
import Assembly from './Assembly'
import Table from '@/components/Table/Table'
import TableBody from '@/components/Table/TableBody'
import TableFooter from '@/components/Table/TableFooter'
import TableHeader from '@/components/Table/TableHeader'
import TableFooterCell from '@/components/Table/TableFooterCell'
import TableHeaderCell from '@/components/Table/TableHeaderCell'

import JobsContext from './../contexts/Job'
import PhaseContext from './../contexts/Phase'
import axios from 'axios'

import { currency, number } from '@/utilities/format'

import { ToggleEnabled } from './ToggleEnabled'

export default function Assemblies(props: any) {
  const { job, setJob }: any = useContext(JobsContext)
  const phase: any = useContext(PhaseContext)

  const [isModalOpen, setIsModalOpen] = useState(false)

  const [focus, setFocus] = useState(null)

  const addAssembly = async () => {
    setIsModalOpen(true)
  }

  const setData = async (data: any) => {
    console.log(`update`, data)

    const payload = {
      job_phase_id: phase.id,
      assemblies: data.map((item: any) => item),
      labor_factor: job.labor_factor,
    }

    const response = await axios.post(`/api/jobs/assemblies`, payload)
    console.log(response)

    //

    const response2 = await axios.get(`/api/jobs/${job.id}`)
    const data2 = response2.data.data

    setJob(data2)
  }

  const handleFocus = (index: any) => {
    setFocus(index)
  }

  //

  const fingerprint = phase?.assemblies.map((assembly: any) => assembly.enabled)

  useEffect(() => {
    setFocus(null)
  }, [fingerprint])

  //

  let material_total = 0
  let labor_total = 0

  if (phase && phase.assemblies) {
    for (const assembly of phase.assemblies) {
      material_total += Number(assembly.cost_total)
      labor_total += Number(assembly.labor_total)
    }
  }

  //

  return (
    <>
      <AssemblyModal
        open={isModalOpen}
        setOpen={setIsModalOpen}
        setData={setData}
      />
      {phase?.assemblies?.length ? (
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
                  <ToggleEnabled assemblies={phase?.assemblies} />
                </div>
              </TableHeaderCell>
              <TableHeaderCell />
            </tr>
          </TableHeader>
          {phase?.assemblies.map((item: any, index: number) => (
            <Assembly
              key={`${index}${item.id}${item.enabled}`}
              index={item.id}
              focus={focus}
              data={item}
              handleFocus={handleFocus}
            />
          ))}
          {focus === null ? (
            <TableBody>
              <tr>
                <td
                  colSpan={8}
                  className="px-6 py-4 text-center"
                >
                  <button
                    disabled={focus !== null}
                    type="button"
                    onClick={addAssembly}
                    className={`relative inline-flex items-center rounded-md border border-transparent bg-blue-600 px-4 py-2 text-sm font-medium text-white shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-600 focus:ring-offset-2`}
                  >
                    + Add More Assemblies
                  </button>
                </td>
              </tr>
            </TableBody>
          ) : null}
          <TableFooter>
            <tr>
              <TableFooterCell />
              <TableFooterCell>Total</TableFooterCell>
              <TableFooterCell />
              <TableFooterCell />
              <TableFooterCell />
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
        </Table>
      ) : (
        <>
          <NoAssemblies />
          <div className="mt-4 text-center">
            <button
              type="button"
              onClick={addAssembly}
              className="relative inline-flex items-center rounded-md border border-transparent bg-blue-600 px-4 py-2 text-sm font-medium text-white shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-600 focus:ring-offset-2"
            >
              Add Assemblies
            </button>
          </div>
        </>
      )}
    </>
  )
}
