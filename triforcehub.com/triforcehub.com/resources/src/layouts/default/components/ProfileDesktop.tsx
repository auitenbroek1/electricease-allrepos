import axios from 'axios'
import { NavLink } from 'react-router-dom'

import {
  ArrowRightOnRectangleIcon,
  Cog6ToothIcon,
  UserIcon,
  VideoCameraIcon,
} from '@/components/Icons'

import { useAuth } from '@/contexts/AuthContext'
import { Actions } from '@/components'

const DesktopProfile = () => {
  const { user, refetch } = useAuth()

  const handleLogOut = async () => {
    await axios.post(`/auth/logout `)
    location.href = `/auth/login`
  }

  const handleStopImpersonating = async () => {
    const response = await axios.delete(`/api/impersonation`)
    console.log(response)
    await refetch()
    window.location.reload()
  }

  return (
    <div className="relative ml-3">
      <div className="flex">
        <Actions.Root>
          <Actions.Trigger>
            <button
              className={`
                text-white
                opacity-75
                hover:opacity-100
              `}
              type={`button`}
            >
              <div className={`h-6 w-6`}>
                <UserIcon />
              </div>
            </button>
          </Actions.Trigger>
          <Actions.Portal>
            <Actions.Group>
              <Actions.Title icon={<Cog6ToothIcon />}>
                Account Options
              </Actions.Title>
            </Actions.Group>
            <Actions.Group>
              <div
                className="space-y-1 whitespace-nowrap px-4 py-3"
                role="none"
              >
                <span className="block text-xs text-gray-500">
                  Signed in as
                </span>
                <div className="text-sm font-semibold">{user.member.name}</div>
                <div className="text-sm">{user.email}</div>
              </div>
            </Actions.Group>
            {user.impersonated ? (
              <Actions.Group>
                <Actions.Item
                  icon={<ArrowRightOnRectangleIcon />}
                  onClick={handleStopImpersonating}
                >
                  Stop Impersonating
                </Actions.Item>
              </Actions.Group>
            ) : null}
            <Actions.Group>
              <NavLink to={`/app/profile`}>
                <Actions.Item icon={<UserIcon />}>Profile</Actions.Item>
              </NavLink>
              {/* </Actions.Group> */}
              {/* <Actions.Group> */}
              {/* <NavLink to={`/app/tutorials`}>
                <Actions.Item icon={<VideoCameraIcon />}>
                  Training Library
                </Actions.Item>
              </NavLink> */}
            </Actions.Group>
            <Actions.Group>
              <Actions.Item
                danger={true}
                icon={<ArrowRightOnRectangleIcon />}
                onClick={handleLogOut}
              >
                Log Out
              </Actions.Item>
            </Actions.Group>
          </Actions.Portal>
        </Actions.Root>
      </div>
    </div>
  )
}

export default DesktopProfile
