import { Form, Sortable } from '@/components'

import { ProposalBlock } from './ProposalBlock'

import JobContext from '../contexts/Job'
import { Fragment, useContext } from 'react'
import axios from 'axios'

import { useJob } from '@/data/Job'

export const ProposalBlocks = (props: any) => {
  const { section } = props

  const { job }: any = useContext(JobContext)
  const { reloadJob } = useJob()

  if (!job) return null

  const data = job.blocks.filter(
    (block: any) => block.job_section_id === section,
  )

  const handleSort = async (items: any) => {
    console.log(`debug.sort`, items)
    await axios.post(`/api/jobs/blocks/sort`, items)
    await reloadJob()
  }

  const handleNew = async () => {
    const payload = {
      job_id: job?.id,
      job_section_id: section,
      content: ``,
    }

    console.log(`debug.save`, payload)

    try {
      const response = await axios.post(`/api/jobs/blocks`, payload)
      console.log(`debug.save`, response.data?.data)
      await reloadJob()
    } catch (exception: any) {
      console.log(`debug.save`, exception.response.data?.errors)
      // setErrors(exception.response.data?.errors)
    }
  }

  return (
    <Fragment key={data?.length}>
      <Sortable.Root
        data={data}
        render={(item: any) => (
          <ProposalBlock
            data={item}
            key={item.uuid}
            section={section}
          />
        )}
        onSortEnd={handleSort}
      />
      <div className={`mt-4 flex space-x-4`}>
        <div className={`w-5`}></div>
        <div className={`w-full`}>
          <Form.Buttons>
            <Form.Button onClick={handleNew}>Add New</Form.Button>
          </Form.Buttons>
        </div>
      </div>
    </Fragment>
  )
}
