import { PageSectionIntro, PageSectionItems } from '@/components'

import { JumpStart } from './components/JumpStart'

export const MemberJumpStart = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`JumpStart`}>
        JumpStart bids in this member&apos;s account.
      </PageSectionIntro>
      <JumpStart />
    </PageSectionItems>
  )
}
