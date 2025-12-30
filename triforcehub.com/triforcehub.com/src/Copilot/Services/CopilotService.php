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

    public function __construct(Client $client, int $maxRetries = 3, int $retryDelay = 1000)
    {
        $this->client = $client;
        $this->model = config('copilot.model', 'claude-opus-4-20250514');
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
