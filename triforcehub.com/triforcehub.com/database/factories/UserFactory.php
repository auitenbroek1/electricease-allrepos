<?php

namespace Database\Factories;

use App\Models\Member;
use Illuminate\Database\Eloquent\Factories\Factory;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Str;

class UserFactory extends Factory
{
    protected static ?string $password;

    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'principal' => false,
            'member_id' => Member::factory(),
            'name' => $this->faker->name(),
            'email' => $this->faker->unique()->safeEmail(),
            'email_verified_at' => now(),
            'password' => static::$password ??= Hash::make('Triforce2022!'),
            'remember_token' => Str::random(10),
        ];
    }

    public function administrator()
    {
        return $this->state(function () {
            return [
                'member_id' => Member::factory()->principal(),
            ];
        });
    }

    public function principal()
    {
        return $this->state(function () {
            return [
                'principal' => true,
            ];
        });
    }

    public function root()
    {
        return $this->state(function () {
            return [
                'principal' => true,
                'member_id' => Member::factory()->principal(),
            ];
        });
    }

    public function unverified()
    {
        return $this->state(function (array $attributes) {
            return [
                'email_verified_at' => null,
            ];
        });
    }
}
