<?php

namespace App\Console\Commands;

use App\Models\Assembly;
use Illuminate\Console\Command;
use Illuminate\Support\Str;

// artisan triforce:normalize-3

class Normalize3 extends Command
{
    protected $signature = 'triforce:normalize-3 {--sql}';

    protected $description = 'Verify material assigned to an assembly (conduits)';

    public function handle(): int
    {
        Assembly::disableSearchSyncing();

        Assembly::default()->orderBy('name')->with(['categories', 'parts'])->chunk(100, function ($assemblies) use (&$categories_to_be_created) {
            foreach ($assemblies as $assembly) {
                $id = $assembly->id;
                $name = $assembly->name;
                $categories = $assembly->categories;
                $parts = $assembly->parts;

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

                $this->check($id, $name, $category, $parts);
            }
        });

        return 0;
    }

    private function check($id, $name, $category, $parts)
    {
        $segments = [
            'conduit' => '',
            'trade_size' => '',
            'application' => '',
            'fitting' => '',
            'casting' => '',
            'metal' => '',
            'wires' => '',
        ];

        // region conduit

        $conduits = [
            'Aluminum Rigid Conduit' => 'Aluminum Rigid Conduit',
            'EMT Feeders' => 'EMT',
            'EMT Branch' => 'EMT',
            'EMT' => 'EMT',
            'Galvanized Rigid Conduit' => 'Galvanized Rigid Conduit',
            'PVC' => 'PVC',
        ];

        foreach ($conduits as $key => $value) {
            if (Str::of($category)->startsWith($key)) {
                if (
                    Str::of($category)->contains('Receptacle Opening')
                    || Str::of($category)->contains('Device Opening')
                    || Str::of($category)->contains('Switch Opening')
                    || Str::of($category)->contains('Stub')
                ) {
                    continue;
                }
                $segments['conduit'] = $value;
                break;
            }
        }

        // endregion

        // region trade size

        $trade_size_pattern = '/(([0-9]\-)?([0-9])(\/[0-9])?(\"))/';

        // category

        $segments['trade_size'] = Str::of($category)->match($trade_size_pattern);

        if (Str::of($segments['trade_size'])->length()) {
            if (! Str::of($category)->contains($segments['trade_size'])) {
                $segments['trade_size'] = '<fg=yellow>'.$segments['trade_size'].'</>';
            }
        }

        // name

        if (! Str::of($segments['trade_size'])->length()) {
            $segments['trade_size'] = Str::of($name)->match($trade_size_pattern);

            if (Str::of($segments['trade_size'])->length()) {
                if (! Str::of($name)->contains($segments['trade_size'])) {
                    $segments['trade_size'] = '<fg=yellow>'.$segments['trade_size'].'</>';
                }
            }
        }

        // fallback

        if (! Str::of($segments['trade_size'])->length()) {
            $segments['trade_size'] = '<fg=red>missing trade size</>';
        }

        // endregion

        // region application

        $applications = [
            'Beam',
            'Elbow',
            'Slab',
            'Strap',
            'Strut',
            'Underground',
        ];

        foreach ($applications as $application) {
            if (Str::of($category)->contains($application)) {
                $segments['application'] = $application;
                break;
            }
        }

        if (Str::of($name)->contains('Elbow')) {
            $segments['application'] = 'Elbow';
        }

        if (! Str::of($segments['application'])->length()) {
            $segments['application'] = '<fg=red>missing application</>';
        }

        // endregion

        // region fitting

        $fittings = [
            'Compression',
            'Set Screw',
        ];

        foreach ($fittings as $fitting) {
            if (Str::of($category)->contains($fitting)) {
                $segments['fitting'] = $fitting;
                break;
            }
        }

        if (in_array($segments['conduit'], ['EMT'])) {
            if (! Str::of($segments['fitting'])->length()) {
                $segments['fitting'] = '<fg=red>missing fitting</>';
            }
        }

        // endregion

        // region castings

        $castings = [
            'Die Cast' => 'DC',
            'Stainless Steel' => 'SS',
        ];

        foreach ($parts as $part) {
            if (! Str::of($segments['casting'])->length()) {
                foreach ($castings as $key => $value) {
                    if (Str::of($part['name'])->lower()->contains(Str::of($key)->lower())) {
                        $segments['casting'] = $value;
                        break;
                    }
                }
            }
        }

        // endregion

        // region metal

        $metals = [
            'Aluminum / THHN-THWN' => 'Aluminum / THHN-THWN',
            'Aluminum' => 'Aluminum / THHN-THWN',
            'Alum' => 'Aluminum / THHN-THWN',

            'Copper / Solid' => 'Copper / Solid / THHN',
            'Copper / Stranded' => 'Copper / Stranded / THHN',
            'Copper / THHN-THWN' => 'Copper / THHN',
            'Copper' => 'Copper / THHN',

            'SOLID COPPER' => 'Copper / Solid / THHN',
            'STRANDED COPPER' => 'Copper / Stranded / THHN',
        ];

        // category

        $category_without_conduit = Str::of($category)->replace($segments['conduit'], '');

        foreach ($metals as $key => $value) {
            if (Str::of($category_without_conduit)->contains($key)) {
                $segments['metal'] = $value;
                break;
            }
        }

        if (Str::of($segments['metal'])->length()) {
            if (! Str::of($category)->contains($segments['metal'])) {
                $segments['metal'] = '<fg=yellow>'.$segments['metal'].'</>';
            }
        }

        // name

        $name_without_conduit = Str::of($name)->replace($segments['conduit'], '');

        if (! Str::of($segments['metal'])->length()) {
            foreach ($metals as $key => $value) {
                if (Str::of($name_without_conduit)->contains($key)) {
                    $segments['metal'] = $value;
                    break;
                }
            }

            if (Str::of($segments['metal'])->length()) {
                if (! Str::of($name)->contains($segments['metal'])) {
                    $segments['metal'] = '<fg=yellow>'.$segments['metal'].'</>';
                }
            }
        }

        // parts

        foreach ($parts as $part) {
            if (! Str::of($segments['metal'])->length()) {
                foreach ($metals as $key => $value) {
                    if (Str::of($part['name'])->contains($key)) {
                        $segments['metal'] = $value;
                        break;
                    }
                }
            }
        }

        // fallback

        if (! Str::of($segments['metal'])->length()) {
            $segments['metal'] = '<fg=red>missing metal</>';
        }

        if ($segments['application'] == 'Elbow') {
            $segments['metal'] = '';
        }

        // endregion

        // region wires

        // TODO: remove this hack
        $name = Str::of($name)->replace('PVC with 4 10s', 'PVC with 4 # 10s');
        if (Str::of($name)->endsWith('PVC Empty')) {
            $name = Str::of($name)->replace('PVC Empty', 'PVC Empty with Pull Line');
        }
        //

        $wires_pattern = '/(([0-9])( \# )([0-9]+)(\/[0-9])?)/';

        $segments['wires'] = Str::of($name)->matchAll($wires_pattern)->toArray();
        $segments['wires'] = implode(' - ', $segments['wires']);

        // if found in name
        if (Str::of($segments['wires'])->length()) {
            // but not an exact match
            if (! Str::of($name)->contains($segments['wires'])) {
                $segments['wires'] = '<fg=yellow>'.$segments['wires'].'</>';
            }
        }

        if (! Str::of($segments['wires'])->length()) {
            foreach (['Pull Line', 'Pull line'] as $empty) {
                if (Str::of($name)->contains($empty)) {
                    $segments['metal'] = '';
                    $segments['wires'] = 'Empty / Pull Line';
                    break;
                }
            }
        }

        // if not found at all
        if (! Str::of($segments['wires'])->length()) {
            $segments['wires'] = '<fg=red>missing wires</>';
        }

        if ($segments['application'] == 'Elbow') {
            $segments['wires'] = '';
        }

        //

        if (
            Str::of($segments['wires'])->contains('# 6') ||
            Str::of($segments['wires'])->contains('# 8') ||
            Str::of($segments['wires'])->contains('# 10')
        ) {
            $segments['metal'] = Str::of($segments['metal'])->replace(' / Stranded / ', ' / ');
        }

        // endregion

        // only look at conduits for now
        if (! strlen($segments['conduit'])) {
            // $this->error("$category / $name");
            return;
        }

        $new_category = [];
        $new_category[] = $segments['conduit'] ?? '';
        $new_category[] = $segments['trade_size'] ?? '';
        $new_category[] = $segments['application'] ?? '';
        if (strlen($segments['fitting'])) {
            $new_category[] = $segments['fitting'] ?? '';
        }
        if (strlen($segments['casting'])) {
            $new_category[] = $segments['casting'] ?? '';
        }
        $new_category = implode(' / ', $new_category);

        $new_name = [];
        if (strlen($segments['metal'])) {
            $new_name[] = $segments['metal'] ?? '';
        }
        $new_name[] = $segments['wires'] ?? '';
        $new_name = implode(' / ', $new_name);

        if (! Str::of($new_name)->length()) {
            if ($segments['application'] == 'Elbow') {
                $new_name = $new_category;
            }
        }

        if (! Str::of($new_name)->length()) {
            $new_name = '<fg=red>missing name</>';
        }

        //

        if ($name != $new_name || $category != $new_category) {
            $this->line("$id / <fg=gray>$category / $name</> -> <fg=green>$new_category / $new_name</>");
            if ($segments['application'] == 'Elbow') {
                // $this->line($parts);
                // $this->newLine();
            }
        }

        // conduit
        // trade size
        // application
        // fitting
        // casting
        // metal
        // wires

        $this->check_trade_size($name, $category, $segments['conduit'], $segments['trade_size'], $parts);
        $this->check_application($name, $category, $segments['trade_size'], $segments['application'], $parts);
        $this->check_fitting_casting($name, $category, $segments['application'], $segments['fitting'], $segments['casting'], $parts);
        $this->check_wires($name, $category, $segments['wires'], $parts);
    }

