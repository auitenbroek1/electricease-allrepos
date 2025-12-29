import { PageSectionIntro, PageSectionItems } from '@/components'

import { Details } from '../../resource/edit/components/Details'

export const CreatePage = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Material Details`}>
        Create material that can be added to an assembly or a job.
      </PageSectionIntro>
      <Details />
    </PageSectionItems>
  )
}
