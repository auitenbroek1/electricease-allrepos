import { useContext, useEffect } from 'react'
import { useHref, resolvePath, useLocation } from 'react-router-dom'

import { HeaderSecondaryContext } from '@/layouts/default/contexts/HeaderSecondary'
import { HeaderTertiaryContext } from '@/layouts/default/contexts/HeaderTertiary'

import {
  ArrowLongLeftIcon,
  InformationCircleIcon,
  UserGroupIcon,
  BoltIcon,
} from '@/components/Icons'

export const useNavigation = () => {
  const href = useHref(``)
  const location = useLocation()

  const parts = location.pathname.split(`/`).filter((item) => item.length)
  const controller = parts[2]
  const action = parts[parts.length === 5 ? 4 : 3]
  const id = parts.length === 5 ? parts[3] : null

  console.log(`debug.routes`, { href, parts, controller, action, id })

  const segments: any = []
  const items2: any = []

  if (controller && action) {
    const depth = id ? `../..` : `..`
    const controllers: any = {
      all: `All Members`,
    }

    segments.push({
      icon: ArrowLongLeftIcon,
      title: controllers[controller],
      to: resolvePath(depth, location.pathname),
    })

    if (controller === `all`) {
      const prefix = `members/all/${id}`

      items2.push({
        disabled: id ? false : true,
        icon: InformationCircleIcon,
        name: `Member Details`,
        to: `${prefix}/edit`,
      })

      items2.push({
        disabled: id ? false : true,
        icon: UserGroupIcon,
        name: `Users`,
        to: `${prefix}/users`,
      })

      items2.push({
        disabled: id ? false : true,
        icon: BoltIcon,
        name: `JumpStart`,
        to: `${prefix}/jumpstart`,
      })
    }
  } else {
    // segments.push({
    //   title: `All Jobs`,
    //   to: resolvePath(`all`, href),
    // })
  }

  const {
    setHeading,
    setItems: setItems1,
    setTutorials,
  } = useContext(HeaderSecondaryContext)
  const { setItems: setItems2 } = useContext(HeaderTertiaryContext)

  useEffect(() => {
    setHeading(`Members`)

    setItems1(segments)
    setItems2(items2)

    setTutorials([])
  }, [controller, action, id])
}
