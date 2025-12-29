<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class JobLaborFactory extends Factory
{
    private static $i = 1;

    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => 'Labor '.self::$i++,
            'hours' => $this->faker->randomFloat(2, 1, 10),
            'rate' => $this->faker->randomFloat(2, 25, 100),
            'burden' => 25,
            'fringe' => 1.5,
            'notes' => $this->faker->realText($this->faker->numberBetween(64, 512)),
            'enabled' => $this->faker->boolean(),
        ];
    }
}
