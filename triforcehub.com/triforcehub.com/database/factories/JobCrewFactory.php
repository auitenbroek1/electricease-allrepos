<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class JobCrewFactory extends Factory
{
    private static $i = 1;

    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => 'Crew '.self::$i++,
            'quantity' => $this->faker->numberBetween(1, 3),
            'rate' => $this->faker->randomFloat(2, 25, 100),
            'burden' => 25,
            'fringe' => 1.5,
            'notes' => $this->faker->realText($this->faker->numberBetween(64, 512)),
            'enabled' => 1,
        ];
    }
}
