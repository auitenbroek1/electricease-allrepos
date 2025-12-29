import { usePagination } from '@/hooks/pagination'
import Trigger from '../Trigger'

import { ArrowLongLeftIcon, ArrowLongRightIcon } from '@/components/Icons'

const Pagination = (props: any) => {
  const {
    currentPage = 1,
    lastPage = 1,
    handlePageChange,
    size,
    setSize,
    showPageDropdown = true,
  } = props

  // const pagination = null
  const pagination = usePagination(currentPage, lastPage)

  if (!pagination) return null
  if (currentPage !== pagination.current.page) return null

  // console.log('render', JSON.stringify(props), JSON.stringify(pagination.current.title))

  return (
    <nav className="flex items-center justify-between border-t border-gray-200 px-4 sm:px-0">
      <div className="-mt-px flex w-0 flex-1">
        <div className={`pt-4`}>
          <Trigger.Anchor
            disabled={pagination.previous.disabled}
            icon={
              <div className="h-5 w-5">
                <ArrowLongLeftIcon />
              </div>
            }
            onClick={() => handlePageChange(pagination.previous.page)}
            title={pagination.previous.title}
          />
        </div>
      </div>
      <div className={`flex space-x-8`}>
        {showPageDropdown ? (
          <div
            className={`flex items-center space-x-2 pt-4 text-sm text-gray-500`}
          >
            <span>Show</span>
            <select
              onChange={(event) => setSize(event?.target.value)}
              value={size}
            >
              <option value={10}>10</option>
              <option value={25}>25</option>
              <option value={50}>50</option>
              <option value={100}>100</option>
            </select>
            <span>per page</span>
          </div>
        ) : null}
        <div className={`flex pt-4`}>
          {pagination.range.map((item, index) => (
            <span
              className={`min-w-9`}
              key={index}
            >
              <Trigger.Anchor
                active={item.current}
                disabled={item.disabled}
                onClick={() => handlePageChange(item.page)}
                title={item.title}
              />
            </span>
          ))}
        </div>
      </div>
      <div className="-mt-px flex w-0 flex-1 justify-end">
        <div className={`pt-4`}>
          <Trigger.Anchor
            disabled={pagination.next.disabled}
            iconAfter={
              <div className="h-5 w-5">
                <ArrowLongRightIcon />
              </div>
            }
            onClick={() => handlePageChange(pagination.next.page)}
            title={pagination.next.title}
          />
        </div>
      </div>
    </nav>
  )
}

// import React from 'react'
// export default React.memo(Pagination)

export default Pagination
