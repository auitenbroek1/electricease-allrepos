<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class JobFactory extends Factory
{
    private static $i = 1;

    public function definition(): array
    {
        $count = self::$i;

        self::$i++;

        return [
            'uuid' => $this->faker->uuid(),
            'member_id' => 1,
            'job_type_id' => 1,
            'number' => 'KB-'.(1000 + $count),
            'name' => 'Job Name '.$count,
            'temporary_power' => $this->faker->boolean(),
            'temporary_lighting' => $this->faker->boolean(),
            'sqft' => $this->faker->numberBetween(1000, 10000),
            'labor_factor' => $this->faker->randomFloat(2, 1, 3),
            'job_status_id' => 1,
        ];
    }
}
