<?php

namespace Database\Seeders;

use App\Models\Unit;
use Illuminate\Database\Seeder;

class UnitSeeder extends Seeder
{
    public function run(): void
    {
        $units = [
            ['name' => 'Each', 'abbreviation' => 'E'],
        ];

        foreach ($units as $unit) {
            Unit::factory([
                'name' => $unit['name'],
                'abbreviation' => $unit['abbreviation'],
            ])->create();
        }
    }
}
