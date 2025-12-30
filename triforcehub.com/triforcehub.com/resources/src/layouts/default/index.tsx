import { Outlet } from 'react-router-dom'

import { CopilotProvider, useCopilot } from '../../contexts/CopilotContext'
import { CopilotSidePanel } from '../../components/copilot'
import { HeaderPrimary } from './components/HeaderPrimary'
import { HeaderSecondary } from './components/HeaderSecondary'
import { HeaderTertiary } from './components/HeaderTertiary'
import { Page } from './components/Page'
import { BackToTop } from './components/BackToTop'

// Inner layout that can access CopilotContext
const LayoutContent = () => {
  const { isOpen } = useCopilot()

  return (
    <div className="flex min-h-screen">
      {/* Main content area - shrinks when panel is open */}
      <div
        className={`
          flex-1
          min-w-0
          transition-all
          duration-300
          ease-in-out
          ${isOpen ? 'mr-[380px]' : 'mr-0'}
        `}
      >
        <div className="relative z-0 min-h-full">
          <HeaderPrimary />
          <HeaderSecondary />
          <HeaderTertiary />
          <Page>
            <Outlet />
          </Page>
          <BackToTop />
        </div>
      </div>

      {/* Copilot Side Panel */}
      <CopilotSidePanel />
    </div>
  )
}

export const Default = () => {
  return (
    <CopilotProvider>
      <LayoutContent />
    </CopilotProvider>
  )
}
