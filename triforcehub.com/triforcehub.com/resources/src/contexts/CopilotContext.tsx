import { createContext, useContext, useState, useCallback, ReactNode } from 'react'
import { CopilotController, CopilotMessage, CopilotContext as CopilotContextData } from '../api/copilot'

interface CopilotState {
  isOpen: boolean
  messages: CopilotMessage[]
  isLoading: boolean
  error: string | null
  context: CopilotContextData
}

interface CopilotContextValue extends CopilotState {
  togglePanel: () => void
  openPanel: () => void
  closePanel: () => void
  sendMessage: (message: string) => Promise<void>
  clearMessages: () => void
  clearError: () => void
  setContext: (context: CopilotContextData) => void
}

const defaultState: CopilotState = {
  isOpen: false,
  messages: [],
  isLoading: false,
  error: null,
  context: {},
}

const CopilotContext = createContext<CopilotContextValue | undefined>(undefined)

export const CopilotProvider = ({ children }: { children: ReactNode }) => {
  const [state, setState] = useState<CopilotState>(defaultState)

  const togglePanel = useCallback(() => {
    setState(prev => ({ ...prev, isOpen: !prev.isOpen }))
  }, [])

  const openPanel = useCallback(() => {
    setState(prev => ({ ...prev, isOpen: true }))
  }, [])

  const closePanel = useCallback(() => {
    setState(prev => ({ ...prev, isOpen: false }))
  }, [])

  const clearMessages = useCallback(() => {
    setState(prev => ({ ...prev, messages: [], error: null }))
  }, [])

  const clearError = useCallback(() => {
    setState(prev => ({ ...prev, error: null }))
  }, [])

  const setContext = useCallback((context: CopilotContextData) => {
    setState(prev => ({ ...prev, context }))
  }, [])

  const sendMessage = useCallback(async (message: string) => {
    if (!message.trim()) return

    const userMessage: CopilotMessage = { role: 'user', content: message }

    setState(prev => ({
      ...prev,
      messages: [...prev.messages, userMessage],
      isLoading: true,
      error: null,
    }))

    try {
      const response = await CopilotController.chat({
        message,
        context: state.context,
        history: state.messages,
      })

      if (response.success && response.response) {
        const assistantMessage: CopilotMessage = {
          role: 'assistant',
          content: response.response,
        }
        setState(prev => ({
          ...prev,
          messages: [...prev.messages, assistantMessage],
          isLoading: false,
        }))
      } else {
        setState(prev => ({
          ...prev,
          isLoading: false,
          error: response.error || 'Failed to get response',
        }))
      }
    } catch (err) {
      setState(prev => ({
        ...prev,
        isLoading: false,
        error: 'Failed to connect to AI assistant. Please try again.',
      }))
    }
  }, [state.context, state.messages])

  const value: CopilotContextValue = {
    ...state,
    togglePanel,
    openPanel,
    closePanel,
    sendMessage,
    clearMessages,
    clearError,
    setContext,
  }

  return (
    <CopilotContext.Provider value={value}>
      {children}
    </CopilotContext.Provider>
  )
}

export const useCopilot = (): CopilotContextValue => {
  const context = useContext(CopilotContext)
  if (context === undefined) {
    throw new Error('useCopilot must be used within a CopilotProvider')
  }
  return context
}

export default CopilotContext
