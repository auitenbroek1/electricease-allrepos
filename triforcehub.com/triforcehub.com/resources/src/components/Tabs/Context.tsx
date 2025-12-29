import { createContext } from 'react'

const Context = createContext({
  index: {},
  setIndex: (index: number) => { },
});

export default Context
