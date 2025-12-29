import { useContext, useEffect, useState } from 'react'

import { Collection } from '@/components/Collection/Context'

import { Actions } from '@/components'
import { StarOnIcon, StarOffIcon } from '@/components/Icons'

import axios from 'axios'

export const AssemblyFavorite = (props: any) => {
  const { data, disabled = false } = props
  const { setRefresh }: any = useContext(Collection)

  const [checked, setChecked] = useState(data.favorited)
  const [saving, setSaving] = useState(false)

  const handleFavorite = async (item: any) => {
    console.log(`handleFavorite`, item)
    setSaving(true)
    try {
      if (item.favorited) {
        await axios.delete(`/api/assemblies/${item.id}/favorites`, {})
      } else {
        await axios.post(`/api/assemblies/${item.id}/favorites`, {})
      }
      setChecked(!item.favorited)
      setSaving(false)
      setRefresh(Date.now)
    } catch (error) {
      console.log(error)
      setSaving(false)
    }
  }

  useEffect(() => {
    setChecked(data.favorited)
  }, [data.favorited])

  if (saving) {
    return <div></div>
  }

  return (
    <>
      <Actions.Root>
        <Actions.Trigger>
          {checked ? (
            <div
              className={`
                h-5
                w-5
                cursor-pointer
                text-brand-gradient-light
                ${disabled ? `pointer-events-none` : ``}
              `}
              onClick={() => handleFavorite(data)}
            >
              <StarOnIcon />
            </div>
          ) : (
            <div
              className={`
                h-5
                w-5
                cursor-pointer
                text-brand-gradient-light
                ${disabled ? `pointer-events-none` : ``}
              `}
              onClick={() => handleFavorite(data)}
            >
              <StarOffIcon />
            </div>
          )}
        </Actions.Trigger>
      </Actions.Root>
    </>
  )
}
