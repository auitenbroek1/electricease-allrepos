/* eslint-disable @typescript-eslint/no-explicit-any */
// Helper function for structured logging
export function logEvent(event: string, data: Record<string, any> = {}) {
  const logEntry = {
    timestamp: new Date().toISOString(),
    event,
    ...data,
  };
  console.log(JSON.stringify(logEntry));
}
