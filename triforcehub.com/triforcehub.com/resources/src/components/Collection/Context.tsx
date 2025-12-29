import { createContext } from 'react'

export const Collection = createContext({
  data: null,
  setData: (data: any) => {},

  meta: null,
  setMeta: (meta: any) => {},

  page: null,
  setPage: (page: any) => {},

  q: null,
  setQ: (q: any) => {},

  size: null,
  setSize: (size: any) => {},

  categories: null,
  setCategories: (categories: any) => {},

  favorites: null,
  setFavorites: (favorites: any) => {},
})

export const Item = createContext({
  data: null,
})
