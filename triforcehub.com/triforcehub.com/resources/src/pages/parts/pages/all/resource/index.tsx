import { Navigate, Outlet, Route, Routes } from 'react-router-dom'

import { EditPage } from './edit'
import { AssembliesPage } from './assemblies'

export const ResourceRoutes = () => {
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
          element={<AssembliesPage />}
          path={`assemblies`}
        />
      </Route>
    </Routes>
  )
}
