import { useSortable } from '@dnd-kit/sortable'
import { CSS } from '@dnd-kit/utilities'

import Handle from './Handle'
import Placeholder from './Placeholder'

const Item = (props: any) => {
  const { children, id, active, animateLayoutChanges } = props

  const {
    attributes,
    isDragging,
    isSorting,
    listeners,
    activeIndex,
    overIndex,
    setNodeRef,
    transform,
    transition,
  } = useSortable({ id, animateLayoutChanges })

  const style = {
    transform: CSS.Transform.toString(transform),
    transition,
  }

  return (
    <Placeholder ref={setNodeRef} active={active} style={style}>
      <Handle {...attributes} {...listeners} />
      <span className={`w-full`}>{children}</span>
    </Placeholder>
  )
}

export default Item
