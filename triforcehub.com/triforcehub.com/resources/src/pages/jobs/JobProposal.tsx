import {
  PageSectionIntro,
  PageSectionItem,
  PageSectionItems,
} from '@/components'

import { ProposalBlocks } from './components/ProposalBlocks'

export const JobProposal = () => {
  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Build Proposal`}>
        Add job notes, scope work, exclusions and contract terms and conditions.
      </PageSectionIntro>
      <PageSectionItem heading="Job Notes" openByDefault={true}>
        <div className={`text-sm text-gray-500`}>
          Add job notes for your crew – customer will not see these notes.
        </div>
        <hr className={`my-4`} />
        <ProposalBlocks section={1} />
      </PageSectionItem>
      <PageSectionItem heading="Scope of Work" openByDefault={true}>
        <div className={`text-sm text-gray-500`}>
          Add work details for your customer to review. Could include scope of
          work, inclusion and exclusions.
        </div>
        <hr className={`my-4`} />
        <ProposalBlocks section={2} />
      </PageSectionItem>
      <PageSectionItem heading="Terms and Conditions" openByDefault={true}>
        <div className={`text-sm text-gray-500`}>
          Add terms and conditions to clarify any legal details, upfront
          payments, etc.
        </div>
        <hr className={`my-4`} />
        <ProposalBlocks section={3} />
      </PageSectionItem>
    </PageSectionItems>
  )
}
