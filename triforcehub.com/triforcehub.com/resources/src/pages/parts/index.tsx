import { Navigate, Route, Routes } from 'react-router-dom'

import { PageNotFound } from '@/pages/errors'

import { useNavigation } from './hooks/useNavigation'

import { All } from './pages/all'
import { Categories } from './pages/categories'
import { Tags } from './pages/tags'

export const Parts = () => {
  useNavigation()

  return (
    <Routes>
      <Route
        element={<Navigate to={`/app/parts/all`} />}
        index
      />
      <Route
        element={<All />}
        path={`all/*`}
      />
      <Route
        element={<Categories />}
        path={`categories/*`}
      />
      <Route
        element={<Tags />}
        path={`tags/*`}
      />
      <Route
        element={<PageNotFound />}
        path={`*`}
      />
    </Routes>
  )
}
