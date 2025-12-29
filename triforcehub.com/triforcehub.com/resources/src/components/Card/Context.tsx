import { createContext, useContext, useState } from 'react'

const CardContext = createContext<any>(null)

const CardProvider = (props: any) => {
  const { children, ...initial } = props

  const [open, setOpen] = useState<any>(initial?.open ?? true)

  const value = {
    open,
    setOpen,
  }

  return <CardContext.Provider value={value}>{children}</CardContext.Provider>
}

const useCard = () => {
  return useContext(CardContext)
}

export { CardProvider, useCard }
