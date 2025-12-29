<?php

namespace App\Console\Commands;

use App\Models\Assembly;
use App\Models\AssemblyCategory;
use Illuminate\Console\Command;
use Illuminate\Support\Str;

// artisan triforce:normalize-2

// delete all duplicates of duplicates
// leave only one of each under the main account
// we can leave duplicates within member accounts

class Normalize2 extends Command
{
    protected $signature = 'triforce:normalize-2 {--sql} {--categories}';

    protected $description = 'Normalize assembly to category assignment';

    public function handle(): int
    {
        $this->sql = $this->option('sql');
        $this->categories = $this->option('categories');

        $categories_to_be_created = [];

        // region

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
                    return $part->name;
                })->toArray();

                [$new_name, $new_category] = $this->normalize($name, $category, $parts);

                if ($this->sql) {
                    if ($this->categories) {
                        if ($category != $new_category) {
                            $category_to_assign_to = AssemblyCategory::default()->where('name', $new_category)->first();

                            if (! $category_to_assign_to) {
                                $categories_to_be_created[] = $new_category;
                            }
                        }
                    } else {
                        if ($name != $new_name) {
                            $category_to_assign_to = AssemblyCategory::default()->where('name', $new_category)->first();
                            if ($category_to_assign_to) {
                                $this->line("-- id: $id");
                                $this->line("-- name: $name");
                                if ($name != $new_name) {
                                    $escaped_name = Str::replace("'", "''", $new_name);
                                    $this->line("UPDATE `assemblies` SET `name` = '$escaped_name' WHERE `id` = $id;");
                                }
                                if ($category != $new_category) {
                                    $this->line("-- category: $category");
                                    $this->line("-- new category: $new_category");
                                    $this->line("-- new category: $category_to_assign_to->name");
                                    $this->line("UPDATE `assembly_assembly_category` SET `assembly_category_id` = $category_to_assign_to->id WHERE `assembly_id` = $id;");
                                }
                                $this->newLine();
                            } else {
                                $this->error("-- missing category: $new_category");
                            }
                        }
                    }
                }
            }
        });

        // endregion

        $categories_to_be_created = array_unique($categories_to_be_created);

        foreach ($categories_to_be_created as $category_to_be_created) {
            $escaped_category = Str::replace("'", "''", $category_to_be_created);
            $uuid = Str::uuid();
            $this->line("INSERT INTO `assembly_categories` SET `uuid` = '$uuid', `name` = '$escaped_category';");
            $this->newLine();
        }

        return 0;
    }

    private function normalize($name, $category, $parts)
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
            // 'Aluminum Rigid Conduit' => 'Aluminum Rigid Conduit',
            // 'EMT Feeders' => 'EMT',
            // 'EMT Branch' => 'EMT',
            'Galvanized Rigid Conduit' => 'Galvanized Rigid Conduit',
            // 'PVC' => 'PVC',
        ];

        foreach ($conduits as $key => $value) {
            if (Str::of($category)->startsWith($key)) {
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

        // TODO
        // check the parts, if die cast append DC
        // if stainless steal, add SS (we don't have this yet)

        foreach ($parts as $part) {
            if (! Str::of($segments['casting'])->length()) {
                foreach ($castings as $key => $value) {
                    if (Str::of($part)->lower()->contains(Str::of($key)->lower())) {
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
                    if (Str::of($part)->contains($key)) {
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
            return [$name, $category];
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

        // TODO: if elbow
        // emt / 4" / elbow / compression / dc           emt / 4" / elbow

        if (! Str::of($new_name)->length()) {
            if ($segments['application'] == 'Elbow') {
                $new_name = $new_category;
            }
        }

        if (! Str::of($new_name)->length()) {
            $new_name = '<fg=red>missing name</>';
        }

        //

        if (! $this->sql) {
            if ($name != $new_name || $category != $new_category) {
                $this->line("<fg=gray>$category / $name</> -> <fg=green>$new_category / $new_name</>");
                if ($segments['application'] == 'Elbow') {
                    // $this->line($parts);
                    // $this->newLine();
                }
            }
        }

        return [$new_name, $new_category];
    }
}
