import { Navigate, Outlet, Route, Routes } from 'react-router-dom'

import { useNavigation } from './hooks/useNavigation'

import { AssemblyCollection } from './AssemblyCollection'
import { AssemblyForm } from './AssemblyForm'

import { AssemblyCategoryCollection } from './AssemblyCategoryCollection'
import { AssemblyCategoryForm } from './AssemblyCategoryForm'

import { AssemblyTagCollection } from './AssemblyTagCollection'
import { AssemblyTagForm } from './AssemblyTagForm'

import { PageNotFound } from '../errors'

export const Assemblies = () => {
  useNavigation()

  return (
    <Routes>
      <Route
        element={<Navigate to={`/app/assemblies/all`} />}
        index
      />

      <Route
        element={<AssemblyCollection />}
        path={`all`}
      />
      <Route
        element={<AssemblyForm />}
        path={`all/create`}
      />
      <Route
        element={<AssemblyForm />}
        path={`all/:id/edit`}
      />

      <Route
        element={<AssemblyCategoryCollection />}
        path={`categories`}
      />
      <Route
        element={<AssemblyCategoryForm />}
        path={`categories/create`}
      />
      <Route
        element={<AssemblyCategoryForm />}
        path={`categories/:id/edit`}
      />

      <Route
        element={<AssemblyTagCollection />}
        path={`tags`}
      />
      <Route
        element={<AssemblyTagForm />}
        path={`tags/create`}
      />
      <Route
        element={<AssemblyTagForm />}
        path={`tags/:id/edit`}
      />

      <Route
        element={<PageNotFound />}
        path={`*`}
      />
    </Routes>
  )
}
