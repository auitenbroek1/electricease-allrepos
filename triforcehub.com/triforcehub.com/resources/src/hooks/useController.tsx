import { useRef } from 'react'

import axios from 'axios'
import toast from 'react-hot-toast'

export const useController = (props: any) => {
  const { endpoint } = props

  const controllerRef = useRef<any>()

  //

  const dispatch = async (action: any, showNotifications = false) => {
    return new Promise(async (resolve, reject) => {
      try {
        const response = await action()
        const data = response?.data?.data
        showNotifications && toast.success(`Saved!`)
        return resolve(data)
      } catch (error: any) {
        const errors = error?.response?.data?.errors
        if (errors) {
          return reject(errors)
        }
        if (error?.code === `ERR_CANCELED`) {
          return resolve({})
        }
        toast.error(error.message)
        return reject({ '*': [error.message] })
      }
    })
  }

  //

  const index = async ({ size = 6 }) => {
    return dispatch(async () => await axios.get(`${endpoint}?size=${size}`))
  }

  const store = async (payload: any) => {
    return dispatch(async () => await axios.post(endpoint, payload), true)
  }

  const show = async (id: any) => {
    controllerRef.current = new AbortController()

    return dispatch(
      async () =>
        await axios.request({
          method: `GET`,
          url: `${endpoint}/${id}`,
          signal: controllerRef.current.signal,
        }),
    )
  }

  const update = async (id: any, payload: any) => {
    return dispatch(
      async () => await axios.patch(`${endpoint}/${id}`, payload),
      true,
    )
  }

  const destroy = async (id: any) => {
    return dispatch(async () => await axios.delete(`${endpoint}/${id}`), true)
  }

  //

  const save = async (id: any, payload: any) => {
    if (id) return update(id, payload)
    return store(payload)
  }

  const cancel = () => {
    controllerRef?.current?.abort()
  }

  //

  return {
    index,
    store,
    show,
    update,
    destroy,

    save,
    cancel,
  }
}
