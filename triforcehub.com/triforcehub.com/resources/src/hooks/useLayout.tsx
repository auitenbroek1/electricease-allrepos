import { useContext } from 'react'

import { HeaderSecondaryContext } from '@/layouts/default/contexts/HeaderSecondary'
import { HeaderTertiaryContext } from '@/layouts/default/contexts/HeaderTertiary'

export const useLayout = () => {
  const { setHeading: setHeaderSecondaryHeading, setItems: setHeaderSecondaryItems } =
    useContext(HeaderSecondaryContext)

  const { setItems: setHeaderTertiaryItems } = useContext(HeaderTertiaryContext)

  return {
    setHeaderSecondaryHeading,
    setHeaderSecondaryItems,

    setHeaderTertiaryItems,
  }
}
