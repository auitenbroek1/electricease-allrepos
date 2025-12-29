import { PageSectionIntro, PageSectionItems } from '@/components'

import Proposals from './components/Proposals'

export const JobSend = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Send Proposal`}>
        Send proposals to customer(s).
      </PageSectionIntro>
      <Proposals />
    </PageSectionItems>
  )
}
