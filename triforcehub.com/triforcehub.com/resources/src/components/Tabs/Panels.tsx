import { Children } from 'react'

import Panel from './Panel'

const Panels = (props: any) => {
  // console.log('resolve.panels', typeof props.children, props)

  const resolved = Children.map(props.children, (child, i) => {
    // const type = child?.type?.name
    const type = child?.props?.type
    console.log('resolve.panels', type)
    if (type === 'Panel') {
      return <Panel {...child.props} i={i} />
    }
  })

  return (
    <div className={props.className}>
      {resolved}
    </div>
  )
}

export default Panels
