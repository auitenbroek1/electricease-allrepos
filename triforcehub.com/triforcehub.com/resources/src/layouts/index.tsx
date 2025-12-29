import { HeaderPrimaryContextProvider } from '@/layouts/default/contexts/HeaderPrimary'
import { HeaderSecondaryContextProvider } from '@/layouts/default/contexts/HeaderSecondary'
import { HeaderTertiaryContextProvider } from '@/layouts/default/contexts/HeaderTertiary'

import { Default } from './default'

export const Layout = () => {
  return (
    <HeaderPrimaryContextProvider>
      <HeaderSecondaryContextProvider>
        <HeaderTertiaryContextProvider>
          <Default />
        </HeaderTertiaryContextProvider>
      </HeaderSecondaryContextProvider>
    </HeaderPrimaryContextProvider>
  )
}
