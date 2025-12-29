import { Fragment, useContext } from 'react'
import { HeaderTertiaryContext } from '@/layouts/default/contexts/HeaderTertiary'
import Trigger from '@/components/Trigger'

export const HeaderTertiary = (props: any) => {
  const { children } = props

  // if (!children) return null

  const { items } = useContext(HeaderTertiaryContext)

  if (items.length === 0) return null

  console.log(`debug.header.tertiary`)

  return (
    <div
      className={`
        border-b
        border-gray-300
        px-8
        py-4
      `}
    >
      <div
        className={`
          -mb-px
          flex
          h-5
          space-x-8
        `}
        id={`header-tertiary`}
      >
        {items.map((tab: any, index: any) => (
          <Fragment key={index}>
            <Trigger.Anchor
              disabled={tab.disabled}
              icon={
                <div className={`h-5 w-5`}>
                  <tab.icon />
                </div>
              }
              title={tab.name}
              to={tab.to}
            />
          </Fragment>
        ))}
        {children}
      </div>
    </div>
  )
}
