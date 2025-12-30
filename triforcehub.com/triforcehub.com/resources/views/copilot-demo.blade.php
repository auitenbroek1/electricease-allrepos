<!DOCTYPE html>
<html lang="en" class="h-full bg-gray-100">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>AI Copilot Demo - Electric Ease</title>
    <script src="https://cdn.tailwindcss.com"></script>
    <style>
        .typing-indicator span {
            animation: bounce 1.4s infinite ease-in-out both;
        }
        .typing-indicator span:nth-child(1) { animation-delay: -0.32s; }
        .typing-indicator span:nth-child(2) { animation-delay: -0.16s; }
        @keyframes bounce {
            0%, 80%, 100% { transform: scale(0); }
            40% { transform: scale(1); }
        }
        .message-content {
            white-space: pre-wrap;
            word-wrap: break-word;
        }
        .message-content strong { font-weight: 600; }
        .message-content h2 { font-size: 1rem; font-weight: 600; margin-top: 0.75rem; margin-bottom: 0.25rem; }
        .message-content ul, .message-content ol { margin-left: 1rem; margin-top: 0.25rem; }
        .message-content li { margin-bottom: 0.125rem; }
        .message-content code { background: rgba(0,0,0,0.1); padding: 0.125rem 0.25rem; border-radius: 0.25rem; font-size: 0.875em; }
    </style>
