import { forwardRef } from 'react'

const Placeholder = (props: any, ref: any) => {
  const { children, active, isDragOverlay = false, style } = props

  return (
    <div
      className={`
        flex
        w-full
        space-x-4
        ${isDragOverlay ? `cursor-grabbing` : ``}
        ${active ? `opacity-50` : ``}
      `}
      ref={ref}
      style={style}
    >
      {children}
    </div>
  )
}

export default forwardRef(Placeholder)
