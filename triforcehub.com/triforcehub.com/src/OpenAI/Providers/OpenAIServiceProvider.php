<?php

namespace Src\OpenAI\Providers;

use Illuminate\Support\ServiceProvider;
use OpenAI\Client as OpenAI;
use OpenAI\Factory;
use Src\OpenAI\Services\OpenAIService;

class OpenAIServiceProvider extends ServiceProvider
{
    public function register()
    {
        $this->mergeConfigFrom(
            __DIR__.'/../config.php', 'openai'
        );

        $this->app->singleton(OpenAI::class, function ($app) {
            return (new Factory)->withApiKey(config('openai.api_key'))->make();
        });

        $this->app->singleton(OpenAIService::class, function ($app) {
            return new OpenAIService(
                $app->make(OpenAI::class),
            );
        });
    }
}
