import { Route, Routes } from 'react-router-dom'

import { PartCategoryCollection } from './collection/PartCategoryCollection'
import { PartCategoryForm } from './resource/PartCategoryForm'

export const Categories = () => {
  return (
    <Routes>
      <Route
        element={<PartCategoryCollection />}
        path={`*`}
      />
      <Route
        element={<PartCategoryForm />}
        path={`create`}
      />
      <Route
        element={<PartCategoryForm />}
        path={`:id/edit`}
      />
    </Routes>
  )
}
