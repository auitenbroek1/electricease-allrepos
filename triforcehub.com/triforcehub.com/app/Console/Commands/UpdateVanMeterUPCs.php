<?php

namespace App\Console\Commands;

use App\Models\Part;
use App\Models\PartUPC;
use Illuminate\Console\Command;

class UpdateVanMeterUPCs extends Command
{
    protected $signature = 'triforce:update-van-meter-upcs';

    protected $description = 'Update Van Meter UPCs';

    public function handle(): int
    {
        Part::disableSearchSyncing();

        // UPC
        // Part Number
        // Category
        // My Cost
        // Markup
        // Resale
        // Labor Unit
        // Purchased From
        // My Description
        // UOM

        $handle = fopen(database_path('PriceBook9800104-22.csv'), 'r');

        if ($handle) {
            while (($columns = fgetcsv($handle, 1000, ',')) !== false) {
                $_upc = trim($columns[0]);
                $_part_number = trim($columns[1]);

                //

                if (empty($_upc) || empty($_part_number)) {
                    continue;
                }

                if (strlen($_upc) === 10) {
                    $_upc = '0'.$_upc;
                }

                // if (strlen($_upc) != 12) {
                //     $this->info($_upc . ': ' . strlen($_upc));
                // }

                //

                $part = Part::where([
                    '_Part_Number' => $_part_number,
                ])->first();

                if ($part && $part->id) {
                    // $this->line($_part_number . ': part found');

                    $upc = PartUPC::firstOrCreate([
                        'name' => $_upc,
                    ]);

                    try {
                        $part->upcs()->sync($upc);
                        $part->save();
                    } catch (\Exception $e) {
                        $this->error('Van Meter Part Number: '.$_part_number);
                        $this->error('UPC: '.$_upc);
                        $this->error($e->getMessage());
                    }
                } else {
                    // $this->line($_part_number . ': skipping');
                }
            }
            fclose($handle);
        } else {
            // error opening the file.
        }

        return 0;
    }
}
