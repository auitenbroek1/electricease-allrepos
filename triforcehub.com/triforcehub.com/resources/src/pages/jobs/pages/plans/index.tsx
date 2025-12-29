import { Navigate, Route, Routes } from 'react-router-dom'

import { AllRoutes } from './all'
import { PageNotFound } from '@/pages/errors'

export const JobPlans = () => {
  return (
    <Routes>
      <Route
        element={<Navigate to={`all`} />}
        index
      />
      <Route
        element={<AllRoutes />}
        path={`all/*`}
      />
      <Route
        element={<PageNotFound />}
        path={`*`}
      />
    </Routes>
  )
}
