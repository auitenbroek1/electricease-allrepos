<?php

namespace Database\Factories;

use App\Models\Part;
use App\Models\PartCategory;
use Illuminate\Database\Eloquent\Factories\Factory;
use Throwable;

class PartFactory extends Factory
{
    private static $i = 1;

    public function configure()
    {
        return $this->afterMaking(function (Part $part) {
            //
        })->afterCreating(function (Part $part) {
            // try {
            //     $categories = PartCategory::all()->random($this->faker->numberBetween(0, 3));
            //     $tags = PartCategory::all()->random($this->faker->numberBetween(0, 3));

            //     $part->categories()->attach($categories);
            //     $part->tags()->attach($tags);
            // } catch (Throwable $exception) {
            // }
        });
    }

    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => 'Part Name '.self::$i++.' '.$this->faker->words(5, true),
            'unit_id' => 1,
            'cost' => $this->faker->randomFloat($this->faker->numberBetween(2, 6), 0, 100),
            'labor' => $this->faker->randomFloat($this->faker->numberBetween(2, 6), 0, 5),
        ];
    }
}
