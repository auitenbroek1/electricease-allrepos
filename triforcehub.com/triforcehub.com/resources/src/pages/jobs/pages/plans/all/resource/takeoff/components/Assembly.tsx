import { useContext, useState } from 'react'

import { EntityContext } from '../contexts/EntityContext'

import { AnnotationModal } from './AnnotationModal'

export const Assembly = (props: any) => {
  const { assembly } = props

  const entity = {
    singular: `assembly`,
    plural: `assemblies`,
    ...assembly,
  }

  const { selected_entity }: any = useContext(EntityContext)

  const selected = selected_entity?.entity.uuid === entity.uuid

  //

  const [showModal, setShowModal] = useState(false)

  const handleSelect = () => {
    setShowModal(true)
  }

  const onSave = async () => {
    setShowModal(false)
  }

  //

  return (
    <div>
      <button
        key={assembly.uuid}
        onClick={handleSelect}
        className={`block ${selected ? `` : `opacity-50`} w-full border-t border-t-gray-300 pt-4`}
        type={`button`}
      >
        <div className="flex items-center space-x-2 text-left text-sm">
          <div>
            <div
              className="h-3 w-3 rounded-full"
              style={{ backgroundColor: entity.annotation_color }}
            ></div>
          </div>
          <div>
            <div className="space-x-2">
              {assembly.reference.categories.map(
                (category: any, index: number) => (
                  <div
                    className="text-xs text-gray-500"
                    key={index}
                  >
                    {category.name}
                  </div>
                ),
              )}
            </div>
            <div className="">{assembly.reference.name}</div>
            <div className="text-xs text-gray-500">
              Quantity: {assembly.quantity_digital}
            </div>
          </div>
        </div>
      </button>
      {showModal && (
        <AnnotationModal
          entity={entity}
          onSave={onSave}
          onCancel={() => setShowModal(false)}
        />
      )}
    </div>
  )
}
