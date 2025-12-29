<?php

namespace App\Console\Commands;

use App\Models\Assembly;
use Illuminate\Console\Command;
use Illuminate\Support\Str;

// artisan triforce:normalize-5

class Normalize5 extends Command
{
    protected $signature = 'triforce:normalize-5';

    protected $description = 'Verify material assigned to an assembly (mc)';

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
                ['MC Switch Opening', 'MC Receptacle', 'MC Fixture Opening']
            )
        ) {
            if (! Str::of($name)->contains('Track Head')) {
                $this->require_part($parts, 'copper solid armored');
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC HCF']
            )
        ) {
            $this->require_part($parts, 'MC CABLE MC-AP HOSPITAL CARE FACILITY');
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC']
            )
        ) {
            if (Str::of($category)->contains('Bracket Box')) {
                $this->require_part($parts, 'deep square box');

                if (Str::of($category)->contains('Wall Mount')) {
                    $this->require_part($parts, 'deep square box', 1);
                }
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC']
            )
        ) {
            if (Str::of($category)->contains('Mason Box')) {
                $this->require_part($parts, 'one gang masonry boxes', 1);
            }

            if (Str::of($category)->contains('Bell Box')) {
                $this->require_part($parts, 'one gang cast weatherproof boxes', 1);
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC']
            )
        ) {
            if (Str::of($category)->contains('Snap In / DC')) {
                $this->require_part($parts, 'snap in quick connector');
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC Fixture Opening']
            )
        ) {
            if (Str::of($category)->contains('With Fixture')) {
                if (
                    ! Str::of($name)->contains('Track Head')
                    && ! Str::of($name)->contains('Wall')
                    && ! Str::of($category)->contains('Wall')
                ) {
                    $this->require_part($parts, 'emt clips');
                }
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC', '_EMT_']
            )
        ) {
            if (Str::of($category)->contains('Switch Opening')) {
                $this->require_part($parts, 'switch cover plates');
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC', '_EMT_']
            )
        ) {
            if (Str::of($category)->contains('Receptacle Opening')) {
                if (Str::of($name)->contains('Power Opening')) {

                } elseif (Str::of($name)->contains('In Use Cover')) {

                } elseif (Str::of($name)->contains('Single Receptacle')) {
                    $this->require_part($parts, 'single receptacle cover plate');
                } elseif (Str::of($name)->contains('(1) GFCI + (1) Duplex')) {
                    $this->require_part($parts, '1 duplex receptacle / 1 decora cover');
                } elseif (Str::of($name)->contains('(1) GFCI')) {
                    $this->require_part($parts, 'decorator wiring device plates');
                } else {
                    $this->require_part($parts, 'duplex receptacle cover plates');
                }
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC']
            )
        ) {
            if (Str::of($category)->contains('Telescoping')) {
                $this->require_part($parts, 'telescoping', 1);
            }
        }

        if (
            $this->is_category_match(
                $category,
                ['_EMT_']
            )
        ) {
            if (Str::of($category)->contains('Telescoping')) {
                $this->require_part($parts, 'telescoping', 3);
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC', '_EMT_']
            )
        ) {
            if (Str::of($category)->contains('Suspended Ceiling')) {
                $this->require_part($parts, 'adjustable bar hanger');
            }

            if (Str::of($name)->contains('Ceiling Mount')) {
                $this->require_part($parts, 'adjustable bar hanger');
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC']
            )
        ) {
            if (Str::of($category)->contains('Cable')) {
                $this->require_part($parts, '12g ground pigtail', 1);
                $this->require_part($parts, 'wire nuts red');

                if (! Str::of($category)->contains('Suspended Ceiling')) {
                    $this->require_part($parts, '3/8 1 hole flex strap');
                }

                if (
                    ! Str::of($category)->contains('Suspended Ceiling')
                    && ! Str::of($category)->contains('Mason Box')
                ) {
                    $this->require_part($parts, '1/2 phillips wafer head screw');
                }
            }
        }

        if (
            $this->is_category_match(
                $category,
                ['_EMT_']
            )
        ) {
            $this->require_part($parts, '12g ground pigtail', 1);
            $this->require_part($parts, 'wire nuts red');

            if (! Str::of($category)->contains('Mason Box')) {
                $this->require_part($parts, '1/2 phillips wafer head screw');
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC', '_EMT_']
            )
        ) {
            $quantity_should_be_12_2 = 0;
            $quantity_should_be_12_3 = 0;

            //

            if (Str::of($name)->contains('Single Pole')) {
                if (Str::of($name)->contains('(1) Single Pole')) {
                    $quantity_should_be_12_2 = 12;
                }
                if (Str::of($name)->contains('(2) Single Pole')) {
                    $quantity_should_be_12_2 = 24;
                }
                if (Str::of($name)->contains('(3) Single Pole')) {
                    $quantity_should_be_12_2 = 36;
                }
                if (Str::of($name)->contains('(4) Single Pole')) {
                    $quantity_should_be_12_2 = 48;
                }
            }

            if (Str::of($name)->contains('3-Way')) {
                if (Str::of($name)->contains('(1) 3-Way')) {
                    $quantity_should_be_12_3 = 12;
                }
                if (Str::of($name)->contains('(2) 3-Way')) {
                    $quantity_should_be_12_3 = 24;
                }
                if (Str::of($name)->contains('(3) 3-Way')) {
                    $quantity_should_be_12_3 = 36;
                }
                if (Str::of($name)->contains('(4) 3-Way')) {
                    $quantity_should_be_12_3 = 48;
                }
            }

            if (Str::of($name)->contains('4-Way')) {
                if (Str::of($name)->contains('(1) 4-Way')) {
                    $quantity_should_be_12_3 += 24;
                }
                if (Str::of($name)->contains('(2) 4-Way')) {
                    $quantity_should_be_12_3 += 48;
                }
            }

            //

            $cable_1 = '#12-2';
            $part_1 = 'copper solid armored (mc) cable '.$cable_1;

            $cable_2 = '#12-3';
            $part_2 = 'copper solid armored (mc) cable '.$cable_2;

            if (Str::of($category)->contains('HCF')) {
                $part_1 = 'MC CABLE MC-AP HOSPITAL CARE FACILITY '.Str::of($cable_1)->replace('#', '')->replace('-', '/');
                $part_2 = 'MC CABLE MC-AP HOSPITAL CARE FACILITY '.Str::of($cable_2)->replace('#', '')->replace('-', '/');
            }

            if ($quantity_should_be_12_2) {
                $this->require_part($parts, $part_1, $quantity_should_be_12_2);
            }
            if ($quantity_should_be_12_3) {
                $this->require_part($parts, $part_2, $quantity_should_be_12_3);
            }

            //

            if (
                Str::of($name)->contains('Single Pole')
                || Str::of($name)->contains('-Way')
            ) {
                $q1 = $quantity_should_be_12_2 + $quantity_should_be_12_3;

                $q2 = $q1 / 12;
                $this->require_part($parts, 'quick connector', $q2);

                $q3 = $q1 / 6;
                $this->require_part($parts, '1 hole flex strap', $q3);

                $q4 = 0;
                if ($q1 == 12) {
                    $q4 = 4;
                }
                if ($q1 == 24) {
                    $q4 = 6;
                }
                if ($q1 == 36) {
                    $q4 = 8;
                }
                if ($q1 == 48) {
                    $q4 = 10;
                }
                $this->require_part($parts, 'phillips wafer head', $q4);
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC', '_EMT_']
            )
        ) {
            if (Str::of($name)->contains('1 Gang')) {
                $this->require_part($parts, 'cover plates 1 gang');
            }

            if (Str::of($name)->contains('2 Gang')) {
                $this->require_part($parts, 'cover plates 2 gang');
            }

            if (Str::of($name)->contains('3 Gang')) {
                $this->require_part($parts, 'cover plates 3 gang');
            }

            if (Str::of($name)->contains('4 Gang')) {
                $this->require_part($parts, 'cover plates 4 gang');
            }

            if (Str::of($name)->contains('Quad') && ! Str::of($name)->contains('(1) GFCI + (1) Duplex')) {
                $this->require_part($parts, 'cover plates 2 gang');
            }

            if (Str::of($name)->contains('NEMA 6-20R')) {
                $this->require_part($parts, 'cover plates 1 gang', 1);
            }

            if (Str::of($name)->contains('GFCI + (1) Duplex')) {
                $this->require_part($parts, 'duplex', 1);
            }
        }

        // endregion

        // region

        if (
            $this->is_category_match(
                $category,
                ['MC', '_EMT_']
            )
        ) {
            $part1 = null;
            $q1 = 0;
            if (Str::of($name)->contains('(1) Single Pole')) {
                $part1 = 'single pole, white';
                $q1 = 1;
            }
            if (Str::of($name)->contains('(2) Single Pole')) {
                $part1 = 'single pole, white';
                $q1 = 2;
            }
            if (Str::of($name)->contains('(3) Single Pole')) {
                $part1 = 'single pole, white';
                $q1 = 3;
            }
            if (Str::of($name)->contains('(4) Single Pole')) {
                $part1 = 'single pole, white';
                $q1 = 4;
            }
            if ($part1) {
                $this->require_part($parts, $part1, $q1);
            }

            $part2 = null;
            $q2 = 0;
            if (Str::of($name)->contains('(1) 3-Way')) {
                $part2 = 'three-way, white';
                $q2 = 1;
            }
            if (Str::of($name)->contains('(2) 3-Way')) {
                $part2 = 'three-way, white';
                $q2 = 2;
            }
            if (Str::of($name)->contains('(3) 3-Way')) {
                $part2 = 'three-way, white';
                $q2 = 3;
            }
            if (Str::of($name)->contains('(4) 3-Way')) {
                $part2 = 'three-way, white';
                $q2 = 4;
            }
            if ($part2) {
                $this->require_part($parts, $part2, $q2);
            }

            $part3 = null;
            $q3 = 0;
            if (Str::of($name)->contains('(1) 4-Way')) {
                $part3 = 'four-way, white';
                $q3 = 1;
            }
            if (Str::of($name)->contains('(2) 4-Way')) {
                $part3 = 'four-way, white';
                $q3 = 2;
            }
            if (Str::of($name)->contains('(3) 4-Way')) {
                $part3 = 'four-way, white';
                $q3 = 3;
            }
            if (Str::of($name)->contains('(4) 4-Way')) {
                $part3 = 'four-way, white';
                $q3 = 4;
            }
            if ($part3) {
                $this->require_part($parts, $part3, $q3);
            }

            $part4 = null;
            $q4 = 0;
            if (Str::of($name)->contains('(1) Duplex')) {
                $part4 = '20a duplex receptacle';
                $q4 = 1;
            }
            if (Str::of($name)->contains('(2) Duplex')) {
                $part4 = '20a duplex receptacle';
                $q4 = 2;
            }
            if (Str::of($name)->contains('(3) Duplex')) {
                $part4 = '20a duplex receptacle';
                $q4 = 3;
            }
            if (Str::of($name)->contains('(4) Duplex')) {
                $part4 = '20a duplex receptacle';
                $q4 = 4;
            }
            if ($part4) {
                $this->require_part($parts, $part4, $q4);
                if (Str::of($name)->contains('Weather')) {
                    $this->require_part($parts, 'weather resistant', $q4);
                    $this->require_part($parts, 'weatherproof 1g', $q4);
                }
            }

            $part5 = null;
            $q5 = 0;
            if (Str::of($name)->contains('(1) GFCI')) {
                $part5 = '20a gfci receptacle';
                $q5 = 1;
            }
            if (Str::of($name)->contains('(2) GFCI')) {
                $part5 = '20a gfci receptacle';
                $q5 = 2;
            }
            if (Str::of($name)->contains('(3) GFCI')) {
                $part5 = '20a gfci receptacle';
                $q5 = 3;
            }
            if (Str::of($name)->contains('(4) GFCI')) {
                $part5 = '20a gfci receptacle';
                $q5 = 4;
            }
            if ($part5) {
                $this->require_part($parts, $part5, $q5);
                if (Str::of($name)->contains('Weather')) {
                    $this->require_part($parts, 'weather resistant', $q5);
                    $this->require_part($parts, 'weatherproof 1g', $q5);
                }
            }
        }

        // endregion

        // region

        // $logs = [];
        // $logs[] = ['white', ["# $category / $name"]];

        if (
            $this->is_category_match(
                $category,
                ['MC', '_EMT_']
            )
        ) {
            if (Str::of($name)->contains('Ceiling Mount')) {
                $this->require_part($parts, 'octagon', 1);
            }

            if (Str::of($name)->contains('Track Light')) {
                $this->require_part($parts, 'octagon', 1);
            }
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
