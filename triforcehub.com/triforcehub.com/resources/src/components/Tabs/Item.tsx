import { useContext } from 'react'

import IndexContext from './Context'

const Item = (props: any) => {
  const { children, className, onClick, i } = props

  const { index, setIndex } = useContext(IndexContext)

  // console.log('resolve.context', index)
  // console.log('resolve.item', typeof children, props)

  const resolve = (children: any, slot: any) => {
    return typeof children === `function`
      ? children({ ...slot, test: `test` })
      : children
  }

  const resolved = resolve(children, { selected: i === index })

  return (
    <>
      <button
        className={className}
        onClick={() => setIndex(i)}
        type={`button`}
      >
        {resolved}
      </button>
    </>
  )
}

export default Item
