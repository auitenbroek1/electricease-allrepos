<?php

namespace Src\Copilot\Services;

use Anthropic\Client;
use Exception;
use Illuminate\Support\Facades\Log;

class CopilotService
{
    protected Client $client;

    protected string $model;

    protected int $maxTokens;

    protected int $maxRetries;

    protected int $retryDelay;

    protected string $systemPrompt = <<<'PROMPT'
You are an expert electrical estimating assistant for Electric Ease, a professional electrical estimating software. You have deep knowledge of:

- Electrical materials, parts, and assemblies (conduit, wire, fittings, boxes, panels, etc.)
- Labor units and productivity rates for electrical work
- NEC (National Electrical Code) requirements and best practices
- Electrical takeoff and estimating methodologies
- Commercial, residential, and industrial electrical systems
- Cost estimation, markup strategies, and bid preparation

Your role is to help electrical contractors and estimators with:
- Answering questions about electrical materials and installation methods
- Providing guidance on labor hour estimates
- Explaining code requirements relevant to estimates
- Suggesting assemblies or material alternatives
- Helping troubleshoot estimating challenges
- Offering best practices for accurate electrical bids

Be concise, professional, and practical in your responses. When discussing costs or labor hours, note that actual values may vary based on local conditions, labor rates, and specific project requirements. Always encourage users to verify critical information against current code editions and local requirements.
PROMPT;

    protected array $pageInsightPrompts = [
        'dashboard' => 'Generate 2-3 concise insights for an electrical estimator viewing their dashboard. Focus on: productivity tips, pending bid reminders, or workflow suggestions. Keep each insight to 1-2 sentences.',
        'assemblies' => 'Generate 2-3 concise insights about electrical assemblies. Focus on: assembly optimization tips, commonly grouped parts, or labor efficiency suggestions. Keep each insight to 1-2 sentences.',
        'bids' => 'Generate 2-3 concise insights for bid preparation. Focus on: competitive bidding strategies, common markup considerations, or proposal best practices. Keep each insight to 1-2 sentences.',
        'jobs' => 'Generate 2-3 concise insights for job management. Focus on: project organization tips, phase planning suggestions, or material tracking best practices. Keep each insight to 1-2 sentences.',
        'parts' => 'Generate 2-3 concise insights about electrical parts and materials. Focus on: pricing trends, alternative materials, or inventory management tips. Keep each insight to 1-2 sentences.',
        'settings' => 'Generate 2-3 concise insights about optimizing software settings. Focus on: labor rate configuration, markup strategies, or workflow customization tips. Keep each insight to 1-2 sentences.',
        'default' => 'Generate 2-3 concise, helpful insights for an electrical estimator. Focus on: estimating best practices, efficiency tips, or industry knowledge. Keep each insight to 1-2 sentences.',
    ];

    public function __construct(Client $client, int $maxRetries = 3, int $retryDelay = 1000)
    {
        $this->client = $client;
        $this->model = config('copilot.model', 'claude-sonnet-4-20250514');
        $this->maxTokens = config('copilot.max_tokens', 4096);
        $this->maxRetries = $maxRetries;
        $this->retryDelay = $retryDelay;
    }

    /**
     * Send a chat message and get a response
     *
     * @param  string  $message  The user's message
     * @param  array  $context  Additional context about the current state (e.g., current job, selected parts)
     * @param  array  $history  Previous conversation messages [['role' => 'user|assistant', 'content' => '...']]
     * @return string The assistant's response
     *
     * @throws Exception
     */
    public function chat(string $message, array $context = [], array $history = []): string
    {
        $attempts = 0;
        $lastException = null;

        $systemPrompt = $this->buildSystemPrompt($context);
        $messages = $this->buildMessages($message, $history);

        while ($attempts < $this->maxRetries) {
            try {
                $response = $this->client->messages->create([
                    'model' => $this->model,
                    'max_tokens' => $this->maxTokens,
                    'system' => $systemPrompt,
                    'messages' => $messages,
                ]);

                return $response->content[0]->text;
            } catch (Exception $e) {
                $lastException = $e;
                $attempts++;

                if ($attempts < $this->maxRetries) {
                    usleep($this->retryDelay * pow(2, $attempts - 1) * 1000);
                    Log::warning("Anthropic API call failed, attempt {$attempts} of {$this->maxRetries}", [
                        'error' => $e->getMessage(),
                    ]);

                    continue;
                }
            }
        }

        Log::error('Failed to get response after max retries', [
            'error' => $lastException?->getMessage() ?? 'Unknown error',
        ]);

        throw new Exception('Failed to get response after '.$this->maxRetries.' attempts: '.
            ($lastException?->getMessage() ?? 'Unknown error'));
    }

