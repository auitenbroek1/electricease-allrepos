<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class JobLocationFactory extends Factory
{
    private static $i = 1;

    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => 'Location '.self::$i++,
            'address1' => $this->faker->streetAddress(),
            'address2' => $this->faker->secondaryAddress(),
            'city' => $this->faker->city(),
            'state_id' => $this->faker->numberBetween(1, 51),
            'zip' => $this->faker->postcode(),
        ];
    }
}
