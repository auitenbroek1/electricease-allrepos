import { useState } from 'react'

import { Menu } from '@headlessui/react'

import Context from './Context'

const Root = (props: any) => {
  const { children } = props

  const [triggerElement, setTriggerElement]: any = useState()
  const [portalElement, setPortalElement]: any = useState()

  const context = {
    triggerElement,
    setTriggerElement,

    portalElement,
    setPortalElement,
  }

  return (
    <Context.Provider value={context}>
      <Menu>{children}</Menu>
    </Context.Provider>
  )
}

export default Root
