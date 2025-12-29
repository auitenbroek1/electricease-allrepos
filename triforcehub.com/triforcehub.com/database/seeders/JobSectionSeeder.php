<?php

namespace Database\Seeders;

use App\Models\JobSection;
use Illuminate\Database\Seeder;

class JobSectionSeeder extends Seeder
{
    public function run(): void
    {
        JobSection::factory([
            'name' => 'Notes',
        ])->create();

        JobSection::factory([
            'name' => 'Scope of Work',
        ])->create();

        JobSection::factory([
            'name' => 'Terms and Conditions',
        ])->create();
    }
}
