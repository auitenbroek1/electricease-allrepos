import { Route, Routes } from 'react-router-dom'

import { CollectionPage } from './collection'

export const CollectionRoutes = () => {
  return (
    <Routes>
      <Route
        element={<CollectionPage />}
        path={`*`}
      />
    </Routes>
  )
}
