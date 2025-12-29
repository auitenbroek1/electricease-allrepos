import { Children } from 'react'

import Item from './Item'

const Items = (props: any) => {
  // console.log('resolve.items', typeof props.children, props)

  const resolved = Children.map(props.children, (child, i) => {
    // const type = child?.type?.name
    const type = child?.props?.type
    console.log('resolve.items', type)
    if (type === 'Item') {
      return <Item {...child.props} i={i} />
    }
  })

  return (
    <div className={props.className}>
      {resolved}
    </div>
  )
}

export default Items
