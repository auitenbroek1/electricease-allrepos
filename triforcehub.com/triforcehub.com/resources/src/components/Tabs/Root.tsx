import { useEffect, useState } from 'react'

import IndexContext from './Context'
import Group from './Group'

const Root = (props: any) => {
  const {
    children,
    data = [],
  } = props

  const [index, setIndex] = useState(0)

  const contextValue = {index, setIndex}

  useEffect(() => {
    console.log('debug.tabs', data.length, index)
    if (data.length && index) {
      if (index > data.length - 1) {
        setIndex(data.length - 1)
      }
    }
  }, [data])

  //

  const resolve = (children: any, slot: any) => {
    return (typeof children === 'function' ? children({ ...slot, test: 'test' }) : children)
  }

  return (
    <IndexContext.Provider value={contextValue}>
      <Group {...props}  />
    </IndexContext.Provider>
  )
}

export default Root
