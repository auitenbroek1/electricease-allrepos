import { useContext } from 'react'

import {
  PageSectionIntro,
  PageSectionItem,
  PageSectionItems,
} from '@/components'

import JobContext from './contexts/Job'

import Client from './components/Client'
import General from './components/General'

export const JobForm = () => {
  const { job }: any = useContext(JobContext)

  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Job Details`}>
        General information about customer, job, or project.
      </PageSectionIntro>
      <PageSectionItem heading="Job Information" openByDefault={true}>
        <General />
      </PageSectionItem>
      {job?.type?.id === 1 && (
        <PageSectionItem heading="Customer Information" openByDefault={true}>
          <Client />
        </PageSectionItem>
      )}
    </PageSectionItems>
  )
}
