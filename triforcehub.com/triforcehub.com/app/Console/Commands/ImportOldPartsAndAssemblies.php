<?php

namespace App\Console\Commands;

use App\Models\Assembly;
use App\Models\AssemblyCategory;
use App\Models\AssemblyTag;
use App\Models\Part;
use App\Models\PartCategory;
use App\Models\PartTag;
use Illuminate\Console\Command;
use Illuminate\Support\Facades\DB;

class ImportOldPartsAndAssemblies extends Command
{
    protected $signature = 'triforce:import-old-parts-and-assemblies';

    protected $description = 'Import old parts and assemblies';

    public function handle(): int
    {
        Part::disableSearchSyncing();

        $this->importPartCategories();
        $this->importPartTags();
        $this->importParts();

        $this->importAssemblyCategories();
        $this->importAssemblyTags();
        $this->importAssemblies();
        $this->importAssemblyParts();

        return 0;
    }

    private function importPartCategories()
    {
        $items = [];

        //

        $handle = fopen(database_path('Distributor_Parts_Details.csv'), 'r');

        if ($handle) {
            while (($columns = fgetcsv($handle, 1000, ',')) !== false) {
                try {
                    $category = $columns[2];

                    $items[$category] = [
                        'name' => trim($category),
                    ];
                } catch (\TypeError $e) {
                }
            }
            fclose($handle);
        } else {
            // error opening the file.
        }

        //

        foreach ($items as $item) {
            PartCategory::factory()->create($item);
        }
    }

    private function importPartTags()
    {
        PartTag::factory()
            ->count(10)
            ->create();
    }

    public function importParts()
    {
        DB::statement('SET FOREIGN_KEY_CHECKS=0;');
        DB::table('part_part_category')->truncate();
        DB::table('parts')->truncate();
        DB::statement('SET FOREIGN_KEY_CHECKS=1;');

        $items = [];

        //

        $handle = fopen(database_path('Distributor_Parts_Details.csv'), 'r');

        if ($handle) {
            while (($columns = fgetcsv($handle, 1000, ',')) !== false) {
                try {
                    $number = $columns[1];
                    $category = $columns[2];
                    $description = $columns[3];
                    $cost = $columns[5];
                    $uom = $columns[13];
                    $labor = $columns[16];

                    $part = Part::factory()->create([
                        'name' => trim($description),
                        'unit_id' => 1,
                        'cost' => $cost,
                        'labor' => $labor,

                        '_Part_Number' => $number,
                        '_Part_Category' => $category,
                        '_Description' => $description,
                        '_Cost' => $cost,
                        '_UOM' => $uom,
                        '_LaborUnit' => $labor,
                    ]);

                    $categories = PartCategory::where([
                        'name' => $category,
                    ])->get();

                    $part
                        ->categories()
                        ->attach($categories);
                } catch (\TypeError $e) {
                }
            }
            fclose($handle);
        } else {
            // error opening the file.
        }

        //

        foreach ($items as $item) {
            PartCategory::factory()->create($item);
        }
    }

    public function importAssemblyCategories()
    {
        $items = [];

        //

        $handle = fopen(database_path('Distributor_Assemblies_Master.csv'), 'r');

        if ($handle) {
            while (($columns = fgetcsv($handle, 1000, ',')) !== false) {
                try {
                    $category = $columns[2];

                    $items[$category] = [
                        'name' => trim($category),
                    ];
                } catch (\TypeError $e) {
                }
            }
            fclose($handle);
        } else {
            // error opening the file.
        }

        //

        foreach ($items as $item) {
            AssemblyCategory::factory()->create($item);
        }
    }

    public function importAssemblyTags()
    {
        AssemblyTag::factory()
            ->count(10)
            ->create();
    }

    public function importAssemblies()
    {
        DB::statement('SET FOREIGN_KEY_CHECKS=0;');
        DB::table('assembly_assembly_category')->truncate();
        DB::table('assembly_part')->truncate();
        DB::table('assemblies')->truncate();
        DB::statement('SET FOREIGN_KEY_CHECKS=1;');

        $handle = fopen(database_path('Distributor_Assemblies_Master.csv'), 'r');

        if ($handle) {
            while (($columns = fgetcsv($handle, 1000, ',')) !== false) {
                try {
                    $name = $columns[1];
                    $category = $columns[2];
                    $description = $columns[3];
                    $id = $columns[21];

                    $assembly = Assembly::factory()->create([
                        'name' => trim(str_replace('10ft', '', $description)),

                        '_AssemblyID' => $id,
                        '_Assemblies_Name' => $name,
                        '_Assemblies_Description' => $description,
                        '_Assemblies_Category' => $category,
                    ]);

                    $categories = AssemblyCategory::where([
                        'name' => $category,
                    ])->get();

                    $assembly
                        ->categories()
                        ->attach($categories);
                } catch (\TypeError $e) {
                }
            }
            fclose($handle);
        } else {
            // error opening the file.
        }
    }

    public function importAssemblyParts()
    {
        DB::statement('SET FOREIGN_KEY_CHECKS=0;');
        DB::table('assembly_part')->truncate();
        DB::statement('SET FOREIGN_KEY_CHECKS=1;');

        $handle = fopen(database_path('Distributor_Assemblies_Parts.csv'), 'r');

        if ($handle) {
            while (($columns = fgetcsv($handle, 1000, ',')) !== false) {
                try {
                    $assembly_name = $columns[1];
                    $part_name = $columns[4];
                    $quantity = $columns[9];

                    if (str_contains($assembly_name, '10ft')) {
                        $quantity = $quantity / 10;
                    }

                    $part = Part::where([
                        '_Part_Number' => $part_name,
                    ])
                        ->get()
                        ->keyBy('id')
                        ->map(function () use ($quantity) {
                            return ['quantity' => $quantity];
                        });

                    $assemblies = Assembly::where([
                        '_Assemblies_Name' => $assembly_name,
                    ])->get();

                    foreach ($assemblies as $assembly) {
                        $assembly->parts()
                            ->attach($part);
                    }
                } catch (\TypeError $e) {
                }
            }
            fclose($handle);
        } else {
            // error opening the file.
        }
    }
}
