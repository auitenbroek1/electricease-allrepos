import { createContext } from 'react'

export const EntityContext = createContext({
  selected_entity: {},
  set_selected_entity: (entity: object) => {},
})
