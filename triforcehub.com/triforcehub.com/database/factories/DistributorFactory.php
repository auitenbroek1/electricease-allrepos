<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class DistributorFactory extends Factory
{
    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => $this->faker->company(),
        ];
    }
}
