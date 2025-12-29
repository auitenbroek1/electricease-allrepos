import { createContext, useState } from 'react'

export const HeaderPrimaryContext = createContext({
  items: [],
  setItems: (items: any) => {},
})

export const HeaderPrimaryContextProvider = (props: any) => {
  const { children } = props

  const [items, setItems] = useState([])

  const value = {
    items,
    setItems,
  }

  // console.log(`debug.render`)

  return <HeaderPrimaryContext.Provider value={value}>{children}</HeaderPrimaryContext.Provider>
}
