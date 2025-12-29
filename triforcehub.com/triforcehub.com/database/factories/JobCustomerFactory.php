<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class JobCustomerFactory extends Factory
{
    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => $this->faker->company(),
            'email' => $this->faker->unique()->safeEmail(),
            'address1' => $this->faker->streetAddress(),
            'address2' => $this->faker->secondaryAddress(),
            'city' => $this->faker->city(),
            'state_id' => $this->faker->numberBetween(1, 51),
            'zip' => $this->faker->postcode(),
            'mobile' => $this->faker->numerify('##########'),
            'office' => $this->faker->numerify('##########'),
        ];
    }
}
