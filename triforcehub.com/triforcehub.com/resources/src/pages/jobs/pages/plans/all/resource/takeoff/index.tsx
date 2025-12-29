import { ErrorBoundary } from '@/components/ErrorBoundary'
import { PageSectionIntro, PageSectionItems } from '@/components'
import OstPdfRenderer from './components/OstPdfRenderer'
import { useParams } from 'react-router-dom'
import { useContext, useEffect, useState } from 'react'

import JobContext from '@/pages/jobs/contexts/Job'
import axios from 'axios'
import { sleep } from '@/helpers/sleep'
import toast from 'react-hot-toast'

export const TakeoffPage = (props: any) => {
  const { job }: any = useContext(JobContext)

  const params = useParams()
  const { id } = params

  const [plan, setPlan] = useState<any>()
  const [annotations, setAnnotations] = useState<any>()

  //

  useEffect(() => {
    if (!job?.id) return

    console.log(`debug.ost.plan`, { job: job.id, plan: id })

    const fetchData = async () => {
      try {
        const response = await axios.get(`/api/jobs/${job.id}/plans/${id}`)
        const data = response.data.data
        console.log(`debug.ost.plan`, data)
        setPlan(data)
      } catch (exception) {
        console.log(`debug.ost.plan`, exception)
      }
    }

    fetchData()
  }, [job?.id, id])

  useEffect(() => {
    if (!job?.id) return
    if (!plan?.id) return

    console.log(`debug.plans.takeoff.mount`, `mount`, {
      job: job.id,
      plan: plan.id,
    })

    let loading: any = null

    async function* fetchData() {
      let page: number | null = 1

      while (page) {
        console.log(`debug.ost.plan.annotations`, { page })

        const url = `/api/jobs/${job.id}/plans/${id}/annotations?page=${page}&size=2000`
        const response: any = await axios.get(url)
        const data = response.data.data
        const meta = response.data.meta

        const current_page = meta.current_page
        const last_page = meta.last_page

        const percentage = Math.floor((current_page / last_page) * 100)

        toast.loading(`Loading annotations: ${percentage}%`, {
          id: loading,
        })

        if (meta.current_page === meta.last_page) {
          page = null
          toast.success(`Done!`, { id: loading })
        } else {
          page = current_page + 1
        }

        await sleep(500)

        yield data
      }
    }

    const fetchAllData = async () => {
      try {
        loading = toast.loading(`Loading...`)
        const data = []
        for await (const items of fetchData()) {
          data.push(...items)
        }
        setAnnotations(data)
      } catch (exception) {
        console.log(`debug.ost.plan.annotations`, exception)
        toast.error(`Oops!`, { id: loading })
      }
    }

    fetchAllData()

    return () => {
      console.log(`debug.plans.takeoff.mount`, `umount`)
    }
  }, [job?.id, plan?.id])

  //

  console.log(`debug.ost`, { params, job })

  if (!plan) return null
  if (!annotations) return null

  const clearCache = () => {
    const allKeys = Object.keys(localStorage)
    const keysToRemove = allKeys.filter((key: string) => {
      if (key.startsWith(`default-toolData`)) return true
      if (key.startsWith(`init_timestamp`)) return true
      if (key.startsWith(`persist:search-default`)) return true
      if (key.startsWith(`persist:viewer-default`)) return true
    })
    console.log(`debug`, `remove`, `pdftron cache`, keysToRemove)
    keysToRemove.forEach((key: string) => {
      localStorage.removeItem(key)
    })
    window.location.reload()
  }

  return (
    <PageSectionItems>
      <PageSectionIntro heading={`Digital Takeoff`}>
        <div className="flex items-center justify-between">
          <div>Upload and manage bid plans.</div>
          <div>
            <button
              className="text-brand-gradient-light hover:underline"
              onClick={clearCache}
              type="button"
            >
              Refresh
            </button>
          </div>
        </div>
      </PageSectionIntro>
      <ErrorBoundary>
        <OstPdfRenderer
          plan={plan}
          annotations={annotations}
        />
      </ErrorBoundary>
    </PageSectionItems>
  )
}
