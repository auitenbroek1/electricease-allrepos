<?php

namespace Database\Seeders;

use App\Models\AssemblyTag;
use Illuminate\Database\Seeder;

class AssemblyTagSeeder extends Seeder
{
    public function run(): void
    {
        AssemblyTag::factory()
            ->count(33)
            ->create();
    }
}
