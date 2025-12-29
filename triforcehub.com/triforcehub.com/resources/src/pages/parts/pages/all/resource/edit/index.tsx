import { PageSectionIntro, PageSectionItems } from '@/components'

import { Details } from './components/Details'

export const EditPage = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Material Details`}>
        Edit material that can be added to an assembly or a job.
      </PageSectionIntro>
      <Details />
    </PageSectionItems>
  )
}
