import { useContext, useEffect, useState } from 'react'

import {
  Table,
  TableHeader,
  TableHeaderRow,
  TableHeaderCell,
  TableBody,
  TableBodyRow,
  TableBodyCell,
} from '@/components'

import { Collection, Item } from './Context'
import { ChevronDownIcon, ChevronRightIcon } from '@/components/Icons'

const ParentRow = (props: any) => {
  const {
    columns,
    item,
    handleSelectChange,
    isSelected,
    useClickToSelect,
    handleDoubleClick,
  } = props

  const [isOpen, setIsOpen] = useState(false)

  const handleToggleClick = (event: any) => {
    event.stopPropagation()
    setIsOpen(!isOpen)
  }

  const internalDoubleClick = (id: any) => {
    if (handleDoubleClick) {
      handleDoubleClick(id)
    }
  }

  const cells = (input: any, child = false) =>
    columns.map((column: any, column_index: any) => {
      const value = input[column.accessor] ?? null
      // console.log('cell', value)

      const render = child ? column.renderChild ?? column.render : column.render
      // console.log(render)

      let cell = render ? render({ value, data: child ? input : item }) : value
      // console.log(`debug.cell`, typeof render, cell)

      if (column.id === `toggle`) {
        if (child) {
          cell = null
        } else {
          const disabled = input.children.length === 0
          cell = disabled ? (
            <button
              className={`
                group
                -m-2
                flex
                h-9
                w-9
                items-center
                justify-center
                rounded-full
                p-2
              `}
              disabled={true}
            >
              <div className={`h-4 w-4 text-gray-300`}>
                <ChevronRightIcon />
              </div>
            </button>
          ) : (
            <button
              className={`
                group
                -m-2
                flex
                h-9
                w-9
                items-center
                justify-center
                rounded-full
                p-2
                hover:bg-blue-50
                focus:outline-none
              `}
              onClick={handleToggleClick}
              type={`button`}
            >
              {isOpen ? (
                <div
                  className={`h-4 w-4 text-blue-600 group-hover:text-blue-700`}
                >
                  <ChevronDownIcon />
                </div>
              ) : (
                <div
                  className={`h-4 w-4 text-blue-600 group-hover:text-blue-700`}
                >
                  <ChevronRightIcon />
                </div>
              )}
            </button>
          )
        }
      }

      if (column.id === `selected`) {
        const checked: any = isSelected(value)
        cell = (
          <input
            checked={checked}
            className={`pointer-events-none cursor-pointer`}
            type={`checkbox`}
            onChange={(event) => handleSelectChange(event, value)}
          />
        )
      }

      return <TableBodyCell key={column_index}>{cell}</TableBodyCell>
    })

  return (
    <TableBody>
      <TableBodyRow
        onClick={(event: any) =>
          useClickToSelect && handleSelectChange(event, item.id)
        }
        onDoubleClick={(event: any) => internalDoubleClick(item.id)}
      >
        <Item.Provider value={item}>{cells(item, false)}</Item.Provider>
      </TableBodyRow>
      {isOpen &&
        item.children?.map((child: any) => (
          <TableBodyRow
            key={child.uuid}
            onClick={(event: any) =>
              useClickToSelect && handleSelectChange(event, child.id)
            }
            onDoubleClick={(event: any) => internalDoubleClick(child.id)}
          >
            <Item.Provider value={child}>{cells(child, true)}</Item.Provider>
          </TableBodyRow>
        ))}
    </TableBody>
  )
}

export const Data = (props: any) => {
  const { columns, onSelectChange, handleDoubleClick } = props

  const { data, selected, setSelected }: any = useContext(Collection)

  useEffect(() => {
    onSelectChange && onSelectChange(selected)
  }, [selected])

  const isSelected = (id: number) => {
    const match = selected.find((item: any) => item === id)
    return match ? true : false
  }

  const handleSelectChange = (event: any, id: any) => {
    console.log(`debug.select.click`, id)
    const checked = event.target.checked

    setSelected((previous: any) => {
      const match = previous.find((item: any) => item === id)
      if (match) {
        const filtered = previous.filter((item: any) => item !== id)
        console.log(`debug.select`, filtered)
        return filtered
      }

      const items = [...previous, id]
      console.log(`debug.select`, items)
      return items
    })
  }

  const header = columns.map((column: any, column_index: any) => {
    const cell = column.name ?? column.Header

    return <TableHeaderCell key={column_index}>{cell}</TableHeaderCell>
  })

  const body = data.map((item: any, row_index: any) => {
    return (
      <ParentRow
        columns={columns}
        item={item}
        key={row_index}
        handleSelectChange={handleSelectChange}
        isSelected={isSelected}
        useClickToSelect={onSelectChange ? true : false}
        handleDoubleClick={handleDoubleClick}
      />
    )
  })

  return (
    <Table>
      <colgroup>
        {columns.map((column: any, index: number) => (
          <col
            key={index}
            width={column.width ?? 1}
          />
        ))}
      </colgroup>
      <TableHeader>
        <TableHeaderRow>{header}</TableHeaderRow>
      </TableHeader>
      {body}
    </Table>
  )
}
