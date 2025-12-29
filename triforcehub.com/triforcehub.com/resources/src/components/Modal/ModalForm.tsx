import { Dialog, Transition } from '@headlessui/react'
import { Fragment } from 'react'

import { FormButton } from '@/components'

const ModalForm = (props: any) => {
  const { content, description, onClose, onSubmit, open, title } = props

  if (!open) return null

  return (
    <>
      <Transition
        appear
        show={open}
        as={Fragment}
      >
        <Dialog
          as="div"
          className="fixed inset-0 z-10 overflow-y-auto"
          onClose={onClose}
        >
          <div className="min-h-screen px-4 text-center">
            <Transition.Child
              as={Fragment}
              enter="ease-out duration-300"
              enterFrom="opacity-0"
              enterTo="opacity-100"
              leave="ease-in duration-200"
              leaveFrom="opacity-100"
              leaveTo="opacity-0"
            >
              <Dialog.Overlay className="fixed inset-0 bg-black opacity-50" />
            </Transition.Child>

            {/* This element is to trick the browser into centering the modal contents. */}
            <span
              className="inline-block h-screen align-middle"
              aria-hidden="true"
            >
              &#8203;
            </span>
            <Transition.Child
              as={Fragment}
              enter="ease-out duration-300"
              enterFrom="opacity-0 scale-95"
              enterTo="opacity-100 scale-100"
              leave="ease-in duration-200"
              leaveFrom="opacity-100 scale-100"
              leaveTo="opacity-0 scale-95"
            >
              <div className="my-8 inline-block w-full max-w-md transform overflow-hidden rounded-2xl bg-white p-8 text-left align-middle shadow-xl transition-all">
                <div className={`space-y-8`}>
                  <div className={`space-y-2`}>
                    {title && (
                      <Dialog.Title
                        as={`div`}
                        className={`text-lg font-medium leading-none`}
                      >
                        {title}
                      </Dialog.Title>
                    )}

                    {description && (
                      <Dialog.Description
                        as={`div`}
                        className={`text-sm text-gray-500`}
                      >
                        {description}
                      </Dialog.Description>
                    )}
                  </div>

                  {content}

                  <div className="flex items-center justify-center space-x-8">
                    <FormButton
                      onClick={onClose}
                      type={`secondary`}
                    >
                      Cancel
                    </FormButton>
                    <FormButton onClick={onSubmit}>Submit</FormButton>
                  </div>
                </div>
              </div>
            </Transition.Child>
          </div>
        </Dialog>
      </Transition>
    </>
  )
}

export default ModalForm
