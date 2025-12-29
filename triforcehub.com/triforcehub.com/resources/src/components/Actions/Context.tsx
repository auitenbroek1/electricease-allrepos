import { createContext } from 'react'

const Context = createContext({
  triggerElement: null,
  setTriggerElement: (element: any) => { },

  portalElement: null,
  setPortalElement: (element: any) => { },
});

export default Context
