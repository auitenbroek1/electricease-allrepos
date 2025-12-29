import { useEffect, useState } from 'react'

import { generatePaginationRange } from '@/helpers/pagination'

export interface PaginationItemProps {
  current: boolean;
  disabled: boolean;
  page: number;
  title: string;
}

type PaginationType = {
  previous: PaginationItemProps;
  current: PaginationItemProps;
  next: PaginationItemProps;
  range: [PaginationItemProps];
}

export const usePagination = (currentPage = 1, lastPage = 1) => {
  const [pagination, setPagination] = useState<PaginationType|null>(null)

  useEffect(() => {

    const previous = {
      current: false,
      disabled: currentPage <= 1,
      page: currentPage - 1,
      title: 'Previous',
    }

    const current = {
      current: true,
      disabled: true,
      page: currentPage,
      title: `${currentPage} / ${lastPage}`,
    }

    const next = {
      current: false,
      disabled: currentPage >= lastPage,
      page: currentPage + 1,
      title: 'Next',
    }

    const rangeItems: any = generatePaginationRange(
      currentPage,
      lastPage,
    )

    // console.log(rangeItems, currentPage, lastPage)

    //

    /*
    const _min = 1
    const _max = 20
    for (let i = _min; i <= _max; i++) {
      for (let j = 1; j <= i; j++) {
        // console.log(j, i, pagination11(j, i))
        console.log(j, i, pagination22(j, i))
      }
    }
    */

    //

    const range = rangeItems.map((item: any) => {
      const button = {
        current: item === currentPage,
        disabled: typeof item !== 'number',
        page: item,
        title: item,
      }

      return button
    })

    const pagination = {
      previous,
      current,
      next,
      range,
    }

    setPagination(pagination)
  }, [currentPage, lastPage])

  return pagination
}
