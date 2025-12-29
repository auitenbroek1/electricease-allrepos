import './index.css'

import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import { Toaster } from 'react-hot-toast'
import { App } from '@/pages'

import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import { ReactQueryDevtools } from '@tanstack/react-query-devtools'

//

declare global {
  interface Window {
    Vapor: any
  }
}

import Vapor from 'laravel-vapor'

window.Vapor = Vapor
window.Vapor.withBaseAssetUrl(import.meta.env.VITE_VAPOR_ASSET_URL)

//

const queryClient = new QueryClient()

const container = document.getElementById(`root`)
const root = createRoot(container!)

root.render(
  <StrictMode>
    <QueryClientProvider client={queryClient}>
      <BrowserRouter>
        <Toaster position={`top-right`} />
        <App />
      </BrowserRouter>
      {/* <ReactQueryDevtools initialIsOpen={false} /> */}
    </QueryClientProvider>
  </StrictMode>,
)
