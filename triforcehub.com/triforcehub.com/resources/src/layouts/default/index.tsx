import { Outlet } from 'react-router-dom'

import { CopilotProvider } from '../../contexts/CopilotContext'
import { CopilotPanel, CopilotToggle } from '../../components/copilot'
import { HeaderPrimary } from './components/HeaderPrimary'
import { HeaderSecondary } from './components/HeaderSecondary'
import { HeaderTertiary } from './components/HeaderTertiary'
import { Page } from './components/Page'
import { BackToTop } from './components/BackToTop'

export const Default = () => {
  return (
    <CopilotProvider>
      <div className={`relative z-0 min-h-full`}>
        <HeaderPrimary />
        <HeaderSecondary />
        <HeaderTertiary />
        <Page>
          <Outlet />
        </Page>
        <BackToTop />
      </div>
      <CopilotPanel />
      <CopilotToggle />
    </CopilotProvider>
  )
}
