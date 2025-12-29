<?php

namespace App\Console\Commands;

use App\Models\Assembly;
use App\Models\AssemblyCategory;
use App\Models\Part;
use App\Models\PartCategory;
use Illuminate\Console\Command;
use Illuminate\Support\Str;

// artisan triforce:normalize-1 --all

class Normalize1 extends Command
{
    protected $signature = 'triforce:normalize-1
        {--all}
        {--assemblies}
        {--assembly-categories}
        {--parts}
        {--part-categories}
        {--sql}';

    protected $description = 'Normalize assembly and material names';

    public function handle(): int
    {
        $this->all = $this->option('all');
        $this->assemblies = $this->option('assemblies') || $this->all;
        $this->assembly_categories = $this->option('assembly-categories') || $this->all;
        $this->parts = $this->option('parts') || $this->all;
        $this->part_categories = $this->option('part-categories') || $this->all;
        $this->sql = $this->option('sql');

        // region

        Assembly::disableSearchSyncing();

        if ($this->assemblies) {
            $this->line('-- assemblies');

            Assembly::default()->orderBy('name')->chunk(100, function ($assemblies) {
                foreach ($assemblies as $assembly) {
                    $id = $assembly->id;

                    $before = $assembly->name;
                    $after = $this->normalize($before);

                    if ($before != $after && $this->sql) {
                        $escaped = Str::replace("'", "''", $after);
                        $this->line("UPDATE assemblies SET name = '$escaped' WHERE id = $id;");
                    }
                }
            });
        }

        if ($this->assembly_categories) {
            $this->line('-- assembly categories');

            AssemblyCategory::default()->orderBy('name')->chunk(100, function ($categories) {
                foreach ($categories as $category) {
                    $id = $category->id;

                    $before = $category->name;
                    $after = $this->normalize($before);

                    if ($before != $after) {
                        $escaped = Str::replace("'", "''", $after);
                        $this->line("UPDATE assembly_categories SET name = '$escaped' WHERE id = $id;");
                    }
                }
            });
        }

        // endregion

        // region

        Part::disableSearchSyncing();

        if ($this->parts) {
            $this->line('-- parts');

            Part::default()->orderBy('name')->chunk(100, function ($parts) {
                foreach ($parts as $part) {
                    $id = $part->id;

                    $before = $part->name;
                    $after = $this->normalize($before);

                    if ($before != $after && $this->sql) {
                        $escaped = Str::replace("'", "''", $after);
                        $this->line("UPDATE parts SET name = '$escaped' WHERE id = $id;");
                    }
                }
            });
        }

        if ($this->part_categories) {
            $this->line('-- part categories');

            PartCategory::default()->orderBy('name')->chunk(100, function ($categories) {
                foreach ($categories as $category) {
                    $id = $category->id;

                    $before = $category->name;
                    $after = $this->normalize($before);

                    if ($before != $after) {
                        $escaped = Str::replace("'", "''", $after);
                        $this->line("UPDATE part_categories SET name = '$escaped' WHERE id = $id;");
                    }
                }
            });
        }

        // endregion

        return 0;
    }

    private function normalize($input)
    {
        $output = $input;

        // region  / done

        $output = Str::squish($output);

        // endregion

        // region / done

        $output = Str::of($output)->replaceMatches('/([0-9]+) \#([0-9\/]+)(s?)/', function ($match) {
            $count = intval($match[1]);
            $number = $match[2];

            $output = "$count # $number";

            return $output;
        });

        // endregion

        // region / done

        foreach (['ARC', 'EMT', 'GRC', 'PVC'] as $conduit) {
            if (Str::of($output)->contains('SCHEDULE 40')) {
                continue;
            }

            if (Str::of($output)->contains('SCHEDULE 80')) {
                continue;
            }

            $output = Str::of($output)->replaceMatches("/(([0-9])(\/?)([0-9]?)) ($conduit)/", function ($match) {
                $length = $match[1];
                $conduit = $match[5];

                $output = "$length\" $conduit";

                return $output;
            });
        }

        // endregion

        // region / done

        $output = Str::of($output)->replace('.25"', '-1/4"');
        $output = Str::of($output)->replace('.5"', '-1/2"');
        $output = Str::of($output)->replace('.75"', '-3/4"');

        // endregion

        if ($input != $output && ! $this->sql) {
            $this->line("<fg=gray>$input</> -> <fg=green>$output</>");
        }

        return $output;
    }
}
