<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class JobAssemblyFactory extends Factory
{
    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
        ];
    }
}
