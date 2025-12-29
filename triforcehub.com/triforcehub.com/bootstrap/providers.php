<?php

return [
    App\Providers\FortifyServiceProvider::class,
    App\Providers\AppServiceProvider::class,
    Src\OpenAI\Providers\OpenAIServiceProvider::class,
    Src\Prices\Providers\PricesServiceProvider::class,
    Src\Upstash\Providers\UpstashServiceProvider::class,
];
