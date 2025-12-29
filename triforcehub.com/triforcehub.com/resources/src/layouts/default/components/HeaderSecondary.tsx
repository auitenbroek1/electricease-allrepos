import { Fragment, useContext } from 'react'
import { HeaderSecondaryContext } from '@/layouts/default/contexts/HeaderSecondary'
import Trigger from '@/components/Trigger'

import TutorialModal from '@/components/Tutorial/TutorialModal'

export const HeaderSecondary = (props: any) => {
  const { children } = props

  // if (!children) return null

  const { items, heading, tutorials } = useContext(HeaderSecondaryContext)

  console.log(`debug.render`)

  return (
    <div
      className={`
        bg-white shadow
      `}
    >
      <div
        className={`
          flex
          justify-between
          py-4
          px-8
        `}
        id={`header-secondary`}
      >
        <div
          className={`
            flex
            items-center
            space-x-8
        `}
        >
          {heading && (
            <h1 className="text-lg font-semibold leading-6">{heading}</h1>
          )}
          {children}
          {items && (
            <div className="flex space-x-4">
              {items.map((segment: any, index: any) => (
                <Fragment key={index}>
                  {index > 0 && <div className="text-sm text-gray-300">/</div>}
                  <Trigger.Anchor
                    icon={
                      segment.icon && (
                        <div className={`h-5 w-5`}>
                          <segment.icon />
                        </div>
                      )
                    }
                    title={segment.title}
                    to={segment.to}
                  />
                </Fragment>
              ))}
            </div>
          )}
        </div>
        <div
          className={`
            flex
            items-center
            space-x-8
        `}
        >
          {tutorials && <TutorialModal tutorials={tutorials} />}
        </div>
      </div>
    </div>
  )
}
