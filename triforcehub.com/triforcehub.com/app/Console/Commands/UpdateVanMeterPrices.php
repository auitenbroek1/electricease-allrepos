<?php

namespace App\Console\Commands;

use App\Models\Part;
use Illuminate\Console\Command;

class UpdateVanMeterPrices extends Command
{
    protected $signature = 'triforce:update-van-meter-prices';

    protected $description = 'Update Van Meter prices';

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
                $_part_number = trim($columns[1]);
                $_cost = trim($columns[3]);

                //

                if (empty($_part_number)) {
                    continue;
                }

                //

                $cost = floatval($_cost);

                //

                $part = Part::where([
                    '_Part_Number' => $_part_number,
                ])->first();

                if ($part && $part->id) {
                    $part->cost = $cost;
                    $part->save();
                }
            }
            fclose($handle);
        } else {
            // error opening the file.
        }

        return 0;
    }
}
