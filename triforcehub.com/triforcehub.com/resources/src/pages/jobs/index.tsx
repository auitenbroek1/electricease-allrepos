import { useContext, useEffect, useState } from 'react'

import axios from 'axios'

import {
  Navigate,
  Route,
  Routes,
  useHref,
  resolvePath,
  useLocation,
  Outlet,
} from 'react-router-dom'

import { HeaderSecondaryContext } from '@/layouts/default/contexts/HeaderSecondary'
import { HeaderTertiaryContext } from '@/layouts/default/contexts/HeaderTertiary'

import {
  ArrowLongLeftIcon,
  CalculatorIcon,
  ClipboardDocumentCheckIcon,
  CurrencyDollarIcon,
  DocumentChartBarIcon,
  DocumentIcon,
  InformationCircleIcon,
  Squares2x2Icon,
} from '@/components/Icons'

import JobContext from './contexts/Job'

import { JobCollection } from './JobCollection'
import { JobDocuments } from './JobDocuments'
import { JobExpenses } from './JobExpenses'
import { JobForm } from './JobForm'
import { JobPlans } from './pages/plans'
import { JobProposal } from './JobProposal'
import { JobReports } from './JobReports'
import { JobSend } from './JobSend'
import { JobSummary } from './JobSummary'
import { JobTakeoff } from './JobTakeoff'
import { JobWorkOrder } from './JobWorkOrder'
import { PageNotFound } from '../errors'
import { useAuth } from '@/contexts/AuthContext'