    private function check_trade_size($name, $category, $counduit, $trade_size, $parts)
    {
        if (! Str::of($trade_size)->length()) {
            return;
        }

        $has_errors = false;

        $logs = [];

        $logs[] = ['white', "# $category / $name"];

        $logs[] = ['yellow', "Look for '$counduit' and '$trade_size' in these parts:"];
        $found = false;
        foreach ($parts as $part) {
            $part_name = Str::of($part['name'])->replace('GALVANIZED RIGID STEEL CONDUIT', 'Galvanized Rigid Conduit');
            $trade_size = Str::of($trade_size)->replace('"', '');
            if (
                Str::of($part_name)->lower()->contains(Str::of($counduit)->lower()) &&
                Str::of($part_name)->contains("$trade_size")
            ) {
                $found = true;
                $logs[] = ['green', $part['name']];
            } else {
                $logs[] = ['gray', $part['name']];
            }
        }
        if (! $found) {
            $logs[] = ['red', 'not found (error)'];
            $has_errors = true;
        }

        if ($has_errors) {
            foreach ($logs as $log) {
                [$color, $string] = $log;
                $this->line("<fg=$color>$string</>");
            }
            $this->newLine();
        }
    }

    private function check_application($name, $category, $trade_size, $application, $parts)
    {
        if (! Str::of($application)->length()) {
            return;
        }

        //

        $map = [
            'Beam' => [
                [
                    'name' => 'Beam Clamp',
                    'quantity' => 0.2,
                ],
            ],
            'Strap' => [
                [
                    'name' => 'Strap',
                    'quantity' => 0.2,
                ],
                [
                    'name' => 'Hex Head Screw',
                    'quantity' => 0.2,
                ],
            ],
            'Strut' => [
                [
                    'name' => 'Channel Clamp',
                    'quantity' => 0.2,
                ],
            ],
            'Underground' => [
                [
                    'name' => 'FLAG TAPE',
                    'quantity' => 1,
                ],
                [
                    'name' => 'HARD LABOR',
                    'quantity' => 1,
                ],
                // TODO: just for pvc, just underground, conduit part should be *with* coupling, and the qty is 1
                // [
                //     'name' => 'WITH COUPLING',
                //     'quantity' => 1,
                // ],
                // this is working for most, but there is a problem with schedule 40
            ],
        ];

        if ($application === 'Strap') {
            foreach ($parts as $part) {
                if (Str::of($part['name'])->lower()->contains('two hole')) {
                    $map[$application][1] = [
                        'name' => 'Hex Head Screw',
                        'quantity' => 0.4,
                    ];
                }
            }
        }

        $items = $map[$application] ?? [];

        //

        $has_errors = false;
        $logs = [];
        $logs[] = ['white', "# $category / $name"];

        foreach ($items as $item) {
            $look_for_name = $item['name'];
            $look_for_quantity = $item['quantity'];
            $logs[] = ['yellow', "Look for '$look_for_name' in these parts:"];
            $found = false;
            foreach ($parts as $part) {
                if (
                    Str::of($part['name'])->lower()->contains(Str::of($look_for_name)->lower())
                ) {
                    $found = true;
                    // name contains the size, so the quantity not matching is a problem
                    $quantity_is = strval($part['quantity'] * 1);
                    $quantity_should_be = strval($look_for_quantity);
                    if ($quantity_is == $quantity_should_be) {
                        $logs[] = ['green', $part['name']." / $quantity_is = $quantity_should_be (match)"];
                    } else {
                        $logs[] = ['red', $part['name']." / $quantity_is != $quantity_should_be (error)"];
                        $has_errors = true;
                    }
                    // if stap, also check trade size
                    if ($application === 'Strap') {
                        if (Str::of($part['name'])->lower()->contains('strap')) {
                            if (! Str::of($part['name'])->lower()->contains(Str::of($trade_size)->replace('"', ''))) {
                                $logs[] = ['red', $part['name'].' (wrong trade size)'];
                                $has_errors = true;
                            }
                        }
                    }
                } else {
                    $logs[] = ['gray', $part['name']];
                }
                // $logs[] = ['white', $part['name'] . ': ' . $part['quantity']];
            }
            if (! $found) {
                $logs[] = ['red', 'not found (error)'];
                $has_errors = true;
            }
        }
        if ($has_errors) {
            foreach ($logs as $log) {
                [$color, $string] = $log;
                $this->line("<fg=$color>$string</>");
            }
            $this->newLine();
        } else {
            // $this->line("<fg=green>good</>");
        }
    }

