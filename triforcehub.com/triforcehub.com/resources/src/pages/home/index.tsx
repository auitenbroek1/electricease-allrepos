import { useContext, useEffect } from 'react'
import { Route, Routes } from 'react-router-dom'

import { HeaderSecondaryContext } from '@/layouts/default/contexts/HeaderSecondary'
import { HeaderTertiaryContext } from '@/layouts/default/contexts/HeaderTertiary'

import { HomeIndex } from './HomeIndex'
import { PageNotFound } from '../errors'

export const Home = () => {
  const {
    setHeading,
    setItems: setItems1,
    setTutorials,
  } = useContext(HeaderSecondaryContext)
  const { setItems: setItems2 } = useContext(HeaderTertiaryContext)

  useEffect(() => {
    setHeading(`Dashboard`)

    setItems1([])
    setItems2([])

    setTutorials([
      {
        media: `Training_Dashboard.mp4`,
        name: `Dashboard Tutorial`,
      },
    ])
  }, [])

  return (
    <Routes>
      <Route
        index
        element={<HomeIndex />}
      />

      <Route
        path={`*`}
        element={<PageNotFound />}
      />
    </Routes>
  )
}
