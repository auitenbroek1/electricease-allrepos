<?php

namespace Src\Copilot\Providers;

use Anthropic\Client;
use Illuminate\Support\ServiceProvider;
use Src\Copilot\Services\CopilotService;

class CopilotServiceProvider extends ServiceProvider
{
    public function register(): void
    {
        $this->mergeConfigFrom(
            __DIR__.'/../../../config/copilot.php', 'copilot'
        );

        $this->app->singleton(Client::class, function ($app) {
            return new Client(apiKey: config('copilot.api_key'));
        });

        $this->app->singleton(CopilotService::class, function ($app) {
            return new CopilotService(
                $app->make(Client::class),
            );
        });
    }
}
