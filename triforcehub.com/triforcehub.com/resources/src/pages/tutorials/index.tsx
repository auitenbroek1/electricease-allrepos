import { useContext, useEffect } from 'react'
import { Route, Routes } from 'react-router-dom'

import { HeaderSecondaryContext } from '@/layouts/default/contexts/HeaderSecondary'
import { HeaderTertiaryContext } from '@/layouts/default/contexts/HeaderTertiary'

import { TutorialsIndex } from './TutorialsIndex'
import { PageNotFound } from '../errors'

export const Tutorials = () => {
  const {
    setHeading,
    setItems: setItems1,
    setTutorials,
  } = useContext(HeaderSecondaryContext)
  const { setItems: setItems2 } = useContext(HeaderTertiaryContext)

  useEffect(() => {
    setHeading(`Training Library`)

    setItems1([])
    setItems2([])

    setTutorials(null)
  }, [])

  return (
    <Routes>
      <Route
        index
        element={<TutorialsIndex />}
      />

      <Route
        path={`*`}
        element={<PageNotFound />}
      />
    </Routes>
  )
}
