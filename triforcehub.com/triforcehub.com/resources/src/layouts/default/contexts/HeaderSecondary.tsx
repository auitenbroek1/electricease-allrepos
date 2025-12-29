import { createContext, useState } from 'react'

export const HeaderSecondaryContext = createContext({
  heading: ``,
  setHeading: (title: string) => {},

  items: [],
  setItems: (items: any) => {},

  tutorials: [],
  setTutorials: (tutorials: any) => {},
})

export const HeaderSecondaryContextProvider = (props: any) => {
  const { children } = props

  const [heading, setHeading] = useState(`state`)
  const [items, setItems] = useState([])
  const [tutorials, setTutorials] = useState([])

  const value = {
    heading,
    setHeading,

    items,
    setItems,

    tutorials,
    setTutorials,
  }

  return (
    <HeaderSecondaryContext.Provider value={value}>
      {children}
    </HeaderSecondaryContext.Provider>
  )
}
