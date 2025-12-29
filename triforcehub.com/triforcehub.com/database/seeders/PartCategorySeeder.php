<?php

namespace Database\Seeders;

use App\Models\PartCategory;
use Illuminate\Database\Seeder;

class PartCategorySeeder extends Seeder
{
    public function run(): void
    {
        PartCategory::factory()
            ->count(33)
            ->create();
    }
}
