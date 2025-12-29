import { useContext } from 'react'
import { Collection } from './Context'

import { Pagination as Deprecated } from '@/components'

export const Pagination = (props: any) => {
  const { children, showPageDropdown = true } = props

  const { meta, setPage, size, setSize }: any = useContext(Collection)

  return (
    <div className="w-full">
      <Deprecated
        currentPage={meta?.current_page}
        lastPage={meta?.last_page}
        handlePageChange={(i: number) => setPage(i)}
        size={size}
        setSize={setSize}
        showPageDropdown={showPageDropdown}
      />
    </div>
  )
}
