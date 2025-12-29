import axios from 'axios'
import { useCallback, useEffect, useState } from 'react'

import { Collection } from './Context'

export const Root = (props: any) => {
  const { children, endpoint } = props

  const [data, setData]: any = useState([])
  const [meta, setMeta]: any = useState({})
  const [page, setPage]: any = useState(1)
  const [q, setQ]: any = useState(``)
  const [categories, setCategories]: any = useState(``)
  const [favorites, setFavorites]: any = useState(false)
  const [selected, setSelected]: any = useState([])
  const [size, setSize]: any = useState(10)
  const [refresh, setRefresh]: any = useState(null)

  const context = {
    data,
    setData,

    meta,
    setMeta,

    page,
    setPage,

    q,
    setQ,

    categories,
    setCategories,

    favorites,
    setFavorites,

    selected,
    setSelected,

    size,
    setSize,

    refresh,
    setRefresh,
  }

  //

  const fetchData = useCallback(
    async (q: any, size: any, page: any, categories: any, favorites: any) => {
      // Set the loading state
      // setLoading(true)
      const params = { q, size, page, categories, favorites }
      const search = new URLSearchParams(params)

      console.log(`fetchData NEW!!!`, endpoint, params)
      console.log(`fetchData NEW!!!`, search.toString())

      try {
        const response = await axios(`${endpoint}?${search}`)

        setData(response.data.data)
        setMeta(response.data.meta)

        if (response.data.meta.current_page > response.data.meta.last_page) {
          setPage(1)
        }
      } catch (error) {
        // setData([])
        // setMeta({})
      }

      // setLoading(false)
    },
    [],
  )

  useEffect(() => {
    fetchData(q, size, page, categories, favorites)
  }, [q, size, page, categories, favorites, refresh])

  //

  return (
    <Collection.Provider value={context}>
      <div className="space-y-4">{children}</div>
    </Collection.Provider>
  )
}
