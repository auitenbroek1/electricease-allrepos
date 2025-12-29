import { Route, Routes } from 'react-router-dom'

import { CollectionRoutes } from './collection'
import { CreateRoutes } from './create'
import { ResourceRoutes } from './resource'

export const All = () => {
  return (
    <Routes>
      <Route
        element={<CollectionRoutes />}
        path={`*`}
      />
      <Route
        element={<CreateRoutes />}
        path={`create/*`}
      />
      <Route
        path={`:id/*`}
        element={<ResourceRoutes />}
      />
    </Routes>
  )
}
