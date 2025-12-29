<?php

namespace App\Console\Commands;

use App\Models\Part;
use App\Models\PartUPC;
use Illuminate\Console\Command;
use Illuminate\Support\Str;
use Throwable;

// artisan triforce:update-upc

class UpdateUPC extends Command
{
    protected $signature = 'triforce:update-upc';

    protected $description = 'Update UPCs';

    public function handle(): int
    {
        // ray()->showQueries();

        Part::disableSearchSyncing();

        // $this->parse('van-meter-zeros.csv');
        $this->parse('echo-zeros.csv');

        return 0;
    }

    private function parse($file)
    {
        $handle = fopen(database_path($file), 'r');

        if ($handle) {
            while (($columns = fgetcsv($handle, 1000, ',')) !== false) {
                // id,part,upc,cost,new upc

                $id = trim($columns[0]);
                $upc = trim($columns[4]);

                //

                $id = preg_replace('/[^0-9]/', '', $id);

                if (! Str::length($id)) {
                    continue;
                }

                //

                $upc = preg_replace('/[^0-9]/', '', $upc);

                if (! Str::length($upc)) {
                    continue;
                }

                //

                $part = Part::find($id);

                if ($part && $part->id) {
                    $upc = PartUPC::firstOrCreate([
                        'name' => $upc,
                    ]);

                    try {
                        // $this->line($id . ': ' . $upc->name);

                        $part->upcs()->sync($upc);
                        $part->save();
                    } catch (Throwable $e) {
                        $this->error($id.': '.$upc->name);

                        // $this->error($e->getMessage());
                    }
                } else {
                    $this->line($id.': skipping');
                }
            }
            fclose($handle);
        } else {
            // error opening the file.
        }
    }
}
