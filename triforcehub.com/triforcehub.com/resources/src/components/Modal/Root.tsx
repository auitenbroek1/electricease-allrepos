import { Fragment } from 'react'

import { Dialog, Transition } from '@headlessui/react'

import Overlay from './Overlay'
import Window from './Window'

const Root = (props: any) => {
  const {
    children,
    open = false,
    onClose,
  } = props

  // const cancelButtonRef = useRef(null)

  return (
    <Transition.Root as={Fragment} show={open}>
      <Dialog
        className={`
          fixed
          inset-0
          overflow-y-auto
          z-10
        `}
        // initialFocus={cancelButtonRef}
        onClose={onClose}
      >
        <div
          className={`
            min-h-screen
            text-center
          `}
        >
          <Overlay />

          {/* This element is to trick the browser into centering the modal contents. */}
          <span className="hidden sm:inline-block sm:align-middle sm:h-screen" aria-hidden="true">
            &#8203;
          </span>

          <Window>
            {children}
          </Window>
        </div>
      </Dialog>
    </Transition.Root>
  )
}

export default Root
