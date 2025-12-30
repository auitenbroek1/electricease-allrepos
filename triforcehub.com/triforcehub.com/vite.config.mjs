/* eslint-disable no-undef */
import { defineConfig, splitVendorChunkPlugin } from 'vite'
import laravel from 'laravel-vite-plugin'
import react from '@vitejs/plugin-react'
import svgr from 'vite-plugin-svgr'
import 'dotenv/config'
import { homedir } from 'os'
import { resolve } from 'path'
import fs from 'fs'

export default defineConfig({
  build: {
    target: [`esnext`],
  },
  optimizeDeps: {
    esbuildOptions: {
      target: `esnext`,
    },
  },
  plugins: [
    laravel([`resources/src/index.js`]),
    react(),
    splitVendorChunkPlugin(),
    svgr(),
  ],
  resolve: {
    alias: {
      '@': `/resources/src`,
      // emitter: require.resolve(`emitter-component`),
    },
  },
  server: {
    ...detectServerConfig(),
    proxy: {
      '/api': {
        target: process.env.API_URL ?? 'http://localhost:8000',
        changeOrigin: true,
        secure: false,
      },
      '/sanctum': {
        target: process.env.API_URL ?? 'http://localhost:8000',
        changeOrigin: true,
        secure: false,
      },
    },
  },
})

function detectServerConfig() {
  const VITE_URL = process.env.VITE_URL ?? `http://localhost:3000`

  const segments = new URL(VITE_URL)
  const hostname = segments.hostname ?? `localhost`
  const port = segments.port ?? `3000`

  const server = {
    hmr: {
      protocol: `ws`,
      host: hostname,
    },
    host: hostname,
    port: port,
  }

  //

  const certificates = `${homedir()}/.config/valet/Certificates`
  const key = resolve(certificates, `${hostname}.key`)
  const cert = resolve(certificates, `${hostname}.crt`)

  if (fs.existsSync(key) && fs.existsSync(cert)) {
    server.hmr.protocol = `wss`

    server.https = {
      key: fs.readFileSync(key),
      cert: fs.readFileSync(cert),
    }
  }

  //

  return server
}
