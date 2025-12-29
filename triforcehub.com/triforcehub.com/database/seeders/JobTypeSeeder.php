<?php

namespace Database\Seeders;

use App\Models\JobType;
use Illuminate\Database\Seeder;

class JobTypeSeeder extends Seeder
{
    public function run(): void
    {
        JobType::factory([
            'name' => 'Base Bid',
        ])->create();

        JobType::factory([
            'name' => 'Change Order',
        ])->create();

        JobType::factory([
            'name' => 'Alternate',
        ])->create();
    }
}
