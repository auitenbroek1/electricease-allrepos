import { PageSectionIntro, PageSectionItems } from '@/components'

import { Details } from './components/Details'

export const MemberForm = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Member Details`}>
        General information about the member.
      </PageSectionIntro>
      <Details />
    </PageSectionItems>
  )
}
