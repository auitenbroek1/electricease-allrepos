import {
  PageSectionIntro,
  PageSectionItem,
  PageSectionItems,
} from '@/components'

import Takeoff from './components/Takeoff'

export const JobTakeoff = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Takeoff`}>
        Create phases, time and material projects, change orders, or verbal
        quotes.
      </PageSectionIntro>
      <Takeoff />
    </PageSectionItems>
  )
}
