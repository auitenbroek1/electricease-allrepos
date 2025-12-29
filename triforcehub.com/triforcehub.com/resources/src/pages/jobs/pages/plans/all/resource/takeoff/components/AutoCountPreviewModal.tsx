import { AutoCountModal } from './AutoCountModal'

export const AutoCountPreviewModal = (props: any) => {
  const {
    open,
    setOpen,
    template,
    submit,
    on_cancel,
    modal_1_page_loading,
    modal_1_pages_to_load,
  } = props

  const handle_cancel = () => {
    console.log(`debug.test`, `modal.cancel`)
    setOpen(false)
    on_cancel()
  }

  return (
    <AutoCountModal
      open={open}
      on_submit={submit}
      on_cancel={handle_cancel}
      title={`Confirm`}
    >
      <div className="absolute inset-0">
        <div className="relative h-full flex-1 overflow-y-auto">
          <div className="p-4">
            <div className={`w-auto text-center`}>
              {template ? (
                <div className="">
                  <div className="flex justify-center items-center">
                    <span style={{ fontSize: `large`, marginRight: 45 }}>
                      Are you sure you want to proceed with auto count?
                    </span>
                    <img
                      alt="symbol(template)"
                      width={`140px`}
                      src={template}
                    />
                  </div>
                  {modal_1_pages_to_load > 0 && (
                    <div>
                      Loading: {modal_1_page_loading} /{` `}
                      {modal_1_pages_to_load}
                    </div>
                  )}
                </div>
              ) : (
                `Loading...`
              )}
            </div>
          </div>
        </div>
      </div>
    </AutoCountModal>
  )
}
