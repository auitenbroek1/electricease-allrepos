import { Disclosure } from '@headlessui/react'

import { ChevronRightIcon, TrashIcon } from '@/components/Icons'

import { CustomerForm } from './CustomerForm'

import axios from 'axios'
import { useJob } from '@/data/Job'
import { useEffect, useState } from 'react'

export const Customers = (props: any) => {
  const { data } = props

  //

  const [showNew, setShowNew] = useState<any>(false)

  useEffect(() => {
    console.log(`debug.customers`, `close`)
    setShowNew(false)
  }, [data?.length])

  const { reloadJob } = useJob()

  const handleDelete = async (id: number) => {
    console.log(`debug.customers`, `delete`, id)
    await axios.delete(`/api/jobs/customers/${id}`)
    reloadJob()
  }

  //

  if (!data) return null

  return (
    <div
      className="space-y-4"
      key={data.length}
    >
      <div className="border-b pb-4">
        <h3 className="text-lg font-medium leading-none">
          Customer Information
        </h3>
        <p className="mt-1 text-sm text-gray-500">
          Capture information about your customer
        </p>
      </div>
      <div className="space-y-4">
        {data.length === 0 && <CustomerForm />}
        {data.length > 0 && (
          <>
            <div>
              <div className={`-my-4 divide-y`}>
                {data?.map((customer: any) => (
                  <Disclosure key={customer.uuid}>
                    {({ open }) => (
                      <div>
                        <div
                          className={`
                            flex
                            justify-between
                            py-4
                          `}
                        >
                          <Disclosure.Button>
                            <div
                              className={`
                                flex
                                items-center
                                space-x-2
                                text-sm
                                ${
                                  open
                                    ? `text-blue-600`
                                    : `text-gray-500 underline hover:text-blue-600`
                                }
                              `}
                            >
                              <span
                                className={`
                                  leading-none
                                `}
                              >
                                {customer.name}
                              </span>
                              <span
                                className={`
                                  -my-1
                                  flex
                                  h-5
                                  w-5
                                  items-center
                                  justify-center
                                  transition-transform
                                  ${open ? `rotate-90` : ``}
                                `}
                              >
                                <span className={`h-4 w-4`}>
                                  <ChevronRightIcon />
                                </span>
                              </span>
                            </div>
                          </Disclosure.Button>
                          <button
                            className={`h-4 w-4`}
                            onClick={() => handleDelete(customer.id)}
                          >
                            <TrashIcon />
                          </button>
                        </div>
                        <Disclosure.Panel>
                          <div className={`border-t py-6`}>
                            <CustomerForm data={customer} />
                          </div>
                        </Disclosure.Panel>
                      </div>
                    )}
                  </Disclosure>
                ))}
              </div>
            </div>
            <hr />
            {showNew ? (
              <CustomerForm />
            ) : (
              <div className={`space-y-4`}>
                <div className={`text-sm`}>
                  Additional customers can be added if you need to send multiple
                  bids to different customers/general contractors
                </div>
                <button
                  className={`
                    text-sm
                    leading-none
                    text-gray-500
                    underline
                    hover:text-blue-600
                `}
                  onClick={() => setShowNew(true)}
                  type={`button`}
                >
                  + Add Another Customer
                </button>
              </div>
            )}
          </>
        )}
      </div>
    </div>
  )
}
