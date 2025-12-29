<?php

use App\Models\Symbol;
use Illuminate\Database\Migrations\Migration;
use Illuminate\Support\Str;

return new class extends Migration
{
    public function up(): void
    {
        $symbols = [
            [
                'name' => 'Check mark',
                'asset' => null,
                'data' => <<<'EOF'
                    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-12 h-12">
                        <path stroke-linecap="round" stroke-linejoin="round" d="M9 12.75L11.25 15 15 9.75M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                    </svg>
                EOF,
                'position' => 1,
            ],
            // https://www.edrawsoft.com/symbols/wiring-diagram-symbols.html
            [
                'name' => 'Singleplex receptacle',
                'asset' => 'singleplexreceptacle.png',
                'data' => null,
                'position' => 2,
            ],
            [
                'name' => 'Duplex receptacle',
                'asset' => 'duplexreceptacle.png',
                'data' => null,
                'position' => 3,
            ],
            // https://www.archtoolbox.com/electrical-plan-symbols/
            [
                'name' => 'Duplex Outlet',
                'asset' => null,
                'data' => <<<'EOF'
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 60 50" class="w-12 h-12">
                        <path fill="none" stroke="currentColor" d="M35.76,23.45a8.34,8.34,0,1,0-8.34,8.33A8.34,8.34,0,0,0,35.76,23.45Z"/>
                        <line x1="24.4" x2="24.4" y1="15.7" y2="43.8" stroke="currentColor"/>
                        <line x1="30.4" x2="30.4" y1="15.7" y2="43.8" stroke="currentColor"/>
                    </svg>
                EOF,
                'position' => 4,
            ],
            [
                'name' => 'Double Duplex (Quad) Outlet',
                'asset' => null,
                'data' => <<<'EOF'
                    <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 60 50" class="w-12 h-12">
                        <path fill="none" stroke="currentColor" d="M35.76,19.9a8.34,8.34,0,1,0-8.34,8.34A8.34,8.34,0,0,0,35.76,19.9Z"/>
                        <line x1="24.4" x2="24.4" y1="12.1" y2="40.3" stroke="currentColor"/>
                        <line x1="30.4" x2="30.4" y1="12.1" y2="40.3" stroke="currentColor"/>
                        <line x1="19.6" x2="35.2" y1="22.9" y2="22.9" stroke="currentColor"/>
                        <line x1="19.6" x2="35.2" y1="16.9" y2="16.9" stroke="currentColor"/>
                    </svg>
                EOF,
                'position' => 5,
            ],
        ];

        foreach ($symbols as $symbol) {
            $model = new Symbol;
            $model->uuid = Str::uuid();
            $model->name = $symbol['name'];
            $model->data = trim($symbol['data']);
            $model->asset = $symbol['asset'];
            $model->position = $symbol['position'];
            $model->save();
        }
    }
};
