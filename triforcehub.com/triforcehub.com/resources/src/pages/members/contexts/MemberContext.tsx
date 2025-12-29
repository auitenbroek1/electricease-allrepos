import { createContext, useContext, useEffect, useState } from 'react'
import { useParams } from 'react-router-dom'

import { useController } from '@/hooks'

const MemberContext = createContext<any>({
  member: {},
  setMember: () => {},
  reloadMember: () => {},
})

const MemberProvider = (props: any) => {
  const { children, id: prop_id } = props

  const { id: param_id } = useParams()
  const id = prop_id ?? param_id

  const [member, setMember]: any = useState<any>()

  const { show, cancel } = useController({ endpoint: `/api/members` })

  const reloadMember = () => {
    if (!id) return

    show(id)
      .then((data) => setMember(data))
      .catch((errors) => console.log(errors))
  }

  useEffect(() => {
    console.log(`here!!!`, id)

    reloadMember()

    return () => {
      console.log(`here unmount!!!`, id)
      cancel()
    }
  }, [id])

  if (!member) return null
  if (!member.id) return null

  const value = {
    member,
    setMember,
    reloadMember,
  }

  return (
    <MemberContext.Provider value={value}>{children}</MemberContext.Provider>
  )
}

const useMemberContext = () => {
  return useContext(MemberContext)
}

export { MemberProvider, useMemberContext }
