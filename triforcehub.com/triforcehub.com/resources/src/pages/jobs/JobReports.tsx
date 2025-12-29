import { ErrorBoundary } from '@/components/ErrorBoundary'

import { PageSectionIntro, PageSectionItems } from '@/components'
import { Reports } from './components/Reports'

export const JobReports = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Reports`}>
        Pull reports for specific details about this job.
      </PageSectionIntro>
      <ErrorBoundary>
        <Reports />
      </ErrorBoundary>
    </PageSectionItems>
  )
}
