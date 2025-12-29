import { Fragment } from 'react'

import { Dialog, Transition } from '@headlessui/react'

const Overlay = () => {
  return (
    <Transition.Child
      as={Fragment}
      enter={`ease-out duration-300`}
      enterFrom={`opacity-0`}
      enterTo={`opacity-100`}
      leave={`ease-in duration-200`}
      leaveFrom={`opacity-100`}
      leaveTo={`opacity-0`}
    >
      <Dialog.Overlay
        className={`
          bg-black
          bg-opacity-50
          fixed
          inset-0
          transition-opacity
      `}
      />
    </Transition.Child>
  )
}

export default Overlay