    private function check_fitting_casting($name, $category, $application, $fitting, $casting, $parts)
    {
        if (! Str::of($fitting)->length()) {
            return;
        }

        if (! Str::of($casting)->length()) {
            return;
        }

        //

        $casting = Str::of($casting)->replace('DC', 'Die Cast');

        //

        $has_errors = false;

        $logs = [];

        $logs[] = ['white', "# $category / $name"];

        $logs[] = ['yellow', "Look for '$fitting' and '$casting' in these parts:"];
        $found = false;
        foreach ($parts as $part) {
            $part_name = Str::of($part['name']);

            if (
                Str::of($part_name)->lower()->contains(Str::of($fitting)->lower())
                && Str::of($part_name)->lower()->contains(Str::of($casting)->lower())
            ) {
                $found = true;

                $quantity_is = strval($part['quantity'] * 1);
                $quantity_should_be = '0.1';
                if ($application === 'Elbow') {
                    $quantity_should_be = '1';
                }
                if ($quantity_is == $quantity_should_be) {
                    $logs[] = ['green', $part['name']." / $quantity_is = $quantity_should_be (match)"];
                } else {
                    $logs[] = ['red', $part['name']." / $quantity_is != $quantity_should_be (error)"];
                    $has_errors = true;
                }
            } else {
                $logs[] = ['gray', $part['name']];
            }
        }
        if (! $found) {
            $logs[] = ['red', 'not found (error)'];
            $has_errors = true;
        }

        if ($has_errors) {
            foreach ($logs as $log) {
                [$color, $string] = $log;
                $this->line("<fg=$color>$string</>");
            }
            $this->newLine();
        }
    }

