<?php

namespace App\Console\Commands;

use App\Models\Part;
use App\Models\PartCategory;
use Illuminate\Console\Command;
use Illuminate\Support\Facades\DB;

class UpdateCraftsmanBookCompanyData extends Command
{
    protected $signature = 'triforce:import-craftsman-parts';

    protected $description = 'Update Craftsman Book Company data';

    public function handle(): int
    {
        Part::disableSearchSyncing();

        // DB::statement('SET FOREIGN_KEY_CHECKS=0;');
        // DB::table('part_part_category')->truncate();
        // DB::table('part_categories')->truncate();
        // DB::table('parts')->truncate();
        // DB::statement('SET FOREIGN_KEY_CHECKS=1;');

        //

        $parent_categories = [
            'CONDUIT & FITTINGS' => 'Conduit and Fittings',
            'WIRE AND CABLE' => 'Wire and Cable',
            'OUTLET BOXES' => 'Outlet Boxes',
            'LIGHTING FIXTURES' => 'Lighting Fixtures',
            'WIRING DEVICES' => 'Wiring Devices',
            'SERVICE ENTRANCE EQUIPMENT' => 'Service Entrance Equipment',
            'UNDERFLOOR RACEWAY' => 'Underfloor Raceway',
            'BUS DUCT' => 'Bus Duct',
            'CABLE TRAY' => 'Cable Tray',
            'SIGNAL SYSTEMS' => 'Signal Systems',
            'PRECAST ACCESS BOX' => 'Precast Concrete Access Boxes',
            'EQUIPMENT HOOKUP' => 'Equipment Hookup',
            'MOTOR CONTROL EQUIPMENT' => 'Motor Control Equipment',
            'TRENCHING AND EXCAVATION' => 'Trenching and Excavation',
            'SURFACE RACEWAYS' => 'Surface Raceways',
            'GROUNDING' => 'Grounding',
            'ASSEMBLIES' => 'Assemblies',
            'COMMUNICATIONS' => 'Communications',
        ];

        $current_parent_category = '';

        $_uom_units = [
            '50' => 50,
            '100' => 100,
            '500' => 500,
            '1000' => 1000,
            'CLF' => 100,
            'MLF' => 1000,
            'KLF' => 1000,
            'Ea' => 1,
            'Pr' => 2,
        ];

        //

        // $handle = fopen(database_path('nee2022.csv'), 'r');
        $handle = fopen(database_path('nee2022-with-ee-part-ids.csv'), 'r');

        if ($handle) {
            while (($columns = fgetcsv($handle, 1000, ',')) !== false) {
                $_category = trim($columns[0]);
                $_description = trim($columns[1]);
                $_cost = trim($columns[2]);
                $_labor = trim($columns[5]);
                $_uom = trim($columns[7]);

                $_part_id_1 = trim($columns[10]);
                $_part_id_2 = trim($columns[11]);
                $_part_id_3 = trim($columns[12]);

                //

                $name = trim($_category.' '.$_description);

                //

                if ($_cost === '') {
                    if (isset($parent_categories[$_description])) {
                        $current_parent_category = $parent_categories[$_description];
                    }

                    // print "<div>$current_parent_category - $_description</div>";
                    // skip because it's a heading
                    continue;
                }

                if ($current_parent_category === 'Assemblies') {
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
                    exit($_uom);
                }

                //

                $part_id = '';
                if (! empty($_part_id_1)) {
                    $part_id = $_part_id_1;
                }
                if (! empty($_part_id_2)) {
                    $part_id = $_part_id_2;
                }
                if (! empty($_part_id_3)) {
                    $part_id = $_part_id_3;
                }

                // ray($part_id);

                //

                if (empty($part_id)) {
                    $part = Part::factory()->create([
                        'name' => $name,
                        'unit_id' => 1,
                        'cost' => $cost,
                        'labor' => $labor,

                        '_Part_Number' => $part_id,
                        '_Part_Category' => null,
                        '_Description' => $name,
                        '_Cost' => null,
                        '_UOM' => null,
                        '_LaborUnit' => null,
                    ]);
                } else {
                    $part = Part::find($part_id);
                    $part->name = $name;
                    $part->cost = $cost;
                    $part->labor = $labor;
                    $part->save();
                }

                // continue;

                $parent_category = PartCategory::firstOrCreate([
                    'name' => $current_parent_category,
                ]);

                $part
                    ->categories()
                    ->attach($parent_category);
            }
            fclose($handle);
        } else {
            // error opening the file.
        }

        return 0;
    }
}
