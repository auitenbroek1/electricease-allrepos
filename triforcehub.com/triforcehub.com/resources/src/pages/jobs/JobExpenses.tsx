import {
  PageSectionIntro,
  PageSectionItem,
  PageSectionItems,
} from '@/components'

import { LaborList } from './components/LaborList'
import { Expenses } from './components/Expenses'
import { Quotes } from './components/Quotes'

export const JobExpenses = () => {
  return (
    <PageSectionItems>
      <PageSectionItem heading={``} openByDefault={true}>
        <PageSectionIntro heading={`Additional Labor`}>
          Track labor expenses such as safety meetings, estimating time, foreman
          with a truck, etc.
        </PageSectionIntro>
        <LaborList />
      </PageSectionItem>
      <PageSectionItem heading={``} openByDefault={true}>
        <PageSectionIntro heading={`Direct Expenses`}>
          Track direct job expenses such as permits, equipment rental, using
          equipment from a shop, etc.
        </PageSectionIntro>
        <Expenses />
      </PageSectionItem>
      <PageSectionItem heading={``} openByDefault={true}>
        <PageSectionIntro heading={`Vendor Quotes`}>
          Track quotes received from different vendors.
        </PageSectionIntro>
        <Quotes />
      </PageSectionItem>
    </PageSectionItems>
  )
}
