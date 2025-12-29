import { Route, Routes } from 'react-router-dom'

import { CreatePage } from './create'

export const CreateRoutes = () => {
  return (
    <Routes>
      <Route
        element={<CreatePage />}
        path={`*`}
      />
    </Routes>
  )
}
