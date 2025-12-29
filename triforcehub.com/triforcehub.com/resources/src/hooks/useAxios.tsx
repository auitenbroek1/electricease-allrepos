import axios from 'axios'
import { useEffect, useState } from 'react'

export const useAxios = (request: any, fingerprint = `default`) => {
  const [data, setData] = useState<any>(null)
  const error = null
  const loaded = null
  // const [error, setError] = useState(``)
  // const [loaded, setLoaded] = useState(false)

  const prefix = `debug.axios.${fingerprint}`

  console.log(`${prefix}.render`, request.url, {
    data: data?.id,
    error,
    loaded,
  })

  useEffect(() => {
    const controller = new AbortController()
    ;(async () => {
      console.log(`${prefix}.effect`, request)
      try {
        const response = await axios.request({
          signal: controller.signal,
          ...request,
        })
        console.log(`${prefix}.response`, response.data.data)
        // setTimeout(() => {
        setData(response.data.data)
        // }, 2000)
      } catch (error) {
        console.log(`${prefix}.error`, error)
        // setError(error.message)
      } finally {
        console.log(`${prefix}.finally`)
        // setLoaded(true)
      }
    })()

    return () => {
      console.log(`${prefix}.unmount`)
      controller.abort()
    }
  }, [request.url])

  return { data, error, loaded }
}
