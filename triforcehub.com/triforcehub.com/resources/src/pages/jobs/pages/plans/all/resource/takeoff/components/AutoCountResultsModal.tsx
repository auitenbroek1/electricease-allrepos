// TODO: auto-count

import { useState } from 'react'
import { CrossIcon } from '@/components/Icons/CrossIcon'

import { AutoCountModal } from './AutoCountModal'

export const AutoCountResultsModal = (props: any) => {
  const { open, setOpen, template, submit, on_cancel } = props

  const [selectedTemplateIndex, setSelectedTemplateIndex] = useState<number[]>(
    template ? template.map((_: any, index: any) => index) : [],
  )

  const toggleSelectedTemplate = (index: number) => {
    if (selectedTemplateIndex?.includes(index)) {
      setSelectedTemplateIndex(selectedTemplateIndex.filter((i) => i !== index))
    } else {
      setSelectedTemplateIndex([...selectedTemplateIndex, index])
    }
  }

  const selectAll = () => {
    if (!Array.isArray(template)) return
    if (template.length === 0) return
    const selected = template.map((_, index: number) => {
      return index
    })
    setSelectedTemplateIndex(selected)
  }

  console.log(`debug.select`, { template, selectedTemplateIndex })

  const on_submit = () => {
    console.log(`debug.test`, `modal.submit`)
    submit(selectedTemplateIndex)
    setSelectedTemplateIndex([])
  }

  const handle_cancel = () => {
    console.log(`debug.test`, `modal.cancel`)
    setOpen(false)
    setSelectedTemplateIndex([])
    on_cancel()
  }

  return (
    <AutoCountModal
      open={open}
      on_submit={on_submit}
      on_cancel={handle_cancel}
      title={`Results`}
    >
      <div className="absolute inset-0">
        <div className="relative h-full flex-1 overflow-y-auto">
          <div className="px-6 pt-3 pb-3">
            <div className="flex space-x-4 items-center pb-2">
              <h3 className={`w-auto text-start`}>
                Matches (
                {selectedTemplateIndex ? selectedTemplateIndex.length : 0})
              </h3>
              <button
                className="text-blue-600"
                onClick={selectAll}
                type="button"
              >
                Select All
              </button>
            </div>
            {selectedTemplateIndex?.length > 0 && (
              <div
                className="flex cursor-pointer pl-4 pt-3 pb-3 pr-4 mb-5 gap-3 bg-red-50"
                onClick={() => setSelectedTemplateIndex([])}
              >
                <div className="">
                  <CrossIcon />
                </div>
                <span>Clear Selection</span>
                <span className="ml-auto">
                  {selectedTemplateIndex?.length} selected
                </span>
              </div>
            )}
            <div className="grid grid-cols-6 gap-4">
              {Array.isArray(template) &&
                template?.map((match, index: number) => (
                  <div
                    key={index}
                    className={`relative aspect-square border p-1 cursor-pointer ${
                      selectedTemplateIndex?.includes(index)
                        ? `border-blue-500`
                        : `border-gray-300`
                    }`}
                    onClick={() => toggleSelectedTemplate(index)}
                  >
                    <div className="flex justify-center items-center h-full w-full">
                      <img
                        alt={`symbol(template-${index + 1})`}
                        className="max-h-full max-w-full"
                        src={match.val}
                      />
                    </div>
                    {` `}
                    <div className="absolute bottom-0 left-0 text-xs bg-black/50 text-white px-1">
                      {match.score ?? 0}%
                    </div>
                  </div>
                ))}
            </div>
          </div>
        </div>
      </div>
    </AutoCountModal>
  )
}
