import { Navigate, Outlet, Route, Routes, useParams } from 'react-router-dom'

import { EditPage } from './edit'
import { TakeoffPage } from './takeoff'

export const ResourceRoutes = () => {
  const params = useParams()
  console.log(`debug.routes`, `ResourceRoutes`, params)

  return (
    <Routes>
      <Route
        element={<Outlet />}
        path={`*`}
      >
        <Route
          element={<Navigate to={`edit`} />}
          index
        />
        <Route
          element={<EditPage />}
          path={`edit`}
        />
        <Route
          element={<TakeoffPage />}
          path={`takeoff`}
        />
      </Route>
    </Routes>
  )
}