</head>
<body class="h-full antialiased">
    <div class="min-h-full flex flex-col">
        <!-- Header -->
        <header class="bg-white shadow-sm">
            <div class="max-w-4xl mx-auto px-4 py-4 flex items-center justify-between">
                <div class="flex items-center space-x-3">
                    <div class="flex h-10 w-10 items-center justify-center rounded-full bg-blue-100">
                        <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round" class="h-5 w-5 text-blue-600">
                            <path d="M9.937 15.5A2 2 0 0 0 8.5 14.063l-6.135-1.582a.5.5 0 0 1 0-.962L8.5 9.936A2 2 0 0 0 9.937 8.5l1.582-6.135a.5.5 0 0 1 .963 0L14.063 8.5A2 2 0 0 0 15.5 9.937l6.135 1.581a.5.5 0 0 1 0 .964L15.5 14.063a2 2 0 0 0-1.437 1.437l-1.582 6.135a.5.5 0 0 1-.963 0z" />
                            <path d="M20 3v4" /><path d="M22 5h-4" />
                            <path d="M4 17v2" /><path d="M5 18H3" />
                        </svg>
                    </div>
                    <div>
                        <h1 class="text-xl font-semibold text-gray-900">AI Copilot Demo</h1>
                        <p class="text-sm text-gray-500">Electric Ease Estimating Assistant</p>
                    </div>
                </div>
                <button onclick="clearChat()" class="text-sm text-gray-500 hover:text-gray-700 flex items-center space-x-1">
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-4 h-4">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M14.74 9l-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 01-2.244 2.077H8.084a2.25 2.25 0 01-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 00-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 013.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 00-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 00-7.5 0" />
                    </svg>
                    <span>Clear</span>
                </button>
            </div>
        </header>

        <!-- Chat Container -->
        <main class="flex-1 max-w-4xl w-full mx-auto flex flex-col">
            <!-- Messages -->
            <div id="messages" class="flex-1 overflow-y-auto p-4 space-y-4">
                <!-- Welcome message -->
                <div class="flex justify-center">
                    <div class="bg-blue-50 rounded-lg px-4 py-3 max-w-lg text-center">
                        <p class="text-sm text-blue-800 font-medium">Welcome to the AI Copilot</p>
                        <p class="text-xs text-blue-600 mt-1">Ask questions about electrical estimating, materials, labor units, NEC requirements, and more.</p>
                    </div>
                </div>
            </div>

            <!-- Input -->
            <div class="border-t border-gray-200 bg-white p-4">
                <form onsubmit="sendMessage(event)" class="flex items-end space-x-3">
                    <div class="flex-1">
                        <textarea
                            id="messageInput"
                            rows="1"
                            placeholder="Ask a question about electrical estimating..."
                            class="block w-full resize-none rounded-lg border border-gray-300 px-4 py-3 text-sm focus:border-blue-500 focus:outline-none focus:ring-1 focus:ring-blue-500"
                            onkeydown="handleKeyDown(event)"
                            oninput="autoResize(this)"
                        ></textarea>
                    </div>
                    <button
                        type="submit"
                        id="sendButton"
                        class="flex h-11 w-11 items-center justify-center rounded-lg bg-blue-600 text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 disabled:bg-gray-300 disabled:cursor-not-allowed transition-colors"
                    >
                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" class="h-5 w-5">
                            <path stroke-linecap="round" stroke-linejoin="round" d="M6 12L3.269 3.126A59.768 59.768 0 0121.485 12 59.77 59.77 0 013.27 20.876L5.999 12zm0 0h7.5" />
                        </svg>
                    </button>
                </form>
                <p class="mt-2 text-center text-xs text-gray-400">Press Enter to send, Shift+Enter for new line</p>
            </div>
        </main>

        <!-- Sample Questions -->
        <div class="bg-gray-50 border-t border-gray-200">
            <div class="max-w-4xl mx-auto px-4 py-3">
                <p class="text-xs text-gray-500 mb-2">Try asking:</p>
                <div class="flex flex-wrap gap-2">
                    <button onclick="askQuestion('What size conduit do I need for 4 #10 THHN wires?')" class="text-xs bg-white border border-gray-200 rounded-full px-3 py-1 hover:bg-gray-50 hover:border-gray-300 transition-colors">Conduit sizing for #10 wires</button>
                    <button onclick="askQuestion('What is the labor unit for installing a 200A panel?')" class="text-xs bg-white border border-gray-200 rounded-full px-3 py-1 hover:bg-gray-50 hover:border-gray-300 transition-colors">Labor for 200A panel</button>
                    <button onclick="askQuestion('What are typical markup percentages for electrical bids?')" class="text-xs bg-white border border-gray-200 rounded-full px-3 py-1 hover:bg-gray-50 hover:border-gray-300 transition-colors">Markup percentages</button>
                    <button onclick="askQuestion('Explain the difference between EMT, IMC, and rigid conduit')" class="text-xs bg-white border border-gray-200 rounded-full px-3 py-1 hover:bg-gray-50 hover:border-gray-300 transition-colors">Conduit types</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        let isLoading = false;
        let messageHistory = [];

        function autoResize(textarea) {
            textarea.style.height = 'auto';
            textarea.style.height = Math.min(textarea.scrollHeight, 150) + 'px';
        }

        function handleKeyDown(event) {
            if (event.key === 'Enter' && !event.shiftKey) {
                event.preventDefault();
                sendMessage(event);
            }
        }

        function askQuestion(question) {
            document.getElementById('messageInput').value = question;
            sendMessage(new Event('submit'));
        }

        function clearChat() {
            messageHistory = [];
            const messagesContainer = document.getElementById('messages');
            messagesContainer.innerHTML = `
                <div class="flex justify-center">
                    <div class="bg-blue-50 rounded-lg px-4 py-3 max-w-lg text-center">
                        <p class="text-sm text-blue-800 font-medium">Welcome to the AI Copilot</p>
                        <p class="text-xs text-blue-600 mt-1">Ask questions about electrical estimating, materials, labor units, NEC requirements, and more.</p>
                    </div>
                </div>
            `;
        }

        function addMessage(role, content) {
            const messagesContainer = document.getElementById('messages');
            const messageDiv = document.createElement('div');
            messageDiv.className = `flex ${role === 'user' ? 'justify-end' : 'justify-start'}`;

            const bubble = document.createElement('div');
            bubble.className = `max-w-[80%] rounded-lg px-4 py-2 text-sm ${
                role === 'user'
                    ? 'bg-blue-600 text-white'
                    : 'bg-white border border-gray-200 text-gray-900'
            }`;

            const contentDiv = document.createElement('div');
            contentDiv.className = 'message-content';
            contentDiv.innerHTML = formatMessage(content);
            bubble.appendChild(contentDiv);

            messageDiv.appendChild(bubble);
            messagesContainer.appendChild(messageDiv);
            messagesContainer.scrollTop = messagesContainer.scrollHeight;
        }

        function formatMessage(text) {
            // Simple markdown-like formatting
            return text
                .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
                .replace(/^## (.*$)/gm, '<h2>$1</h2>')
                .replace(/^- (.*$)/gm, '<li>$1</li>')
                .replace(/`(.*?)`/g, '<code>$1</code>');
        }

        function showTypingIndicator() {
            const messagesContainer = document.getElementById('messages');
            const typingDiv = document.createElement('div');
            typingDiv.id = 'typing-indicator';
            typingDiv.className = 'flex justify-start';
            typingDiv.innerHTML = `
                <div class="bg-white border border-gray-200 rounded-lg px-4 py-3">
                    <div class="typing-indicator flex space-x-1">
                        <span class="w-2 h-2 bg-gray-400 rounded-full"></span>
                        <span class="w-2 h-2 bg-gray-400 rounded-full"></span>
                        <span class="w-2 h-2 bg-gray-400 rounded-full"></span>
                    </div>
                </div>
            `;
            messagesContainer.appendChild(typingDiv);
            messagesContainer.scrollTop = messagesContainer.scrollHeight;
        }

        function hideTypingIndicator() {
            const indicator = document.getElementById('typing-indicator');
            if (indicator) indicator.remove();
        }

        async function sendMessage(event) {
            event.preventDefault();

            if (isLoading) return;

            const input = document.getElementById('messageInput');
            const message = input.value.trim();

            if (!message) return;

            // Add user message
            addMessage('user', message);
            messageHistory.push({ role: 'user', content: message });

            // Clear input
            input.value = '';
            input.style.height = 'auto';

            // Show loading state
            isLoading = true;
            document.getElementById('sendButton').disabled = true;
            showTypingIndicator();

            try {
                const response = await fetch('/api/copilot/chat', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Accept': 'application/json',
                    },
                    body: JSON.stringify({
                        message: message,
                        history: messageHistory.slice(0, -1), // Exclude the message we just added
                        context: {}
                    }),
                });

                const data = await response.json();

                hideTypingIndicator();

                if (data.success && data.response) {
                    addMessage('assistant', data.response);
                    messageHistory.push({ role: 'assistant', content: data.response });
                } else {
                    addMessage('assistant', 'Sorry, I encountered an error. Please try again.');
                }
            } catch (error) {
                hideTypingIndicator();
                addMessage('assistant', 'Sorry, I could not connect to the server. Please try again.');
                console.error('Error:', error);
            } finally {
                isLoading = false;
                document.getElementById('sendButton').disabled = false;
                input.focus();
            }
        }
    </script>
</body>
</html>
