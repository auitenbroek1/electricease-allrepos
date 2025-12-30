<?php

namespace Src\Copilot\Controllers;

use Exception;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;
use Illuminate\Routing\Controller;
use Illuminate\Support\Facades\Log;
use Src\Copilot\Services\CopilotService;

class CopilotController extends Controller
{
    public function __construct(
        protected CopilotService $copilotService
    ) {}

    /**
     * Handle a chat request
     */
    public function chat(Request $request): JsonResponse
    {
        $validated = $request->validate([
            'message' => 'required|string|max:10000',
            'context' => 'nullable|array',
            'context.job' => 'nullable|string|max:500',
            'context.phase' => 'nullable|string|max:500',
            'context.selected_parts' => 'nullable|array',
            'context.selected_parts.*' => 'string|max:200',
            'context.additional' => 'nullable|string|max:2000',
            'history' => 'nullable|array|max:50',
            'history.*.role' => 'required_with:history|string|in:user,assistant',
            'history.*.content' => 'required_with:history|string|max:10000',
        ]);

        try {
            $response = $this->copilotService->chat(
                message: $validated['message'],
                context: $validated['context'] ?? [],
                history: $validated['history'] ?? []
            );

            return response()->json([
                'success' => true,
                'response' => $response,
            ]);
        } catch (Exception $e) {
            Log::error('Copilot chat error', [
                'error' => $e->getMessage(),
                'trace' => $e->getTraceAsString(),
            ]);

            return response()->json([
                'success' => false,
                'error' => 'Failed to get response from AI assistant. Please try again.',
            ], 500);
        }
    }

    /**
     * Get contextual insights for a page
     */
    public function getInsights(Request $request): JsonResponse
    {
        $validated = $request->validate([
            'page' => 'required|string|max:100',
            'context' => 'nullable|array',
        ]);

        $page = $validated['page'];
        $context = $validated['context'] ?? [];

        try {
            $insights = $this->copilotService->getInsights($page, $context);

            return response()->json([
                'success' => true,
                'insights' => $insights,
            ]);
        } catch (Exception $e) {
            Log::error('Copilot insights error', [
                'error' => $e->getMessage(),
                'page' => $page,
            ]);

            return response()->json([
                'success' => false,
                'error' => 'Failed to load insights.',
            ], 500);
        }
    }
}
