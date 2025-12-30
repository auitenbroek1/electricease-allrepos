import { createContext, useContext, useState, useCallback, useEffect, ReactNode } from 'react'
import { useLocation } from 'react-router-dom'
import { CopilotController, CopilotMessage, CopilotContext as CopilotContextData, CopilotInsight } from '../api/copilot'

interface CopilotState {
  isOpen: boolean
  messages: CopilotMessage[]
  isLoading: boolean
  error: string | null
  context: CopilotContextData
  currentPage: string
  pageContext: Record<string, unknown>
  insights: CopilotInsight[]
  insightsLoading: boolean
  insightsError: string | null
}

interface CopilotContextValue extends CopilotState {
  togglePanel: () => void
  openPanel: () => void
  closePanel: () => void
  sendMessage: (message: string) => Promise<void>
  clearMessages: () => void
  clearError: () => void
  setContext: (context: CopilotContextData) => void
  setPageContext: (context: Record<string, unknown>) => void
  refreshInsights: () => Promise<void>
}

const defaultState: CopilotState = {
  isOpen: false,
  messages: [],
  isLoading: false,
  error: null,
  context: {},
  currentPage: '',
  pageContext: {},
  insights: [],
  insightsLoading: false,
  insightsError: null,
}

const CopilotContext = createContext<CopilotContextValue | undefined>(undefined)

// Helper to extract page name from pathname
const getPageName = (pathname: string): string => {
  const segments = pathname.split('/').filter(Boolean)
  // Handle /app/dashboard, /app/assemblies, /app/bids, etc.
  if (segments[0] === 'app' && segments.length > 1) {
    return segments[1]
  }
  if (segments[0] === 'app') {
    return 'dashboard'
  }
  return segments[0] || 'home'
}

export const CopilotProvider = ({ children }: { children: ReactNode }) => {
  const [state, setState] = useState<CopilotState>(defaultState)
  const location = useLocation()

  // Update current page when location changes
  useEffect(() => {
    const pageName = getPageName(location.pathname)
    setState(prev => ({
      ...prev,
      currentPage: pageName,
      // Reset insights when page changes
      insights: [],
      insightsError: null,
    }))
  }, [location.pathname])

  // Fetch insights when panel opens or page changes (while panel is open)
  const fetchInsights = useCallback(async () => {
    if (!state.currentPage) return

    setState(prev => ({ ...prev, insightsLoading: true, insightsError: null }))

    try {
      const response = await CopilotController.getInsights({
        page: state.currentPage,
        context: state.pageContext,
      })

      if (response.success && response.insights) {
        setState(prev => ({
          ...prev,
          insights: response.insights || [],
          insightsLoading: false,
        }))
      } else {
        setState(prev => ({
          ...prev,
          insightsLoading: false,
          insightsError: response.error || 'Failed to load insights',
        }))
      }
    } catch (err) {
      setState(prev => ({
        ...prev,
        insightsLoading: false,
        insightsError: 'Failed to connect to AI assistant.',
      }))
    }
  }, [state.currentPage, state.pageContext])

  // Fetch insights when panel opens
  useEffect(() => {
    if (state.isOpen && state.currentPage && state.insights.length === 0 && !state.insightsLoading) {
      fetchInsights()
    }
  }, [state.isOpen, state.currentPage])

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

  const setPageContext = useCallback((pageContext: Record<string, unknown>) => {
    setState(prev => ({ ...prev, pageContext }))
  }, [])

  const refreshInsights = useCallback(async () => {
    await fetchInsights()
  }, [fetchInsights])

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
      // Include page context in the request
      const contextWithPage: CopilotContextData = {
        ...state.context,
        additional: `Current page: ${state.currentPage}. ${state.context.additional || ''}`.trim(),
      }

      const response = await CopilotController.chat({
        message,
        context: contextWithPage,
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
  }, [state.context, state.messages, state.currentPage])

  const value: CopilotContextValue = {
    ...state,
    togglePanel,
    openPanel,
    closePanel,
    sendMessage,
    clearMessages,
    clearError,
    setContext,
    setPageContext,
    refreshInsights,
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