export const Jobs = () => {
  const { user } = useAuth()

  const href = useHref(``)
  const location = useLocation()

  const parts = location.pathname.split(`/`).filter((item) => item.length)
  const controller = parts[2]
  let action = parts[parts.length > 4 ? 4 : 3]
  const id = parts.length > 4 ? parts[3] : null

  if (action === `plans` && parts.pop() === `takeoff`) {
    action = `digital-takeoff`
  }

  console.log(`debug.routes`, { href, parts, controller, action, id })

  const segments: any = []
  const items2: any = []

  if (controller && action) {
    const depth = id ? `../..` : `..`
    const controllers: any = {
      all: `All Bids`,
    }

    segments.push({
      icon: ArrowLongLeftIcon,
      title: controllers[controller],
      to: resolvePath(depth, location.pathname),
    })

    const prefix = `jobs/all/${id}`

    items2.push({
      disabled: id ? false : true,
      icon: InformationCircleIcon,
      name: `Job Details`,
      to: `${prefix}/edit`,
    })

    //

    console.log(`debug.features`, user?.member)
    let is_plans_disabled = id ? false : true
    if (user?.member?.feature_digital_takeoff_enabled === false) {
      is_plans_disabled = true
    }
    console.log(`debug.features`, { is_plans_disabled })

    items2.push({
      disabled: is_plans_disabled,
      icon: DocumentIcon,
      name: `Plans`,
      to: `${prefix}/plans`,
    })

    //

    items2.push({
      disabled: id ? false : true,
      icon: Squares2x2Icon,
      name: `Takeoff`,
      to: `${prefix}/takeoff`,
    })

    items2.push({
      disabled: id ? false : true,
      icon: CurrencyDollarIcon,
      name: `Additional Expenses`,
      to: `${prefix}/expenses`,
    })

    items2.push({
      disabled: id ? false : true,
      icon: CalculatorIcon,
      name: `Summary`,
      to: `${prefix}/summary`,
    })

    items2.push({
      disabled: id ? false : true,
      icon: DocumentIcon,
      name: `Build Proposal`,
      to: `${prefix}/proposal`,
    })

    items2.push({
      disabled: id ? false : true,
      icon: ClipboardDocumentCheckIcon,
      name: `Send Proposal`,
      to: `${prefix}/send`,
    })

    items2.push({
      disabled: id ? false : true,
      icon: ClipboardDocumentCheckIcon,
      name: `Send Work Order`,
      to: `${prefix}/work-order`,
    })

    items2.push({
      disabled: id ? false : true,
      icon: DocumentChartBarIcon,
      name: `Reports`,
      to: `${prefix}/reports`,
    })
  } else {
    // segments.push({
    //   title: `All Jobs`,
    //   to: resolvePath(`all`, href),
    // })
  }

  const tutorials: any = {
    create: [
      {
        color: `red`,
        media: `red/02.mp4`,
        name: `JumpStart 2`,
      },
      {
        color: `yellow`,
        media: `yellow/02.mp4`,
        name: `Template 2`,
      },
      {
        media: `green/Job+Details+Tutorial+-+1.mp4`,
        name: `Job Details Tutorial`,
      },
    ],
    edit: [
      {
        color: `red`,
        media: `red/02.mp4`,
        name: `JumpStart 2`,
      },
      {
        color: `yellow`,
        media: `yellow/02.mp4`,
        name: `Template 2`,
      },
      {
        media: `green/Job+Details+Tutorial+-+1.mp4`,
        name: `Job Details Tutorial`,
      },
    ],
    plans: [
      {
        color: `red`,
        media: `red/03.mp4`,
        name: `JumpStart 3`,
      },
      {
        media: `green/Digital+Takeoff+Tutorial.mp4`,
        name: `Digital Takeoff Tutorial`,
      },
    ],
    'digital-takeoff': [
      {
        color: `red`,
        media: `red/04.mp4`,
        name: `JumpStart 4`,
      },
      {
        color: `red`,
        media: `red/06.mp4`,
        name: `JumpStart 6`,
      },
      {
        media: `green/Digital+Takeoff+Tutorial.mp4`,
        name: `Digital Takeoff Tutorial`,
      },
    ],
    takeoff: [
      {
        color: `red`,
        media: `red/05.mp4`,
        name: `JumpStart 5`,
      },
      {
        color: `yellow`,
        media: `yellow/03.mp4`,
        name: `Template 3`,
      },
      {
        media: `green/Takeoff+Tutorial.mp4`,
        name: `Takeoff Tutorial`,
      },
    ],
    expenses: [
      {
        color: `red`,
        media: `red/07.mp4`,
        name: `JumpStart 7`,
      },
      {
        color: `yellow`,
        media: `yellow/04.mp4`,
        name: `Template 4`,
      },
      {
        media: `green/Additional+Expenses+Tutorial.mp4`,
        name: `Additional Expenses Tutorial`,
      },
    ],
    summary: [
      {
        color: `red`,
        media: `red/08.mp4`,
        name: `JumpStart 8`,
      },
      {
        color: `yellow`,
        media: `yellow/05.mp4`,
        name: `Template 5`,
      },
      {
        media: `green/Summary+Tutorial.mp4`,
        name: `Bid Summary Tutorial`,
      },
      {
        media: `green/Adjustments+Summary+Turorial.mp4`,
        name: `Bid Adjustments Tutorial`,
      },
    ],
    proposal: [
      {
        color: `red`,
        media: `red/09.mp4`,
        name: `JumpStart 9`,
      },
      {
        color: `yellow`,
        media: `yellow/06.mp4`,
        name: `Template 6`,
      },
      {
        media: `green/Build+Proposal+Tutorial.mp4`,
        name: `Build Proposal Tutorial`,
      },
    ],
    send: [
      {
        color: `red`,
        media: `red/10.mp4`,
        name: `JumpStart 10`,
      },
      {
        media: `green/Send+Proposal+Tutorial.mp4`,
        name: `Send Proposal Tutorial`,
      },
    ],
    'work-order': [
      {
        media: `green/Send+Proposal+Tutorial.mp4`,
        name: `Send Work Order Tutorial`,
      },
    ],
    reports: [
      {
        media: `green/Reports+Tutorial.mp4`,
        name: `Reports Tutorial`,
      },
    ],
  }

  const {
    setHeading,
    setItems: setItems1,
    setTutorials,
  } = useContext(HeaderSecondaryContext)
  const { setItems: setItems2 } = useContext(HeaderTertiaryContext)

  useEffect(() => {
    setHeading(`Bids`)

    setItems1(segments)
    setItems2(items2)

    console.log(`debug.tutorial`, action, id)

    if (action) {
      if (tutorials[action]) {
        setTutorials(tutorials[action])
      }
    } else {
      setTutorials([
        {
          color: `red`,
          media: `red/01.mp4`,
          name: `JumpStart 1`,
        },
        {
          color: `yellow`,
          media: `yellow/01.mp4`,
          name: `Template 1`,
        },
        {
          media: `green/Bid+List+Tutorial.mp4`,
          name: `Bid List Tutorial`,
        },
      ])
    }
  }, [controller, action, id, user])

  //

  const [job, setJob]: any = useState<any>()
  const JobContextValue = { job, setJob }

  useEffect(() => {
    const call = async () => {
      if (!id) return

      const response = await axios.get(`/api/jobs/${id}`)
      const data = response.data.data

      setJob(data)
    }

    call()
  }, [id])

  //

  console.log(`debug.render`)

  return (
    <Routes>
      <Route
        index
        element={<Navigate to={`/app/jobs/all`} />}
      />

      <Route
        path={`all`}
        element={<JobCollection />}
      />
      <Route
        path={`all/create`}
        element={<JobForm />}
      />

      <Route
        path={`all/:id`}
        element={
          <JobContext.Provider value={JobContextValue}>
            <Outlet />
          </JobContext.Provider>
        }
      >
        <Route
          index
          element={<Navigate to={`edit`} />}
        />
        <Route
          path={`edit`}
          element={<JobForm />}
        />
        <Route
          path={`plans/*`}
          element={<JobPlans />}
        />
        <Route
          path={`takeoff`}
          element={<JobTakeoff />}
        />
        <Route
          path={`expenses`}
          element={<JobExpenses />}
        />
        <Route
          path={`summary`}
          element={<JobSummary />}
        />
        <Route
          path={`documents`}
          element={<JobDocuments />}
        />
        <Route
          path={`proposal`}
          element={<JobProposal />}
        />
        <Route
          path={`send`}
          element={<JobSend />}
        />
        <Route
          path={`work-order`}
          element={<JobWorkOrder />}
        />
        <Route
          path={`reports`}
          element={<JobReports />}
        />
      </Route>

      <Route
        path={`*`}
        element={<PageNotFound />}
      />
    </Routes>
  )
}
