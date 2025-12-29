import { Navigate, Outlet, Route, Routes } from 'react-router-dom'

import { MemberProvider } from './contexts/MemberContext'

import { useNavigation } from './hooks/useNavigation'

import { MemberCollection } from './MemberCollection'
import { MemberForm } from './MemberForm'
import { MemberUsers } from './MemberUsers'
import { MemberJumpStart } from './MemberJumpStart'

import { PageNotFound } from '../errors'

export const Members = () => {
  useNavigation()

  return (
    <Routes>
      <Route
        index
        element={<Navigate to={`/app/members/all`} />}
      />

      <Route
        path={`all`}
        element={<MemberCollection />}
      />
      <Route
        path={`all/create`}
        element={<MemberForm />}
      />

      <Route
        path={`all/:id`}
        element={
          <MemberProvider>
            <Outlet />
          </MemberProvider>
        }
      >
        <Route
          index
          element={<Navigate to={`edit`} />}
        />
        <Route
          path={`edit`}
          element={<MemberForm />}
        />
        <Route
          path={`users`}
          element={<MemberUsers />}
        />
        <Route
          path={`jumpstart`}
          element={<MemberJumpStart />}
        />
      </Route>

      <Route
        path={`*`}
        element={<PageNotFound />}
      />
    </Routes>
  )
}
