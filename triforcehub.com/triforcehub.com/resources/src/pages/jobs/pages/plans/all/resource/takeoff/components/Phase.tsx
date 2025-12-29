import { useState } from 'react'

import { Assembly } from './Assembly'
import { Part } from './Part'

export const Phase = (props: any) => {
  const { phase } = props

  const [isOpen, setIsOpen] = useState(false)

  return (
    <div>
      <div
        className="space-y-4"
        key={phase.uuid}
      >
        <div className={`flex space-x-2 ${isOpen ? `font-bold` : ``}`}>
          <button
            className={`cursor-pointer`}
            onClick={() => setIsOpen(!isOpen)}
          >
            {phase.name}
          </button>
        </div>
        <div className={`${isOpen ? `` : `hidden`}`}>
          <div className="space-y-4">
            {phase.assemblies.map((assembly: any, assembly_index: any) => {
              if (!assembly.enabled) return
              return (
                <Assembly
                  assembly={assembly}
                  key={assembly_index}
                />
              )
            })}
            {phase.parts.map((part: any, part_index: any) => {
              if (!part.enabled) return
              return (
                <Part
                  key={part_index}
                  part={part}
                />
              )
            })}
          </div>
        </div>
      </div>
    </div>
  )
}
