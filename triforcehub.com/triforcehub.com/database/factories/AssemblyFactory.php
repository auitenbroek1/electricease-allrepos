<?php

namespace Database\Factories;

use App\Models\Assembly;
use App\Models\AssemblyCategory;
use App\Models\Part;
use Illuminate\Database\Eloquent\Factories\Factory;
use Throwable;

class AssemblyFactory extends Factory
{
    private static $i = 1;

    public function configure()
    {
        return $this->afterMaking(function (Assembly $assembly) {
            //
        })->afterCreating(function (Assembly $assembly) {
            // try {
            //     $categories = AssemblyCategory::all()->random($this->faker->numberBetween(0, 3));
            //     $tags = AssemblyCategory::all()->random($this->faker->numberBetween(0, 3));
            //     $parts = Part::all()->random($this->faker->numberBetween(1, 3))->keyBy('id')
            //     ->map(function () {
            //         return ['quantity' => $this->faker->numberBetween(1, 9)];
            //     });

            //     $assembly->categories()->attach($categories);
            //     $assembly->tags()->attach($tags);
            //     $assembly->parts()->attach($parts);
            // } catch (Throwable $exception) {
            // }
        });
    }

    public function definition(): array
    {
        return [
            'uuid' => $this->faker->uuid(),
            'name' => 'Assembly Name '.self::$i++.' '.$this->faker->words(5, true),
        ];
    }
}
