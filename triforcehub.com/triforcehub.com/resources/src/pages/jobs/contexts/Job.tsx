import { createContext } from 'react'

const Context = createContext({
  job: {},
  setJob: (job: object) => { },
});

export default Context
