<?php

return [

    'api_key' => env('ANTHROPIC_API_KEY'),

    'model' => env('ANTHROPIC_MODEL', 'claude-opus-4-20250514'),

    'max_tokens' => env('ANTHROPIC_MAX_TOKENS', 4096),

];
