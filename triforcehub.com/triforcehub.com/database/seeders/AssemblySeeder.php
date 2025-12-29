<?php

namespace Database\Seeders;

use App\Models\Assembly;
use Illuminate\Database\Seeder;

class AssemblySeeder extends Seeder
{
    public function run(): void
    {
        Assembly::factory()
            ->count(33)
            ->create();
    }
}
