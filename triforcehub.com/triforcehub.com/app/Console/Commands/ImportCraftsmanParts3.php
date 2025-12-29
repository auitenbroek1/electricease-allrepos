<?php

namespace App\Console\Commands;

use App\Models\Part;
use App\Models\PartCategory;
use Illuminate\Console\Command;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Str;

class ImportCraftsmanParts3 extends Command
{
    protected $signature = 'triforce:import-craftsman-parts-3';

    protected $description = 'Update Craftsman Parts 3';

    public function handle(): int
    {
        Part::disableSearchSyncing();

        // DB::statement('SET FOREIGN_KEY_CHECKS=0;');
        // DB::table('part_part_category')->truncate();
        // DB::table('part_categories')->truncate();
        // DB::table('parts')->truncate();
        // DB::statement('SET FOREIGN_KEY_CHECKS=1;');

        //

        $_uom_units = [
            '50' => 50,
            '100' => 100,
            '500' => 500,
            '1000' => 1000,
            'CLF' => 100,
            'MLF' => 1000,
            'KLF' => 1000,
            'LF' => 1,
            'Ea' => 1,
            'Pr' => 2,
        ];

        //

        $handle = fopen(database_path('Access and Controls.csv'), 'r');

        if ($handle) {
            while (($columns = fgetcsv($handle, 1000, ',')) !== false) {
                $_description = trim($columns[1]);
                $_cost = trim($columns[2]);
                $_labor = trim($columns[5]);
                $_uom = trim($columns[7]);

                //

                $name = $_description;

                //

                if ($_cost === '') {
                    continue;
                }

                $cost = 0;
                $labor = 0;
                if (isset($_uom_units[$_uom])) {
                    try {
                        $cost = floatval($_cost) / $_uom_units[$_uom];
                        $labor = floatval($_labor) / $_uom_units[$_uom];
                    } catch (\TypeError $e) {
                        dump($columns);
                        dump($_cost);
                        dump($_labor);
                        dd($e);
                    }
                } else {
                    echo $name."\n";
                    echo $_cost."\n";
                    echo $_labor."\n";
                    echo $_uom."\n";
                }

                //

                $part = Part::firstOrcreate([
                    'uuid' => Str::uuid(),
                    'name' => $name,
                    'unit_id' => 1,
                    'cost' => $cost,
                    'labor' => $labor,
                ]);

                // ray($part);

                // continue;

                $category = PartCategory::firstOrCreate([
                    'name' => 'Security',
                ]);

                $part
                    ->categories()
                    ->attach($category);
            }
            fclose($handle);
        } else {
            // error opening the file.
        }

        return 0;
    }
}
