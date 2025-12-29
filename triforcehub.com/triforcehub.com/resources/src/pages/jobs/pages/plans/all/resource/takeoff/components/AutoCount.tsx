// TODO: auto-count

import { useContext } from 'react'

import { EntityContext } from '../contexts/EntityContext'
import { useAutoCount } from '../hooks/useAutoCount'

import { AutoCountPreviewModal } from './AutoCountPreviewModal'
import { AutoCountResultsModal } from './AutoCountResultsModal'

export const AutoCount = (props: any) => {
  const { instanceRef } = props

  const { selected_entity }: any = useContext(EntityContext)

  const {
    showModal2AC,
    handleModal2SubmitAC,
    autoCountTemplate,
    handleModal1SubmitAC,
    setShowModal2AC,
    setShowModal1AC,
    showModal1AC,
    template,
    on_cancel,
    //
    selectionRect,
    modal_1_page_loading,
    modal_1_pages_to_load,
  } = useAutoCount(instanceRef, selected_entity)

  return (
    <>
      {false && (
        <div>
          <div>
            selected_entity: {JSON.stringify(selected_entity?.annotation)}
          </div>
          <div>selected_entity: {JSON.stringify(selected_entity?.entity)}</div>
          <div>selectionRect: {JSON.stringify(selectionRect)}</div>
          <div>template: {template}</div>
          <div>autoCountTemplate: {JSON.stringify(autoCountTemplate)}</div>
        </div>
      )}

      <AutoCountPreviewModal
        open={showModal1AC}
        setOpen={setShowModal1AC}
        submit={handleModal1SubmitAC}
        on_cancel={on_cancel}
        template={template}
        modal_1_page_loading={modal_1_page_loading}
        modal_1_pages_to_load={modal_1_pages_to_load}
      />

      <AutoCountResultsModal
        open={showModal2AC}
        setOpen={setShowModal2AC}
        submit={handleModal2SubmitAC}
        on_cancel={on_cancel}
        template={autoCountTemplate}
      />
    </>
  )
}
