import axios from 'axios'

export interface CopilotMessage {
  role: 'user' | 'assistant'
  content: string
}

export interface CopilotContext {
  job?: string
  phase?: string
  selected_parts?: string[]
  additional?: string
}

export interface CopilotChatRequest {
  message: string
  context?: CopilotContext
  history?: CopilotMessage[]
}

export interface CopilotChatResponse {
  success: boolean
  response?: string
  error?: string
}

export interface CopilotInsight {
  title?: string
  content: string
}

export interface CopilotInsightsRequest {
  page: string
  context?: Record<string, unknown>
}

export interface CopilotInsightsResponse {
  success: boolean
  insights?: CopilotInsight[]
  error?: string
}

export const CopilotController = {
  chat: async (request: CopilotChatRequest): Promise<CopilotChatResponse> => {
    const response = await axios.post<CopilotChatResponse>('/api/copilot/chat', request)
    return response.data
  },

  getInsights: async (request: CopilotInsightsRequest): Promise<CopilotInsightsResponse> => {
    const response = await axios.post<CopilotInsightsResponse>('/api/copilot/insights', request)
    return response.data
  },
}
