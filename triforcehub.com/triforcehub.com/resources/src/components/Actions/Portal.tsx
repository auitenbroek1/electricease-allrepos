import { Fragment, useContext, useLayoutEffect } from 'react'
import {
  autoUpdate,
  flip,
  FloatingPortal,
  offset,
  shift,
  useFloating,
} from '@floating-ui/react-dom-interactions'
import { Menu, Transition } from '@headlessui/react'
import Context from './Context'

const Portal = (props: any) => {
  const { children } = props

  const { x, y, reference, floating, strategy } = useFloating({
    middleware: [offset({ mainAxis: 12, crossAxis: 8 }), flip(), shift()],
    placement: `bottom-end`,
    whileElementsMounted: autoUpdate,
  })

  const context = useContext(Context)

  useLayoutEffect(() => {
    reference(context.triggerElement)
  }, [context.triggerElement])

  return (
    <FloatingPortal>
      <div
        ref={floating}
        style={{
          position: strategy,
          top: y ?? ``,
          left: x ?? ``,
        }}
      >
        <Transition
          as={Fragment}
          enter={`transition ease-out duration-100`}
          enterFrom={`transform opacity-0 scale-95`}
          enterTo={`transform opacity-100 scale-100`}
          leave={`transition ease-in duration-75`}
          leaveFrom={`transform opacity-100 scale-100`}
          leaveTo={`transform opacity-0 scale-95`}
        >
          <Menu.Items className={`origin-top-right focus:outline-none`}>
            <div
              className={`
                min-w-[15rem]
                origin-top-right
                select-none
                divide-y
                divide-gray-200
                rounded-md
                bg-white
                shadow-lg
                ring-1
                ring-black
                ring-opacity-5
                focus:outline-none
              `}
            >
              {children}
            </div>
          </Menu.Items>
        </Transition>
      </div>
    </FloatingPortal>
  )
}

export default Portal
