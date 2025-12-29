import { useCallback, useEffect, useState } from 'react'

const useCollection = (props: any) => {
  const { controller } = props

  const [collection, setCollection] = useState<CollectionData>()
  const [collectionResponse, setCollectionResponse] = useState<any>()

  const [page, setPage] = useState(1)
  const [size, setSize] = useState(6)

  //

  const [q, setQ] = useState(``)

  const handleSearch = (event: any) => {
    setQ(event.target.value)
    setPage(1)
  }

  //

  const [selected, setSelected] = useState<any>([])

  const handleSelectChange = (event: any, diff: any) => {
    const checked = event.target.checked

    setSelected((previous: any) => {
      const match = previous.find((item: any) => item.id === diff.id)
      if (match) {
        const filtered = previous.filter((item: any) => item.id !== diff.id)
        return filtered
      }

      const items = [...previous, diff]
      return items
    })
  }

  const { onSelectChange } = props

  useEffect(() => {
    onSelectChange && onSelectChange(selected)
  }, [selected])

  //

  const query = useCallback(({ q, page, size }: any) => {
    const fetch = async () => {
      const collectionResponse = await controller({ q, page, size })
      setCollectionResponse(collectionResponse)
    }

    fetch()
  }, [])

  useEffect(() => {
    query({ q, page, size })
  }, [q, page, size])

  //

  return {
    collection,
    setCollection,
    collectionResponse,
    setCollectionResponse,
    q,
    setQ,
    page,
    setPage,
    size,
    setSize,
    selected,
    setSelected,
    handleSelectChange,
    handleSearch,
  }
}

export default useCollection
