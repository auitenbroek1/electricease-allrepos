import { useEffect, useState } from 'react'

import {
  Announcements,
  closestCenter,
  CollisionDetection,
  DragOverlay,
  DndContext,
  DropAnimation,
  defaultDropAnimation,
  KeyboardSensor,
  KeyboardCoordinateGetter,
  Modifiers,
  MouseSensor,
  MeasuringConfiguration,
  PointerActivationConstraint,
  ScreenReaderInstructions,
  TouchSensor,
  UniqueIdentifier,
  useSensor,
  useSensors,
} from '@dnd-kit/core'

import {
  arrayMove,
  useSortable,
  SortableContext,
  sortableKeyboardCoordinates,
  SortingStrategy,
  rectSortingStrategy,
  AnimateLayoutChanges,
  NewIndexGetter,
} from '@dnd-kit/sortable'

import Handle from './Handle'
import Items from './Items'
import Item from './Item'
import Placeholder from './Placeholder'

const defaultDropAnimationConfig: DropAnimation = {
  ...defaultDropAnimation,
  // dragSourceOpacity: 0.5,
}

const Root = (props: any) => {
  const {
    data,
    render,
    onSortEnd,
    activationConstraint,
    animateLayoutChanges,
    adjustScale = false,
    // Container = List,
    collisionDetection = closestCenter,
    coordinateGetter = sortableKeyboardCoordinates,
    dropAnimation = defaultDropAnimationConfig,
    getItemStyles = () => ({}),
    getNewIndex,
    handle = false,
    itemCount = 16,
    items: initialItems,
    isDisabled = () => false,
    measuring,
    modifiers,
    removable,
    renderItem,
    reorderItems = arrayMove,
    strategy = rectSortingStrategy,
    style,
    useDragOverlay = true,
    wrapperStyle = () => ({}),
  } = props

  const [items, setItems] = useState([])
  const [activeId, setActiveId] = useState(null)

  const sensors = useSensors(
    useSensor(MouseSensor, {
      activationConstraint,
    }),
    useSensor(TouchSensor, {
      activationConstraint,
    }),
    useSensor(KeyboardSensor, {
      coordinateGetter,
    }),
  )

  function handleDragStart(event: any) {
    const { active } = event

    setActiveId(active.id)
  }

  function handleDragEnd(event: any) {
    const { active, over } = event

    console.log(active.id, over.id)

    if (active.id !== over.id) {
      setItems((items) => {
        const oldIndex = items.findIndex((item: any) => item.id === active.id)
        const newIndex = items.findIndex((item: any) => item.id === over.id)
        console.log(oldIndex, newIndex)
        const updated = arrayMove(items, oldIndex, newIndex)
        onSortEnd(updated.map((item: any) => item.id))
        return updated
      })
    }

    setActiveId(null)
  }

  const getItem = (id: any) => {
    return items.find((item: any) => item.id === id)
  }

  useEffect(() => {
    setItems(data)
  }, [data])

  return (
    <DndContext
      sensors={sensors}
      collisionDetection={closestCenter}
      onDragStart={handleDragStart}
      onDragEnd={handleDragEnd}
      // measuring={measuring}
      // modifiers={modifiers}
    >
      {/* <div>active: {activeId}</div> */}
      {/* <div>{JSON.stringify(items)}</div> */}
      <SortableContext
        items={items}
        strategy={strategy}
      >
        <Items>
          {items?.map((item: any) => (
            <Item
              key={item.uuid}
              id={item.id}
              active={activeId === item.id}
              // animateLayoutChanges={animateLayoutChanges}
            >
              {render(item)}
            </Item>
          ))}
        </Items>
      </SortableContext>
      {true && (
        <DragOverlay
          adjustScale={adjustScale}
          dropAnimation={dropAnimation}
        >
          {activeId ? (
            <Placeholder isDragOverlay={true}>
              <Handle />
              {render(getItem(activeId))}
            </Placeholder>
          ) : null}
        </DragOverlay>
      )}
    </DndContext>
  )
}

export default Root
