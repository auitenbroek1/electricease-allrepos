<?php

namespace Database\Factories;

use App\Models\State;
use Illuminate\Database\Eloquent\Factories\Factory;

class MemberFactory extends Factory
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
            'state_id' => State::factory(),
            'zip' => $this->faker->postcode(),
            'mobile' => $this->faker->numerify('##########'),
            'office' => $this->faker->numerify('##########'),
        ];
    }

    public function principal()
    {
        return $this->state(function (array $attributes) {
            return [
                'principal' => true,
            ];
        });
    }
}
