import axios from 'axios'
import { useContext, useEffect, useState } from 'react'
import JobContext from '../contexts/Job'

import { Form } from '@/components'

import { Report } from './Report'

function downloadFile(url: any) {
  // Create a link and set the URL using `createObjectURL`
  const link: any = document.createElement(`a`)
  link.style.display = `none`
  link.href = url
  link.download = `reports.xlsx`

  // It needs to be added to the DOM so it can be clicked
  document.body.appendChild(link)
  link.click()

  // To make this work on Firefox we need to wait
  // a little while before removing it.
  setTimeout(() => {
    URL.revokeObjectURL(link.href)
    link?.parentNode.removeChild(link)
  }, 0)
}

export const Reports = () => {
  const { job }: any = useContext(JobContext)

  const [reports, setReports] = useState([])

  useEffect(() => {
    if (!job?.id) return

    axios
      .get(`/api/jobs/${job.id}/reports`)
      .then((response) => setReports(response.data))
  }, [job?.id])

  if (!job) return null
  if (!job.summary) return null

  const handleDownload = () => {
    downloadFile(`/downloads/jobs/${job.id}/reports`)
  }

  return (
    <>
      {reports.map((report: any, index: number) => (
        <Report
          data={report}
          key={index}
        />
      ))}
      <Form.Buttons>
        <Form.Button onClick={handleDownload}>Download</Form.Button>
      </Form.Buttons>
    </>
  )
}
