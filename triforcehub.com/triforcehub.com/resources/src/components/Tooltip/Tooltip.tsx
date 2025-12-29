import { ReactNode } from 'react'

import { Tooltip as Wrapper } from '@mantine/core'

type PropsType = {
  children: ReactNode
  messages?: string[]
}

export const Tooltip = (props: PropsType) => {
  const { children, messages } = props

  if (!messages || messages.length === 0) {
    return <div>{children}</div>
  }

  return (
    <Wrapper
      arrowSize={8}
      label={messages.join(` `)}
      withArrow
      withinPortal
    >
      <div className="cursor-help">{children}</div>
    </Wrapper>
  )
}