    /**
     * Get contextual insights for a specific page
     *
     * @param  string  $page  The current page name (dashboard, assemblies, bids, etc.)
     * @param  array  $context  Additional context about the page state
     * @return array Array of insights with title and content
     */
    public function getInsights(string $page, array $context = []): array
    {
        $insightPrompt = $this->pageInsightPrompts[$page] ?? $this->pageInsightPrompts['default'];

        // Add context to the prompt if available
        if (! empty($context)) {
            $contextString = json_encode($context, JSON_PRETTY_PRINT);
            $insightPrompt .= "\n\nContext: {$contextString}";
        }

        $systemPrompt = $this->systemPrompt."\n\nIMPORTANT: Respond ONLY with a JSON array of insights. Each insight should have a 'title' (2-4 words) and 'content' (1-2 sentences). Example format:\n[{\"title\": \"Quick Tip\", \"content\": \"Your insight here.\"}]";

        try {
            $response = $this->client->messages->create([
                'model' => $this->model,
                'max_tokens' => 500,
                'system' => $systemPrompt,
                'messages' => [
                    ['role' => 'user', 'content' => $insightPrompt],
                ],
            ]);

            $responseText = $response->content[0]->text;

            // Parse JSON response
            $insights = json_decode($responseText, true);

            if (json_last_error() !== JSON_ERROR_NONE || ! is_array($insights)) {
                // Try to extract JSON from the response
                if (preg_match('/\[.*\]/s', $responseText, $matches)) {
                    $insights = json_decode($matches[0], true);
                }

                if (json_last_error() !== JSON_ERROR_NONE || ! is_array($insights)) {
                    Log::warning('Failed to parse insights JSON', ['response' => $responseText]);

                    return $this->getDefaultInsights($page);
                }
            }

            // Validate and format insights
            return array_slice(array_map(function ($insight) {
                return [
                    'title' => $insight['title'] ?? null,
                    'content' => $insight['content'] ?? $insight['message'] ?? '',
                ];
            }, $insights), 0, 3);

        } catch (Exception $e) {
            Log::error('Failed to get insights', ['error' => $e->getMessage(), 'page' => $page]);

            return $this->getDefaultInsights($page);
        }
    }

    /**
     * Get default insights when API fails
     */
    protected function getDefaultInsights(string $page): array
    {
        $defaults = [
            'dashboard' => [
                ['title' => 'Stay Organized', 'content' => 'Review your pending bids regularly to ensure timely submissions.'],
                ['title' => 'Quick Tip', 'content' => 'Use assemblies to speed up repetitive takeoff tasks.'],
            ],
            'assemblies' => [
                ['title' => 'Optimize Labor', 'content' => 'Group commonly installed items together to improve labor accuracy.'],
                ['title' => 'Best Practice', 'content' => 'Review assembly costs quarterly to stay competitive.'],
            ],
            'bids' => [
                ['title' => 'Competitive Edge', 'content' => 'Always verify material pricing before final bid submission.'],
                ['title' => 'Pro Tip', 'content' => 'Include contingency for unforeseen conditions in complex projects.'],
            ],
            'jobs' => [
                ['title' => 'Phase Planning', 'content' => 'Break large jobs into phases for better tracking and billing.'],
                ['title' => 'Material Tracking', 'content' => 'Update quantities regularly to maintain accurate job costing.'],
            ],
            'parts' => [
                ['title' => 'Price Updates', 'content' => 'Check distributor pricing monthly for accurate estimates.'],
                ['title' => 'Alternatives', 'content' => 'Consider equivalent materials when preferred items are unavailable.'],
            ],
        ];

        return $defaults[$page] ?? [
            ['title' => 'Pro Tip', 'content' => 'Use the AI Copilot chat below for specific estimating questions.'],
            ['title' => 'Need Help?', 'content' => 'Ask about materials, labor units, or NEC requirements.'],
        ];
    }

    /**
     * Build the system prompt with optional context
     */
    protected function buildSystemPrompt(array $context): string
    {
        $prompt = $this->systemPrompt;

        if (! empty($context)) {
            $prompt .= "\n\n## Current Context\n";

            if (isset($context['job'])) {
                $prompt .= "The user is currently working on a job: {$context['job']}\n";
            }

            if (isset($context['phase'])) {
                $prompt .= "Current phase: {$context['phase']}\n";
            }

            if (isset($context['selected_parts']) && is_array($context['selected_parts'])) {
                $prompt .= 'Selected parts: '.implode(', ', $context['selected_parts'])."\n";
            }

            if (isset($context['additional'])) {
                $prompt .= $context['additional']."\n";
            }
        }

        return $prompt;
    }

    /**
     * Build the messages array for the API call
     */
    protected function buildMessages(string $message, array $history): array
    {
        $messages = [];

        foreach ($history as $item) {
            if (isset($item['role'], $item['content'])) {
                $messages[] = [
                    'role' => $item['role'],
                    'content' => $item['content'],
                ];
            }
        }

        $messages[] = [
            'role' => 'user',
            'content' => $message,
        ];

        return $messages;
    }

    /**
     * Get the underlying Anthropic client
     */
    public function getClient(): Client
    {
        return $this->client;
    }
}
