import { useContext } from 'react'

import { Menu } from '@headlessui/react'

import Context from './Context'

const Trigger = (props: any) => {
  const { children } = props

  const context = useContext(Context)

  return (
    <Menu.Button
      as={`div`}
      // className={'bg-red-500'}
      ref={context.setTriggerElement}
    >
      {children}
    </Menu.Button>
  )
}

export default Trigger
