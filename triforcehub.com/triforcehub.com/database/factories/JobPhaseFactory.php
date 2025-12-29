<?php

namespace Database\Factories;

use Illuminate\Database\Eloquent\Factories\Factory;

class JobPhaseFactory extends Factory
{
    private static $i = 1;

    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => 'Phase Name '.self::$i++,
            'area' => 'Area Name',
            'cost_code' => '000',
        ];
    }
}
