import { createContext } from 'react'

const Context = createContext({
  member: {},
  setMember: (member: object) => {},
})

export default Context
