<?php

namespace Src\Prices\Providers;

use Illuminate\Support\ServiceProvider;
use Src\Prices\Commands\CachePrices;

class PricesServiceProvider extends ServiceProvider
{
    public function register(): void {}

    public function boot(): void
    {
        if ($this->app->runningInConsole()) {
            $this->commands([
                CachePrices::class,
            ]);
        }
    }
}
