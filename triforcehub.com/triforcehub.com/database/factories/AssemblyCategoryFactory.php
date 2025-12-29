<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class AssemblyCategoryFactory extends Factory
{
    private static $i = 1;

    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => 'Assembly Category '.self::$i++,
        ];
    }
}
