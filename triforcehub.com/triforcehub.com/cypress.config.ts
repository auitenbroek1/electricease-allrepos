import { defineConfig } from 'cypress'

export default defineConfig({
  e2e: {
    baseUrl: `https://app.electric-ease.com.test`,
    setupNodeEvents(on, config) {
      // implement node event listeners here
    },
    specPattern: `tests/cypress/e2e/**/*.cy.{ts,tsx}`,
    supportFile: false,
  },
})
