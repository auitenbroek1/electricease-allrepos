import axios from 'axios'

import { createContext, useContext } from 'react'
import { useQuery } from '@tanstack/react-query'

const AuthContext = createContext<any>(null)

const AuthProvider = (props: any) => {
  const { children } = props

  const {
    data: user,
    isError,
    status,
    refetch,
  }: any = useQuery(
    [`/api/profile`],
    async () => {
      const response = await axios.get(`/api/profile`)
      return response.data.data
    },
    {
      cacheTime: 0,
      retry: false,
    },
  )

  console.log(`debug.auth`, { status, user })

  if (isError) return null

  if (!user) return null
  if (!user.id) return null

  const value = {
    user,
    refetch,
  }

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>
}

const useAuth = () => {
  return useContext(AuthContext)
}

export { AuthProvider, useAuth }
