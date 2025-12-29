<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class UnitFactory extends Factory
{
    private static $i = 1;

    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => 'Unit '.self::$i++,
            'abbreviation' => '--',
        ];
    }
}
