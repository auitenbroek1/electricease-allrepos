import { useEffect, useState } from 'react'
import { useMemberContext } from '../contexts/MemberContext'

import axios from 'axios'
import { Form } from '@/components/Form'
import toast from 'react-hot-toast'

export const JumpStart = () => {
  const [items, setItems] = useState<any>([])
  const [isSettingUp, setIsSettingUp] = useState<any>(false)

  const { member } = useMemberContext()

  const setup = async (member_id: any) => {
    if (!member_id) return
    try {
      setIsSettingUp(true)
      toast.success(`Processing...`)
      const response = await axios.post(`/api/members/${member_id}/jumpstart`)
      console.log(response.data)
    } catch (exception: any) {
      toast.error(`Oops!`)
    }
  }

  const refresh = async (member_id: any) => {
    if (!member_id) return
    try {
      toast.success(`Processing...`)
      const response = await axios.get(`/api/members/${member_id}/jumpstart`)
      console.log(response.data)
      setItems(response.data)
    } catch (exception: any) {
      toast.error(`Oops!`)
    }
  }

  const reset = async (member_id: any) => {
    if (!member_id) return
    try {
      toast.success(`Processing...`)
      const response = await axios.delete(`/api/members/${member_id}/jumpstart`)
      console.log(response.data)
      setItems(response.data)
    } catch (exception: any) {
      toast.error(`Oops!`)
    }
  }

  useEffect(() => {
    refresh(member.id)
  }, [member?.id])

  if (!member) return null

  return (
    <div className="space-y-4">
      {items.map((item: any) => (
        <div key={item.uuid}>{item.name}</div>
      ))}
      <div className="flex space-x-4">
        {items.length === 0 && !isSettingUp && (
          <Form.Button onClick={() => setup(member.id)}>Setup</Form.Button>
        )}
        {(items.length > 0 || isSettingUp) && (
          <Form.Button
            onClick={() => refresh(member.id)}
            type={`secondary`}
          >
            Refresh
          </Form.Button>
        )}
        {items.length > 0 && (
          <Form.Button
            onClick={() => reset(member.id)}
            type={`secondary`}
          >
            Reset
          </Form.Button>
        )}
      </div>
    </div>
  )
}
