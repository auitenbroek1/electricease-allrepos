import { ChevronDownIcon, ChevronRightIcon } from '@/components/Icons'
import { useCard } from './Context'

export const HeaderNew = (props: any) => {
  const { children } = props

  const { open, setOpen } = useCard()

  const actions = null

  const toggle = () => {
    console.log(`here`, open)
    setOpen(!open)
  }

  return (
    <div
      className={`relative z-10 border border-brand-black ${
        open ? `bg-brand-black text-white` : `rounded border-opacity-50`
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
        <span>{children}</span>
      </div>
      {open && actions && (
        <div
          className={`absolute right-0 top-0 flex h-full items-center space-x-4 px-4 py-3`}
        >
          {actions}
        </div>
      )}
    </div>
  )
}
