import { useContext, useEffect, useRef } from 'react'
import { useHref, resolvePath, useLocation } from 'react-router-dom'

import { HeaderSecondaryContext } from '@/layouts/default/contexts/HeaderSecondary'
import { HeaderTertiaryContext } from '@/layouts/default/contexts/HeaderTertiary'

import { ArrowLongLeftIcon, InformationCircleIcon } from '@/components/Icons'

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
      all: `All Material`,
      categories: `All Categories`,
      tags: `All Tags`,
    }

    segments.push({
      icon: ArrowLongLeftIcon,
      title: controllers[controller],
      to: resolvePath(depth, location.pathname),
    })

    if (controller === `all`) {
      const prefix = `parts/all/${id}`

      items2.push({
        disabled: id ? false : true,
        icon: InformationCircleIcon,
        name: `Material Details`,
        to: `${prefix}/edit`,
      })

      // items2.push({
      //   disabled: id ? false : true,
      //   icon: InformationCircleIcon,
      //   name: `Assemblies`,
      //   to: `${prefix}/assemblies`,
      // })
    }
  } else {
    segments.push({
      title: `All Material`,
      to: resolvePath(`all`, href),
    })

    segments.push({
      title: `Categories`,
      to: resolvePath(`categories`, href),
    })

    segments.push({
      title: `Tags`,
      to: resolvePath(`tags`, href),
    })
  }

  const {
    setHeading,
    setItems: setItems1,
    setTutorials,
  } = useContext(HeaderSecondaryContext)
  const { setItems: setItems2 } = useContext(HeaderTertiaryContext)

  useEffect(() => {
    setHeading(`Material`)

    setItems1(segments)
    setItems2(items2)

    if ([`all`].includes(controller) && !id) {
      setTutorials([
        {
          media: `green/Material+Vault+Tutorial.mp4`,
          name: `Material Vault Tutorial`,
        },
      ])
    } else if ([`categories`, `tags`].includes(controller) && !id) {
      setTutorials([
        {
          media: `Training_TagsAndCategories.mp4`,
          name: `Tags and Categories Tutorial`,
        },
      ])
    } else {
      setTutorials(null)
    }

    console.log(`debug.header.navigation`)
  }, [controller, action, id])
}
