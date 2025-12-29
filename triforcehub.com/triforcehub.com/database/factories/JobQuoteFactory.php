<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class JobQuoteFactory extends Factory
{
    private static $i = 1;

    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => 'Quote '.self::$i++,
            'cost' => $this->faker->randomFloat(2, 100, 1000),
            'notes' => $this->faker->realText($this->faker->numberBetween(64, 512)),
            'enabled' => $this->faker->boolean(),
        ];
    }
}
