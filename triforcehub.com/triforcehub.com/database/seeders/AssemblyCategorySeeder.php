<?php

namespace Database\Seeders;

use App\Models\AssemblyCategory;
use Illuminate\Database\Seeder;

class AssemblyCategorySeeder extends Seeder
{
    public function run(): void
    {
        AssemblyCategory::factory()
            ->count(33)
            ->create();
    }
}
