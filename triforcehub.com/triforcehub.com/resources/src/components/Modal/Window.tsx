import { Fragment } from 'react'

import { Transition } from '@headlessui/react'

const Window = (props: any) => {
  const {
    children,
  } = props

  return (
    <Transition.Child
      as={Fragment}
      enter="ease-out duration-300"
      enterFrom="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
      enterTo="opacity-100 translate-y-0 sm:scale-100"
      leave="ease-in duration-200"
      leaveFrom="opacity-100 translate-y-0 sm:scale-100"
      leaveTo="opacity-0 translate-y-4 sm:translate-y-0 sm:scale-95"
    >
      <div
        className={`
          inline-block
          align-bottom
          bg-white
          rounded-lg
          text-left
          overflow-hidden
          shadow-xl
          transform
          transition-all
          sm:my-8
          sm:align-middle
          sm:max-w-lg
          sm:w-full
        `}
      >
        {children}
      </div>
    </Transition.Child>
  )
}

export default Window
