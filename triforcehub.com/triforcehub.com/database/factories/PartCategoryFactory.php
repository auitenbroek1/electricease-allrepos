<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class PartCategoryFactory extends Factory
{
    private static $i = 1;

    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => 'Part Category '.self::$i++,
        ];
    }
}
