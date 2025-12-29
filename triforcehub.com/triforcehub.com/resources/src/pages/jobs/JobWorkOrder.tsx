import { PageSectionIntro, PageSectionItems } from '@/components'

import WorkOrders from './components/WorkOrders'

export const JobWorkOrder = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Send Work Order`}>
        Send work orders to your crew(s).
      </PageSectionIntro>
      <WorkOrders />
    </PageSectionItems>
  )
}
