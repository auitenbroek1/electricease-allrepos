import { PageSectionIntro, PageSectionItems } from '@/components'

import { Details } from './components/Details'

export const AssemblyForm = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Assembly Details`}>
        Edit the name, description, or material in an assembly.
      </PageSectionIntro>
      <Details />
    </PageSectionItems>
  )
}
