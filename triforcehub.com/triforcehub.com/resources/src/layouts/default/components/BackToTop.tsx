import { useWindowScroll } from '@mantine/hooks'
import { Affix, Transition } from '@mantine/core'

import { ArrowSmallUpIcon } from '@/components/Icons'

export const BackToTop = () => {
  const [scroll, scrollTo] = useWindowScroll()

  return (
    <Affix position={{ bottom: 16, right: 16 }}>
      <Transition
        transition="slide-up"
        mounted={scroll.y > 0}
      >
        {(transitionStyles) => (
          <button
            className={`
              flex
              min-w-[120px]
              items-center
              justify-center
              space-x-2
              rounded-md
              border
              border-transparent
              bg-blue-600
              px-4
              py-2
              text-sm
              font-medium
              text-white
              hover:bg-blue-700
              focus:outline-none
            `}
            type={`button`}
            style={transitionStyles}
            onClick={() => scrollTo({ y: 0 })}
          >
            <div className={`h-5 w-5`}>
              <ArrowSmallUpIcon />
            </div>
            <div>Scroll to top</div>
          </button>
        )}
      </Transition>
    </Affix>
  )
}
