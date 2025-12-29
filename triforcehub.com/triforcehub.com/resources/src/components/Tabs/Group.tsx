import { Children } from 'react'

import Items from './Items'
import Panels from './Panels'

const Group = (props: any) => {
  // console.log('resolve.group', typeof props.children, props)

  const resolve = (children: any, slot: any) => {
    return (typeof children === 'function' ? children({ ...slot, test: 'test' }) : children)
  }

  const resolved = Children.map(props.children, (child) => {
    // const type = child?.type?.name
    const type = child?.props?.type
    console.log('resolve.group', type)
    if (type === 'Items') {
      return <Items {...child.props} />
    }
    if (type === 'Panels') {
      return <Panels {...child.props} />
    }
  })

  return (
    <div className={props.className}>
      {resolved}
    </div>
  )
}

export default Group
