import { useCopilot } from '../../contexts/CopilotContext'
import CopilotMessages from './CopilotMessages'
import CopilotInput from './CopilotInput'

// Sparkle icon for insights
const SparkleIcon = () => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    viewBox="0 0 24 24"
    fill="none"
    stroke="currentColor"
    strokeWidth="1.5"
    strokeLinecap="round"
    strokeLinejoin="round"
    className="h-4 w-4"
  >
    <path d="M9.937 15.5A2 2 0 0 0 8.5 14.063l-6.135-1.582a.5.5 0 0 1 0-.962L8.5 9.936A2 2 0 0 0 9.937 8.5l1.582-6.135a.5.5 0 0 1 .963 0L14.063 8.5A2 2 0 0 0 15.5 9.937l6.135 1.581a.5.5 0 0 1 0 .964L15.5 14.063a2 2 0 0 0-1.437 1.437l-1.582 6.135a.5.5 0 0 1-.963 0z" />
  </svg>
)

// Lightbulb icon for insights section
const LightbulbIcon = () => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    fill="none"
    viewBox="0 0 24 24"
    strokeWidth={1.5}
    stroke="currentColor"
    className="h-4 w-4"
  >
    <path
      strokeLinecap="round"
      strokeLinejoin="round"
      d="M12 18v-5.25m0 0a6.01 6.01 0 001.5-.189m-1.5.189a6.01 6.01 0 01-1.5-.189m3.75 7.478a12.06 12.06 0 01-4.5 0m3.75 2.383a14.406 14.406 0 01-3 0M14.25 18v-.192c0-.983.658-1.823 1.508-2.316a7.5 7.5 0 10-7.517 0c.85.493 1.509 1.333 1.509 2.316V18"
    />
  </svg>
)

// Chat icon
const ChatIcon = () => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    fill="none"
    viewBox="0 0 24 24"
    strokeWidth={1.5}
    stroke="currentColor"
    className="h-4 w-4"
  >
    <path
      strokeLinecap="round"
      strokeLinejoin="round"
      d="M8.625 12a.375.375 0 11-.75 0 .375.375 0 01.75 0zm0 0H8.25m4.125 0a.375.375 0 11-.75 0 .375.375 0 01.75 0zm0 0H12m4.125 0a.375.375 0 11-.75 0 .375.375 0 01.75 0zm0 0h-.375M21 12c0 4.556-4.03 8.25-9 8.25a9.764 9.764 0 01-2.555-.337A5.972 5.972 0 015.41 20.97a5.969 5.969 0 01-.474-.065 4.48 4.48 0 00.978-2.025c.09-.457-.133-.901-.467-1.226C3.93 16.178 3 14.189 3 12c0-4.556 4.03-8.25 9-8.25s9 3.694 9 8.25z"
    />
  </svg>
)

// Refresh icon
const RefreshIcon = ({ className }: { className?: string }) => (
  <svg
    xmlns="http://www.w3.org/2000/svg"
    fill="none"
    viewBox="0 0 24 24"
    strokeWidth={1.5}
    stroke="currentColor"
    className={className || 'h-4 w-4'}
  >
    <path
      strokeLinecap="round"
      strokeLinejoin="round"
      d="M16.023 9.348h4.992v-.001M2.985 19.644v-4.992m0 0h4.992m-4.993 0l3.181 3.183a8.25 8.25 0 0013.803-3.7M4.031 9.865a8.25 8.25 0 0113.803-3.7l3.181 3.182m0-4.991v4.99"
    />
  </svg>
)

