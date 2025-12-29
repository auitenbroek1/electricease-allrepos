<?php

namespace Src\OpenAI\Services;

use Exception;
use Illuminate\Support\Facades\Log;
use OpenAI\Client as OpenAI;
use OpenAI\Exceptions\TransporterException;

class OpenAIService
{
    protected $openai;

    protected $maxRetries;

    protected $retryDelay;

    public function __construct(OpenAI $openai, int $maxRetries = 3, int $retryDelay = 1000)
    {
        $this->openai = $openai;
        $this->maxRetries = $maxRetries;
        $this->retryDelay = $retryDelay; // milliseconds
    }

    /**
     * Generate embeddings for given text with retry logic
     *
     * @throws Exception
     */
    public function createEmbedding(string $text, string $model = 'text-embedding-ada-002'): array
    {
        $attempts = 0;
        $lastException = null;

        while ($attempts < $this->maxRetries) {
            try {
                $response = $this->openai->embeddings()->create([
                    'model' => $model,
                    'input' => $text,
                ]);

                return $response->embeddings[0]->embedding;
            } catch (TransporterException $e) {
                $lastException = $e;
                $attempts++;

                if ($attempts < $this->maxRetries) {
                    // Exponential backoff
                    usleep($this->retryDelay * pow(2, $attempts - 1));
                    Log::warning("OpenAI API call failed, attempt {$attempts} of {$this->maxRetries}", [
                        'error' => $e->getMessage(),
                        'text_length' => strlen($text),
                    ]);

                    continue;
                }
            }
        }

        Log::error('Failed to get embedding after max retries', [
            'error' => $lastException ? $lastException->getMessage() : 'Unknown error',
            'text_length' => strlen($text),
        ]);

        throw new Exception('Failed to generate embedding after '.$this->maxRetries.' attempts: '.
            ($lastException ? $lastException->getMessage() : 'Unknown error'));
    }

    /**
     * Get the underlying OpenAI client
     */
    public function getClient(): OpenAI
    {
        return $this->openai;
    }
}
