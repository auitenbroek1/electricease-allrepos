<?php

namespace App\Console\Commands;

use App\Models\Part;
use Illuminate\Console\Command;
use Illuminate\Support\Facades\DB;

// artisan triforce:check-assembly-parts

class CheckAssemblyParts extends Command
{
    protected $signature = 'triforce:check-assembly-parts';

    protected $description = 'Check assembly parts';

    public function handle(): int
    {
        $part_ids = collect(DB::select('SELECT DISTINCT part_id FROM assembly_part'))->map(function ($item) {
            return $item->part_id;
        })->toArray();

        $parts = Part::with(['upcs'])->whereIn('id', $part_ids)->orderBy('name');
        // $this->line($parts->count());

        $parts_with_missing_upcs = [];
        $parts->each(function ($part) use (&$parts_with_missing_upcs) {
            $upcs = $part->upcs->map(function ($upc) {
                return $upc->name;
            })->toArray();

            if (count($upcs)) {
                // $this->line($part->name.' (id: '.$part->id.', upcs: ' . implode(', ', $upcs). ')');
            } else {
                $parts_with_missing_upcs[] = $part;
                // $this->error($part->name.' (id: '.$part->id.')');

                $alternatives = Part::with(['upcs'])->where('name', $part->name);
                if ($alternatives->count() > 1) {
                    $this->error($part->name.' (id: '.$part->id.')');
                    $alternatives->each(function ($alternative) {
                        // $this->error($alternative->name. ': ' . $alternative->upcs->count());
                        $upcs2 = $alternative->upcs->map(function ($upc) {
                            return $upc->name;
                        })->toArray();
                        // $this->line($upcs2);
                    });
                } else {
                    // $this->line($part->name);
                }
            }
        });
        // ray(count($parts_with_missing_upcs));

        return 0;
    }
}
