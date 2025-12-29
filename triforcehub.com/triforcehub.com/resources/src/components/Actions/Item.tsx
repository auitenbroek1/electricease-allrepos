import { Menu } from '@headlessui/react'

const Item = (props: any) => {
  const { danger, children, icon, onClick } = props

  return (
    <Menu.Item as={`div`}>
      {({ active }) => (
        <button
          className={`
            group
            flex
            w-full
            items-center
            space-x-2
            rounded
            px-3
            py-2
            ${active ? `bg-gray-100` : ``}
            ${danger ? `hover:bg-red-600` : `hover:bg-blue-50`}
          `}
          onClick={onClick}
          type={`button`}
        >
          <span
            className={`
              h-5
              w-5
              text-gray-500
              ${danger ? `group-hover:text-white` : `group-hover:text-blue-700`}
            `}
          >
            {icon}
          </span>
          <span
            className={`
              text-sm
              text-gray-700
              ${danger ? `group-hover:text-white` : `group-hover:text-blue-700`}
            `}
          >
            {children}
          </span>
        </button>
      )}
    </Menu.Item>
  )
}

export default Item
