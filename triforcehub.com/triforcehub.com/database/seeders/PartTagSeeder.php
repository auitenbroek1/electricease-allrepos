<?php

namespace Database\Seeders;

use App\Models\PartTag;
use Illuminate\Database\Seeder;

class PartTagSeeder extends Seeder
{
    public function run(): void
    {
        PartTag::factory()
            ->count(33)
            ->create();
    }
}
