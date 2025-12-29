import { Route, Routes } from 'react-router-dom'

import { PartTagCollection } from './collection/PartTagCollection'
import { PartTagForm } from './resource/PartTagForm'

export const Tags = () => {
  return (
    <Routes>
      <Route
        element={<PartTagCollection />}
        path={`*`}
      />
      <Route
        element={<PartTagForm />}
        path={`create`}
      />
      <Route
        element={<PartTagForm />}
        path={`:id/edit`}
      />
    </Routes>
  )
}
