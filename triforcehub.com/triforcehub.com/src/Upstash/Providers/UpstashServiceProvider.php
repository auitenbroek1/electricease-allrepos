<?php

namespace Src\Upstash\Providers;

use Illuminate\Support\ServiceProvider;
use Laravel\Scout\EngineManager;
use Src\OpenAI\Services\OpenAIService;
use Src\Upstash\Commands\TestVectorSearch;
use Src\Upstash\Engines\HybridEngine;
use Src\Upstash\Engines\UpstashVectorEngine;
use Upstash\Vector\Contracts\IndexInterface;

class UpstashServiceProvider extends ServiceProvider
{
    public function register()
    {
        $this->mergeConfigFrom(
            __DIR__.'/../config.php', 'upstash'
        );
    }

    public function boot()
    {
        $this->commands([
            TestVectorSearch::class,
        ]);

        resolve(EngineManager::class)->extend('upstash', function () {
            return new UpstashVectorEngine(
                resolve(IndexInterface::class),
                resolve(OpenAIService::class),
            );
        });

        resolve(EngineManager::class)->extend('hybrid', function () {
            return new HybridEngine;
        });
    }
}
