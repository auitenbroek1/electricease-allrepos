import { Navigate, Route, Routes } from 'react-router-dom'

import { AuthProvider } from '@/contexts/AuthContext'

import { Layout } from '@/layouts'

import { Assemblies } from '@/pages/assemblies'
import { Home } from '@/pages/home'
import { Jobs } from '@/pages/jobs'
import { Members } from '@/pages/members'
import { PageNotFound } from '@/pages/errors'
import { Parts } from '@/pages/parts'
import { Profile } from '@/pages/profile'
import { Tutorials } from '@/pages/tutorials'

export const App = () => {
  return (
    <AuthProvider>
      <Routes>
        <Route
          path={`/app`}
          element={<Layout />}
        >
          <Route
            index
            element={<Navigate to={`/app/home`} />}
          />

          <Route
            path={`assemblies/*`}
            element={<Assemblies />}
          />
          <Route
            path={`home/*`}
            element={<Home />}
          />
          <Route
            path={`jobs/*`}
            element={<Jobs />}
          />
          <Route
            path={`members/*`}
            element={<Members />}
          />
          <Route
            path={`parts/*`}
            element={<Parts />}
          />
          <Route
            path={`profile/*`}
            element={<Profile />}
          />
          <Route
            path={`tutorials/*`}
            element={<Tutorials />}
          />

          <Route
            path={`*`}
            element={<PageNotFound />}
          />
        </Route>
      </Routes>
    </AuthProvider>
  )
}