    private function check_wires($name, $category, $wires, $parts)
    {
        if (! Str::of($wires)->length()) {
            return;
        }

        $has_errors = false;

        $logs = [];

        $logs[] = ['white', "# $category / $name"];

        $items = explode(' - ', $wires);
        foreach ($items as $item) {
            if (Str::of($item)->contains('#')) {
                [$quantity, $size] = explode(' # ', $item);
                $logs[] = ['yellow', "Look for '# $size' in these parts:"];
                $found = false;
                foreach ($parts as $part) {
                    if (
                        Str::of($part['name'])->contains("# $size ") ||
                        Str::of($part['name'])->contains("#$size ") ||
                        Str::of($part['name'])->endsWith("# $size") ||
                        Str::of($part['name'])->endsWith("#$size") ||
                        ($size == '3' && Str::of($part['name'])->endsWith(" $size BLACK"))
                    ) {
                        $found = true;

                        // name contains the size, so the quantity not matching is a problem
                        $quantity_is = strval($part['quantity'] * 1);
                        $quantity_should_be = strval($quantity * 1.1);

                        if ($quantity_is == $quantity_should_be) {
                            $logs[] = ['green', $part['name']." / $quantity_is = $quantity_should_be (match)"];
                        } else {
                            $logs[] = ['red', $part['name']." / $quantity_is != $quantity_should_be (error)"];
                            $has_errors = true;
                        }
                    } else {
                        $logs[] = ['gray', $part['name']];
                    }
                    // $logs[] = ['white', $part['name'] . ': ' . $part['quantity']];
                }
                if (! $found) {
                    $logs[] = ['red', 'not found (error)'];
                    $has_errors = true;
                }
            } else {
                // $this->error($item);
            }
        }

        if ($has_errors) {
            foreach ($logs as $log) {
                [$color, $string] = $log;
                $this->line("<fg=$color>$string</>");
            }
            $this->newLine();
        }
    }

    // TODO: if greater than 2"???
}
