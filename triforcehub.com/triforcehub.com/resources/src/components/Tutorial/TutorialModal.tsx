import { Dialog, Transition } from '@headlessui/react'
import { Fragment, useState } from 'react'
import { Form } from '../Form'

import { PlayCircleIcon } from '@/components/Icons'

const Link = (props: any) => {
  const { tutorial, onClick } = props

  let color = `text-emerald-600`
  if (tutorial.color === `red`) color = `text-red-500`
  if (tutorial.color === `yellow`) color = `text-yellow-500`

  return (
    <button
      className={`flex items-center space-x-2 text-sm ${color}`}
      onClick={onClick}
      type="button"
    >
      <div className={`h-5 w-5`}>
        <PlayCircleIcon />
      </div>
      <div>{tutorial.name ?? `Watch a Tutorial`}</div>
    </button>
  )
}

export default function TutorialModal(props: any) {
  const { children, media: custom_media, tutorials } = props

  const [isOpen, setIsOpen] = useState(false)
  const [media, setMedia] = useState(null)

  function closeModal() {
    setIsOpen(false)
    setMedia(null)
  }

  function openModal(media: any) {
    setIsOpen(true)
    setMedia(media)
  }

  if (!tutorials) return null

  return (
    <>
      {children ? (
        <div onClick={() => openModal(custom_media)}>{children}</div>
      ) : (
        <div className={`flex space-x-4`}>
          {tutorials.map((tutorial: any, index: number) => (
            <Link
              key={index}
              onClick={() => openModal(tutorial.media)}
              tutorial={tutorial}
            />
          ))}
        </div>
      )}
      <Transition
        appear
        show={isOpen}
        as={Fragment}
      >
        <Dialog
          as="div"
          className="fixed inset-0 z-10 overflow-y-auto"
          onClose={closeModal}
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
              <Dialog.Overlay className="fixed inset-0 bg-black bg-opacity-50 transition-opacity" />
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
              <div className="my-8 inline-block w-full max-w-3xl transform overflow-hidden rounded-2xl bg-white p-6 text-left align-middle shadow-xl transition-all">
                {media && (
                  <div>
                    <video
                      autoPlay
                      className={`w-full`}
                      controls
                      playsInline
                    >
                      {/* <source src="/media/cc0-videos/flower.webm" type="video/webm"> */}
                      <source
                        src={`https://s3.us-east-2.amazonaws.com/media.triforcehub.com/${media}`}
                        type="video/mp4"
                      />
                    </video>
                  </div>
                )}

                <div className={`mt-4 flex justify-center`}>
                  <Form.Button onClick={closeModal}>
                    Got it, thanks!
                  </Form.Button>
                </div>
              </div>
            </Transition.Child>
          </div>
        </Dialog>
      </Transition>
    </>
  )
}
