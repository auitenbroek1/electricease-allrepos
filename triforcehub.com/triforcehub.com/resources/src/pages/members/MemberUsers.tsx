import { PageSectionIntro, PageSectionItems } from '@/components'

import { Users } from './components/Users'

export const MemberUsers = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Users`}>
        Create new users, or edit existing users for this member account.
      </PageSectionIntro>
      <Users />
    </PageSectionItems>
  )
}
