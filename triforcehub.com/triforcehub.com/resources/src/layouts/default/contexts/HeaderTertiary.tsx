import { createContext, useState } from 'react'

export const HeaderTertiaryContext = createContext({
  items: [],
  setItems: (items: any) => {},
})

export const HeaderTertiaryContextProvider = (props: any) => {
  const { children } = props

  const [items, setItems] = useState([])

  const value = {
    items,
    setItems,
  }

  // console.log(`debug.render`)

  return <HeaderTertiaryContext.Provider value={value}>{children}</HeaderTertiaryContext.Provider>
}
