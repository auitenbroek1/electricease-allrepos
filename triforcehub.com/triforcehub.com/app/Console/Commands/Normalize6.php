<?php

namespace App\Console\Commands;

use App\Models\Assembly;
use Illuminate\Console\Command;
use Illuminate\Support\Str;

// artisan triforce:normalize-6

class Normalize6 extends Command
{
    protected $signature = 'triforce:normalize-6';

    protected $description = 'Verify material assigned to an assembly (motor connection)';

    public function handle(): int
    {
        Assembly::disableSearchSyncing();

        Assembly::default()->orderBy('name')->with(['categories', 'parts'])->chunk(100, function ($assemblies) {
            foreach ($assemblies as $assembly) {
                $id = $assembly->id;
                $name = $assembly->name;
                $categories = $assembly->categories;
                $parts = $assembly->parts;

                //

                if ($categories->count() != 1) {
                    continue;
                }

                $category = $categories->first()->name;

                $parts = $parts->map(function ($part) {
                    return [
                        'name' => $part->name,
                        'quantity' => $part->pivot->quantity,
                    ];
                })->toArray();

                $this->check($name, $category, $parts);
            }
        });

        return 0;
    }

    private function check($name, $category, $parts)
    {
        $this->logs = [];

        $this->logs[] = ['white', ["# $category / $name"]];

        // region

        if (
            $this->is_category_match(
                $category,
                ['Motor Connection']
            )
        ) {
            if (Str::of($category)->contains('NEMA 1')) {
                $this->require_part($parts, 'flex aluminum conduit');
                $this->require_part($parts, 'squeeze flex connector', 2);
            }

            if (Str::of($category)->contains('NEMA 3R')) {
                $this->require_part($parts, 'liquid-tight flex conduit');
                $this->require_part($parts, 'liquid-tight flex connector', 2);

                if ($this->has_parts($parts, ['# 10', '# 8'])) {
                    $this->require_part($parts, 'liquid-tight flex conduit 3/4"', 6);
                } elseif ($this->has_parts($parts, ['# 10'])) {
                    $this->require_part($parts, 'liquid-tight flex conduit 3/4"', 6);
                } elseif ($this->has_parts($parts, ['# 10', '# 6'])) {
                    $this->require_part($parts, 'liquid-tight flex conduit 1"', 6);
                } elseif ($this->has_parts($parts, ['# 4', '# 8'])) {
                    $this->require_part($parts, 'liquid-tight flex conduit 1"', 6);
                } elseif ($this->has_parts($parts, ['# 2/0', '# 6'])) {
                    $this->require_part($parts, 'liquid-tight flex conduit 1-1/2"', 6);
                }
            }

            if (Str::of($category)->contains('1 Phase')) {
                if ($this->has_parts($parts, ['# 10', '# 8'])) {
                    $this->require_part($parts, 'thhn 600', 36);
                }
            }

            if (Str::of($category)->contains('3 Phase')) {
                // $this->require_part($parts, 'thhn 600', 48);
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['Motor Connection']
            )
        ) {
            if (Str::of($name)->contains('35a')) {
                $this->require_part($parts, '# 10');
            }

            if (Str::of($name)->contains('70a')) {
                $this->require_part($parts, '# 10');
                $this->require_part($parts, '# 6');
            }

            if (Str::of($name)->contains('50a')) {
                $this->require_part($parts, '# 10');
                $this->require_part($parts, '# 8');
            }

            if (Str::of($name)->contains('40a')) {
                $this->require_part($parts, '# 10');
                $this->require_part($parts, '# 8');
            }

            if (Str::of($name)->contains('80a')) {
                $this->require_part($parts, '# 8');
                $this->require_part($parts, '# 4');
            }

            if (Str::of($name)->contains('70a')) {
                $this->require_part($parts, '# 10');
                $this->require_part($parts, '# 6');
            }

            if (Str::of($name)->contains('60a')) {
                $this->require_part($parts, '# 10');
                $this->require_part($parts, '# 6');
            }

            if (Str::of($name)->contains('45a')) {
                $this->require_part($parts, '# 10');
                $this->require_part($parts, '# 8');
            }

            if (Str::of($name)->contains('90a')) {
                $this->require_part($parts, '# 8');
                $this->require_part($parts, '# 4');
            }

            if (Str::of($name)->contains('175a')) {
                $this->require_part($parts, '# 2/0');
                $this->require_part($parts, '# 6');
            }

            if (Str::of($name)->contains('Horsepower')) {
                $this->require_part($parts, 'motor');
            }

            $this->require_part($parts, 'hole strap', 2);
            $this->require_part($parts, 'hex head', 6);
        }

        // endregion

        //

        $has_errors = false;
        foreach ($this->logs as $log) {
            [$color, $string] = $log;
            if ($color === 'red') {
                $has_errors = true;
                break;
            }
        }

        if ($has_errors) {
            foreach ($this->logs as $log) {
                [$color, $string] = $log;
                $this->line("<fg=$color>".implode(' / ', $string).'</>');
            }
            $this->newLine();
        }
    }

    private function is_category_match($haystack, $needles)
    {
        foreach ($needles as $needle) {
            if (Str::of($haystack)->startsWith($needle)) {
                return true;
            }
        }

        return false;
    }

    private function has_parts($parts, $needles)
    {
        $found = [];

        foreach ($parts as $part) {
            foreach ($needles as $needle) {
                if (Str::of($part['name'])->lower()->contains($needle)) {
                    $found[] = $part;
                    break;
                }
            }
        }

        return count($needles) === count($found);
    }

    private function require_part($parts, $needle, $quantity = null)
    {
        foreach ($parts as $part) {
            $haystack = Str::of($part['name'])->lower();
            $needle = Str::of($needle)->lower();

            if (Str::of($haystack)->contains($needle)) {
                if ($quantity) {
                    $quantity_is = strval($part['quantity'] * 1);
                    $quantity_should_be = strval($quantity);

                    if ($quantity_is !== $quantity_should_be) {
                        $this->logs[] = ['red', ['wrong quantity', $part['name'], 'is', $quantity_is, 'should be', $quantity_should_be]];

                        return false;
                    }
                }

                // $this->logs[] = ['green', ['found', $needle]];

                return true;
            }
        }

        $this->logs[] = ['red', ['missing', $needle]];

        return false;
    }
}
