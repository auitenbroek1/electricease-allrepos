import { Bars2Icon } from '@/components/Icons'

const Handle = (props: any) => {
  return (
    <div
      className={`flex items-center justify-center text-brand-gradient-light`}
      {...props}
    >
      <div className={`h-5 w-5`}>
        <Bars2Icon />
      </div>
    </div>
  )
}

export default Handle