const InsightsSection = () => {
  const { insights, insightsLoading, insightsError, refreshInsights, currentPage } = useCopilot()

  const pageDisplayName = currentPage.charAt(0).toUpperCase() + currentPage.slice(1)

  return (
    <div className="border-b border-slate-200 dark:border-slate-700 h-[40%] overflow-y-auto flex-shrink-0">
      {/* Section Header */}
      <div className="flex items-center justify-between px-4 py-2 bg-slate-100 dark:bg-slate-800">
        <div className="flex items-center space-x-2">
          <LightbulbIcon />
          <span className="text-xs font-semibold uppercase tracking-wide text-slate-600 dark:text-slate-300">
            Insights
          </span>
          <span className="text-xs text-slate-400 dark:text-slate-500">
            ({pageDisplayName})
          </span>
        </div>
        <button
          onClick={refreshInsights}
          disabled={insightsLoading}
          className="p-1 rounded hover:bg-slate-200 dark:hover:bg-slate-700 text-slate-500 dark:text-slate-400 disabled:opacity-50"
          title="Refresh insights"
        >
          <RefreshIcon className={`h-3.5 w-3.5 ${insightsLoading ? 'animate-spin' : ''}`} />
        </button>
      </div>

      {/* Insights Content */}
      <div className="px-4 py-3 max-h-48 overflow-y-auto">
        {insightsLoading && insights.length === 0 && (
          <div className="flex items-center justify-center py-4">
            <div className="flex items-center space-x-2 text-slate-500 dark:text-slate-400">
              <RefreshIcon className="h-4 w-4 animate-spin" />
              <span className="text-sm">Loading insights...</span>
            </div>
          </div>
        )}

        {insightsError && (
          <div className="text-sm text-red-500 dark:text-red-400 py-2">
            {insightsError}
          </div>
        )}

        {!insightsLoading && !insightsError && insights.length === 0 && (
          <p className="text-sm text-slate-500 dark:text-slate-400 py-2">
            No insights available for this page.
          </p>
        )}

        {insights.length > 0 && (
          <ul className="space-y-2">
            {insights.map((insight, index) => (
              <li
                key={index}
                className="flex items-start space-x-2 text-sm"
              >
                <span className="flex-shrink-0 mt-0.5 text-blue-500 dark:text-blue-400">
                  <svg className="h-3.5 w-3.5" fill="currentColor" viewBox="0 0 8 8">
                    <circle cx="4" cy="4" r="3" />
                  </svg>
                </span>
                <div>
                  {insight.title && (
                    <span className="font-medium text-slate-700 dark:text-slate-200">
                      {insight.title}:{' '}
                    </span>
                  )}
                  <span className="text-slate-600 dark:text-slate-300">
                    {insight.content}
                  </span>
                </div>
              </li>
            ))}
          </ul>
        )}
      </div>
    </div>
  )
}

const ChatSection = () => {
  const { clearMessages } = useCopilot()

  return (
    <div className="flex flex-col flex-1 min-h-0 h-[60%]">
      {/* Section Header */}
      <div className="flex items-center justify-between px-4 py-2 bg-slate-100 dark:bg-slate-800 border-b border-slate-200 dark:border-slate-700">
        <div className="flex items-center space-x-2">
          <ChatIcon />
          <span className="text-xs font-semibold uppercase tracking-wide text-slate-600 dark:text-slate-300">
            Chat
          </span>
        </div>
        <button
          onClick={clearMessages}
          className="p-1 rounded hover:bg-slate-200 dark:hover:bg-slate-700 text-slate-500 dark:text-slate-400"
          title="Clear conversation"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth={1.5}
            stroke="currentColor"
            className="h-3.5 w-3.5"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0"
            />
          </svg>
        </button>
      </div>

      {/* Messages */}
      <CopilotMessages />

      {/* Input */}
      <CopilotInput />
    </div>
  )
}

const CopilotSidePanel = () => {
  const { isOpen, togglePanel, closePanel } = useCopilot()

  return (
    <>
      {/* Vertical Tab - Always visible on right edge */}
      <button
        onClick={togglePanel}
        className={`
          fixed
          right-0
          top-1/2
          -translate-y-1/2
          z-40
          flex
          items-center
          justify-center
          bg-blue-600
          hover:bg-blue-700
          text-white
          font-medium
          text-sm
          py-3
          px-2
          rounded-l-lg
          shadow-lg
          transition-all
          duration-300
          ease-in-out
          ${isOpen ? 'opacity-0 pointer-events-none' : 'opacity-100'}
        `}
        style={{
          writingMode: 'vertical-rl',
          textOrientation: 'mixed',
        }}
      >
        <span className="flex items-center gap-2">
          <SparkleIcon />
          <span>AI Copilot</span>
        </span>
      </button>

      {/* Side Panel */}
      <div
        className={`
          fixed
          top-0
          right-0
          h-full
          w-[380px]
          bg-slate-50
          dark:bg-slate-900
          shadow-xl
          z-50
          flex
          flex-col
          transition-transform
          duration-300
          ease-in-out
          ${isOpen ? 'translate-x-0' : 'translate-x-full'}
        `}
      >
        {/* Panel Header */}
        <div className="flex items-center justify-between px-4 py-3 bg-white dark:bg-slate-800 border-b border-slate-200 dark:border-slate-700">
          <div className="flex items-center space-x-2">
            <div className="flex h-8 w-8 items-center justify-center rounded-full bg-blue-100 dark:bg-blue-900">
              <SparkleIcon />
            </div>
            <h2 className="text-sm font-semibold text-slate-900 dark:text-white">
              AI Copilot
            </h2>
          </div>
          <button
            onClick={closePanel}
            className="p-1 rounded hover:bg-slate-100 dark:hover:bg-slate-700 text-slate-500 dark:text-slate-400"
          >
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth={1.5}
              stroke="currentColor"
              className="h-5 w-5"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M6 18L18 6M6 6l12 12"
              />
            </svg>
          </button>
        </div>

        {/* Insights Section */}
        <InsightsSection />

        {/* Chat Section */}
        <ChatSection />
      </div>

      {/* Backdrop for mobile */}
      {isOpen && (
        <div
          className="fixed inset-0 bg-black/20 z-40 lg:hidden"
          onClick={closePanel}
        />
      )}
    </>
  )
}

export default CopilotSidePanel
