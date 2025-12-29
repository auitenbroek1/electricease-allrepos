import { Link } from 'react-router-dom'

import Logo from '@/components/Logo'
import ProfileDesktop from './ProfileDesktop'

import RouterLink from '@/components/Router/RouterLink'
import { BellIcon } from '@/components/Icons'

import { useAuth } from '@/contexts/AuthContext'
import { useState } from 'react'
import { Sparkle } from '@/components/Icons/Sparkle'
import { Modal } from '@/components'
import axios from 'axios'
import toast from 'react-hot-toast'

export const HeaderPrimary = (props: any) => {
  const { children } = props

  const { user } = useAuth()

  const navigationLinks: any = [
    { title: `Dashboard`, href: `/app/home` },
    { title: `Bids`, href: `/app/jobs/all` },
    { title: `Assemblies`, href: `/app/assemblies/all` },
    { title: `Material`, href: `/app/parts/all` },
  ]

  if (user?.member_id === 1) {
    navigationLinks.push({
      href: `/app/members/all`,
      title: `Members`,
    })
  }

  console.log(`debug.render`)

  //

  const features = [
    {
      id: 1,
      name: `QuickSearch AI`,
      description: `QuickSearch AI uses smart, natural language search to help you locate materials, assemblies, specs, and more — all without digging through endless lists.`,
      enabled: true,
    },
    {
      id: 2,
      name: `Member Portal`,
      enabled: false,
    },
    {
      id: 3,
      name: `Purchase Orders`,
      enabled: false,
    },
    {
      id: 4,
      name: `Quantum Count`,
      enabled: false,
    },
  ]

  const default_selected_features = user.member.features.map((feature: any) =>
    feature.id.toString(),
  )
  console.log(`debug.features`, { default_selected_features })

  const [selected_features, set_selected_features] = useState(
    default_selected_features,
  )

  const handleFeaturesChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    set_selected_features((prev: string[]) =>
      e.target.checked
        ? [...new Set([...prev, e.target.value])]
        : prev.filter((id: string) => id !== e.target.value),
    )
  }

  const [showFeatureFlags, setShowFeatureFlags] = useState(false)

  const handleSaveFeatureFlags = async () => {
    setShowFeatureFlags(false)
    try {
      await axios.patch(`/api/members/${user.member.id}/features`, {
        features: selected_features,
      })

      toast.success(
        `Your feature selection has been saved! You may need to refresh the page for the changes to take effect.`,
      )
    } catch (error: any) {
      toast.error(
        `There was an error saving your feature selection. Please try again.`,
      )
    }
  }

  //

  const notifications_dismissed =
    localStorage.getItem(`notifications_dismissed_2`) ?? false
  console.log(`notifications`, notifications_dismissed)

  const [has_notifications, set_has_notifications] = useState(
    !notifications_dismissed,
  )
  const [show_notifications, set_show_notifications] = useState(false)

  const dismiss_notifications = () => {
    set_show_notifications(false)
    set_has_notifications(false)
    localStorage.setItem(`notifications_dismissed_2`, `true`)
  }

  //

  return (
    <div className="relative z-10 bg-gradient-to-r from-brand-gradient-dark to-brand-gradient-light">
      <div className="px-8">
        <div className="flex h-16 items-center justify-between">
          <div className="flex items-center">
            <div className="flex-shrink-0">
              <div className={`block h-8 text-brand-gold`}>
                <Logo />
              </div>
            </div>
            <div className="">
              <div className="ml-8 flex items-baseline space-x-4">
                {navigationLinks.map((item: any, index: number) => (
                  <RouterLink
                    key={index}
                    to={item.href}
                  >
                    {({ active, to }: any) => (
                      <Link
                        className={`
                        ${
                          active
                            ? `bg-black bg-opacity-25 text-white`
                            : `text-white opacity-75 hover:bg-white hover:bg-opacity-10 hover:text-white hover:opacity-100`
                        }
                        rounded-md
                        px-3
                        py-2
                        text-sm
                        font-medium
                      `}
                        to={to}
                      >
                        {item.title}
                      </Link>
                    )}
                  </RouterLink>
                ))}
              </div>
            </div>
          </div>
          <div className="">
            <div className="ml-6 flex items-center">
              <div className="h-6 w-6 text-white opacity-75 hover:opacity-100 relative mr-3">
                <button
                  onClick={() => setShowFeatureFlags(true)}
                  type="button"
                >
                  <Sparkle />
                </button>
                <Modal.Root
                  open={showFeatureFlags}
                  onClose={() => setShowFeatureFlags(false)}
                >
                  <Modal.Content>
                    <div className="mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full bg-blue-100 sm:mx-0 sm:h-10 sm:w-10">
                      <div className="h-6 w-6 text-blue-600">
                        <Sparkle />
                      </div>
                    </div>
                    <div className="text-center sm:mt-0 sm:ml-4 sm:text-left">
                      <div className="text-lg font-medium leading-6">
                        Exciting New Features
                      </div>
                      <div className="mt-4">
                        <div className="text-sm text-gray-500">
                          Be the first to try out our new features! We are
                          constantly working to improve your experience, and
                          your feedback is invaluable to us. If you encounter
                          any issues or have suggestions, please let us know.
                        </div>
                      </div>
                      <hr className="my-4" />
                      <div className="space-y-4">
                        {features.map((feature, index) => (
                          <div key={index}>
                            <label
                              className={`text-sm flex items-center space-x-2 cursor-pointer ${!feature.enabled ? `pointer-events-none` : ``}`}
                            >
                              <div
                                className={`${!feature.enabled ? `opacity-50` : ``}`}
                              >
                                <input
                                  defaultChecked={selected_features.includes(
                                    feature.id.toString(),
                                  )}
                                  onChange={handleFeaturesChange}
                                  type="checkbox"
                                  value={feature.id}
                                />
                              </div>
                              <div>{feature.name}</div>
                              {!feature.enabled ? (
                                <div className="text-xs bg-gray-100 px-2 py-0.5 rounded text-gray-500">
                                  Coming Soon
                                </div>
                              ) : (
                                <div className="text-xs bg-green-200 px-2 py-0.5 rounded text-green-800">
                                  Available Now!
                                </div>
                              )}
                            </label>
                            <div className="text-sm mt-2 text-gray-500">
                              {feature.description ?? ``}
                            </div>
                          </div>
                        ))}
                      </div>
                    </div>
                  </Modal.Content>
                  <Modal.Buttons>
                    <button
                      type="button"
                      className="mt-3 inline-flex w-full justify-center rounded-md border border-gray-300 bg-white px-4 py-2 text-base font-medium text-gray-700 shadow-sm hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2 sm:mt-0 sm:ml-3 sm:w-auto sm:text-sm"
                      onClick={() => setShowFeatureFlags(false)}
                    >
                      Cancel
                    </button>
                    <button
                      type="button"
                      className="inline-flex w-full justify-center rounded-md border border-transparent bg-blue-600 px-4 py-2 text-base font-medium text-white shadow-sm hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2 sm:ml-3 sm:w-auto sm:text-sm"
                      onClick={handleSaveFeatureFlags}
                    >
                      Save
                    </button>
                  </Modal.Buttons>
                </Modal.Root>
              </div>
              <button
                className="text-white opacity-75 hover:opacity-100 relative"
                onClick={() => set_show_notifications(!show_notifications)}
                type="button"
              >
                <div className="h-6 w-6">
                  <BellIcon />
                  {has_notifications && (
                    <span className="absolute -top-1 -right-1 h-4 w-4 bg-red-500 rounded-full flex items-center justify-center">
                      <span className="text-[12px] leading-none">1</span>
                    </span>
                  )}
                </div>
              </button>
              <ProfileDesktop />
            </div>
          </div>
        </div>
      </div>
      {has_notifications && show_notifications && (
        <div className="bg-white px-6 py-4 border border-b space-x-4 flex items-center justify-between">
          <div className="flex space-x-4 items-center">
            <div>
              <svg
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 24 24"
                strokeWidth={1.5}
                stroke="currentColor"
                className="w-6 h-6"
              >
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="m11.25 11.25.041-.02a.75.75 0 0 1 1.063.852l-.708 2.836a.75.75 0 0 0 1.063.853l.041-.021M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0Zm-9-3.75h.008v.008H12V8.25Z"
                />
              </svg>
            </div>
            <div className="text-sm">
              <strong>New Feature Alert: Quantity & Adjust is Here!</strong>
              {` `}
              <span>
                We’ve added a new tool to your takeoff section: Quantity &
                Adjust. It lets you manage takeoff quantities more easily with
                manual adjustments. Watch your email for more details on this
                new feature!
              </span>
            </div>
          </div>
          <div>
            <button
              className="text-sm text-brand-gradient-light hover:underline"
              onClick={dismiss_notifications}
            >
              Dismiss
            </button>
          </div>
        </div>
      )}
    </div>
  )
}
