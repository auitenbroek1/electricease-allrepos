import { useContext } from 'react'

import IndexContext from './Context'

const Panel = (props: any) => {
  const {
    children,
    className,
    onClick,
    i,
  } = props

  const { index, setIndex } = useContext(IndexContext)

  // console.log('resolve.context', index)
  // console.log('resolve.panel', typeof children, props)

  const resolve = (children: any, slot: any) => {
    return (typeof children === 'function' ? children({ ...slot, test: 'test' }) : children)
  }

  if (i !== index) return null

  const resolved = resolve(children, { selected: i === index })

  return (
    <>
      {resolved}
    </>
  )
}

export default Panel
