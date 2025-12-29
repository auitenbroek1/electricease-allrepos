<?php

namespace Database\Seeders;

use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;

class DatabaseSeeder extends Seeder
{
    use WithoutModelEvents;

    public function run(): void
    {
        $this->call([
            StateSeeder::class,
            UnitSeeder::class,

            MemberSeeder::class,
            UserSeeder::class,

            DistributorSeeder::class,

            JobTypeSeeder::class,
            JobStatusSeeder::class,
            JobSectionSeeder::class,

            PartCategorySeeder::class,
            PartTagSeeder::class,
            PartSeeder::class,

            AssemblyCategorySeeder::class,
            AssemblyTagSeeder::class,
            AssemblySeeder::class,

            JobSeeder::class,
        ]);
    }
}
