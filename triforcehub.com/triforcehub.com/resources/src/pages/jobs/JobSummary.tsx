import { useContext } from 'react'

import { Card } from '@/components'

import {
  PageSectionIntro,
  PageSectionItem,
  PageSectionItems,
} from '@/components'

import { MaterialSummary } from './components/MaterialSummary'
import { MaterialSummaryActions } from './components/MaterialSummaryActions'
import { SummaryMaterial } from './components/SummaryMaterial'
import { SummaryCrews } from './components/SummaryCrews'
import { SummaryLabors } from './components/SummaryLabors'
import { SummaryExpenses } from './components/SummaryExpenses'
import { SummaryQuotes } from './components/SummaryQuotes'
import { SummaryAdjustments } from './components/SummaryAdjustments'
import { SummarySettings } from './components/SummarySettings'

import JobContext from './contexts/Job'

export const JobSummary = () => {
  const { job }: any = useContext(JobContext)

  if (!job) return null
  if (!job.summary) return null

  console.log(`debug.render`)

  return (
    <>
      <PageSectionItems>
        <PageSectionIntro heading={`Summary`}>
          Detailed breakdown that summarizes all costs that were entered in the
          takeoff and additional expenses sections.
        </PageSectionIntro>
        <PageSectionItem
          heading="Material Summary"
          openByDefault={true}
          actions={<MaterialSummaryActions />}
        >
          <MaterialSummary />
        </PageSectionItem>
        <PageSectionItem
          heading="Bid Summary"
          openByDefault={true}
        >
          <div className={`space-y-8`}>
            <Card.Group>
              <Card.Header title={`Material`} />
              <hr />
              <SummaryMaterial data={job.summary.materials} />
            </Card.Group>

            <Card.Group>
              <Card.Header title={`Labor Takeoff`} />
              <hr />
              <SummaryCrews data={job.summary.crews} />
            </Card.Group>

            <Card.Group>
              <Card.Header title={`Additional Labor`} />
              <hr />
              <SummaryLabors data={job.summary.labors} />
            </Card.Group>

            <Card.Group>
              <Card.Header title={`Direct Expenses`} />
              <hr />
              <SummaryExpenses data={job.summary.expenses} />
            </Card.Group>

            <Card.Group>
              <Card.Header title={`Vendor Quotes`} />
              <hr />
              <SummaryQuotes data={job.summary.quotes} />
            </Card.Group>

            <Card.Group>
              <Card.Header title={`Adjustments`} />
              <hr />
              <SummaryAdjustments data={job.summary.adjustments} />
              <SummarySettings />
            </Card.Group>
          </div>
        </PageSectionItem>
      </PageSectionItems>
    </>
  )
}
