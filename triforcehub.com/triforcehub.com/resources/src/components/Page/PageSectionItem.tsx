import { ChevronDownIcon, ChevronRightIcon } from '@/components/Icons'
import { useEffect, useState } from 'react'

type Props = {
  actions?: React.ReactNode
  children: React.ReactNode
  fingerprint?: string
  heading?: string
  openByDefault?: boolean
  onToggle?: any
}

const PageSectionItem = (props: Props) => {
  const { actions, children, fingerprint, heading, openByDefault, onToggle } =
    props

  const [open, setOpen] = useState(openByDefault || !heading)

  useEffect(() => {
    let state = openByDefault || !heading
    if (fingerprint) {
      if (window.localStorage.getItem(fingerprint)) {
        state = true
      } else {
        if (state) window.localStorage.setItem(fingerprint, `true`)
      }
    }
    setOpen(state)
  }, [openByDefault, heading])

  const toggle = () => {
    const state = !open
    if (fingerprint) {
      if (state) window.localStorage.setItem(fingerprint, `true`)
      else window.localStorage.removeItem(fingerprint)
    }
    setOpen(state)
    onToggle && onToggle(state)
  }

  return (
    <div className={`${open ? `overflow-hidden rounded shadow` : ``}`}>
      {heading ? (
        <div
          className={`relative z-10 ${
            open
              ? `bg-brand-black text-white`
              : `rounded border border-brand-black border-opacity-50`
          }`}
        >
          <div
            className={`flex cursor-pointer items-center space-x-4 px-4 py-3`}
            onClick={toggle}
          >
            <button
              className={`
                group
                -m-2
                flex
                h-9
                w-9
                items-center
                justify-center
                rounded-full
                p-2
                ${open ? `hover:bg-white` : `hover:bg-black`}
                hover:bg-opacity-10
                focus:outline-none
              `}
              type={`button`}
            >
              {open ? (
                <div
                  className={`
                    h-4
                    w-4
                    opacity-80
                    group-hover:opacity-100
                  `}
                >
                  <ChevronDownIcon />
                </div>
              ) : (
                <div
                  className={`
                    h-4
                    w-4
                    opacity-80
                    group-hover:opacity-100
                  `}
                >
                  <ChevronRightIcon />
                </div>
              )}
            </button>
            <span>{heading}</span>
          </div>
          {open && actions && (
            <div
              className={`absolute right-0 top-0 flex h-full items-center space-x-4 px-4 py-3`}
            >
              {actions}
            </div>
          )}
        </div>
      ) : null}
      <div className={`relative z-0 bg-white p-8 ${open ? `block` : `hidden`}`}>
        {children}
      </div>
    </div>
  )
}

export default PageSectionItem
