import { Outlet } from 'react-router-dom'

import { HeaderPrimary } from './components/HeaderPrimary'
import { HeaderSecondary } from './components/HeaderSecondary'
import { HeaderTertiary } from './components/HeaderTertiary'
import { Page } from './components/Page'
import { BackToTop } from './components/BackToTop'

export const Default = () => {
  return (
    <div className={`relative z-0 min-h-full`}>
      <HeaderPrimary />
      <HeaderSecondary />
      <HeaderTertiary />
      <Page>
        <Outlet />
      </Page>
      <BackToTop />
    </div>
  )
}
