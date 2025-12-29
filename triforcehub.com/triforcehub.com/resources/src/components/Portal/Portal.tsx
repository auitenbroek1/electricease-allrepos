import { useState, useEffect } from 'react'
import { createPortal } from 'react-dom'

export const Portal = (props: any) => {
  const { id, children } = props

  const [hasMounted, setHasMounted] = useState(false)

  useEffect(() => {
    setHasMounted(true)
  }, [])

  if (!hasMounted) {
    return null
  }

  const element: any = document.querySelector(`#${id}`)

  if (!element) return null

  return createPortal(children, element)
}
