<?php

namespace App\Console\Commands;

use App\Models\Assembly;
use Illuminate\Console\Command;
use Illuminate\Support\Str;

// artisan triforce:normalize-7

class Normalize7 extends Command
{
    protected $signature = 'triforce:normalize-7';

    protected $description = 'Verify material assigned to an assembly (emt device)';

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

                if (Str::of($category)->startsWith('EMT /')) {
                    continue;
                }

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
                ['EMT 1/2']
            )
        ) {
            $this->require_part($parts, 'conduit 1/2');
        }

        if (
            $this->is_category_match(
                $category,
                ['EMT 3/4']
            )
        ) {
            $this->require_part($parts, 'conduit 3/4');
        }

        if (
            $this->is_category_match(
                $category,
                ['EMT']
            )
        ) {
            if (Str::of($category)->contains('Bracket Box')) {
                $this->require_part($parts, 'deep square boxes', 1);
            }
        }

        if (
            $this->is_category_match(
                $category,
                ['EMT']
            )
        ) {
            if (Str::of($category)->contains('Mason Box')) {
                $this->require_part($parts, 'masonry boxes', 1);
            }
        }

        if (
            $this->is_category_match(
                $category,
                ['EMT']
            )
        ) {
            if (Str::of($category)->contains('Bell Box')) {
                $this->require_part($parts, 'one gang cast weatherproof', 1);
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['EMT 1/2"']
            )
        ) {
            if (Str::of($category)->contains('Set Screw / DC')) {
                $this->require_part($parts, 'DIE CAST SET SCREW EMT COUPLINGS 1/2"');
                $this->require_part($parts, 'DIE CAST SET SCREW EMT CONNECTORS 1/2"');
            }
        }

        if (
            $this->is_category_match(
                $category,
                ['EMT 3/4"']
            )
        ) {
            if (Str::of($category)->contains('Set Screw / DC')) {
                $this->require_part($parts, 'DIE CAST SET SCREW EMT COUPLINGS 3/4"');
                $this->require_part($parts, 'DIE CAST SET SCREW EMT CONNECTORS 3/4"');
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['EMT 1/2"']
            )
        ) {
            if (Str::of($category)->contains('Compression / DC')) {
                $this->require_part($parts, 'DIE CAST COMPRESSION EMT COUPLINGS, RAINTIGHT 1/2"');
                $this->require_part($parts, 'DIE CAST COMPRESSION EMT CONNECTORS, RAINTIGHT 1/2"');
            }
        }

        if (
            $this->is_category_match(
                $category,
                ['EMT 3/4"']
            )
        ) {
            if (Str::of($category)->contains('Compression / DC')) {
                $this->require_part($parts, 'DIE CAST COMPRESSION EMT COUPLINGS, RAINTIGHT 3/4"');
                $this->require_part($parts, 'DIE CAST COMPRESSION EMT CONNECTORS, RAINTIGHT 3/4"');
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['EMT 1/2"']
            )
        ) {
            if (! Str::of($category)->contains('Mason Box')) {
                $this->require_part($parts, 'straps 1/2"');
            }
        }

        if (
            $this->is_category_match(
                $category,
                ['EMT 3/4"']
            )
        ) {
            if (! Str::of($category)->contains('Mason Box')) {
                $this->require_part($parts, 'straps 3/4"');
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['EMT']
            )
        ) {
            if (Str::of($category)->contains('Switch Opening')) {
                $this->require_part($parts, 'switch cover plate');
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['EMT']
            )
        ) {
            if (Str::of($category)->contains('Receptacle Opening')) {
                $this->require_part($parts, 'duplex receptacle cover plates');
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['EMT']
            )
        ) {
            if (Str::of($category)->contains('Telescoping')) {
                $this->require_part($parts, '24in telescoping', 3);
            }
        }

        // endregion

        // region

        $this->logs = [['white', ["# $category / $name"]]];

        if (
            $this->is_category_match(
                $category,
                ['EMT']
            )
            && ! Str::of($category)->contains('Low Voltage Stub')
        ) {
            $this->require_part($parts, '12g ground pigtail', 1);
        }

        if (
            $this->is_category_match(
                $category,
                ['EMT']
            )
            && ! Str::of($category)->contains('Low Voltage Stub')
        ) {
            $this->require_part($parts, 'wire nuts red');
        }

        if (
            $this->is_category_match(
                $category,
                ['EMT']
            )
            && ! Str::of($category)->contains('Low Voltage Stub')
            && ! Str::of($category)->contains('Bell Box')
            && ! Str::of($category)->contains('Mason Box')
        ) {
            $this->require_part($parts, 'phillips wafer head');
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
                        $this->logs[] = ['red', ['wrong quantity', $part['name'], 'is', $part['quantity'], 'should be', $quantity]];

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
