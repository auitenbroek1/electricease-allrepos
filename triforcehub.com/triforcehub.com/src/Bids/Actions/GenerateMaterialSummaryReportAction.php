<?php

namespace Src\Bids\Actions;

use App\Actions\Jobs\BuildJobResponse;
use App\Models\Job;

class GenerateMaterialSummaryReportAction
{
    public function __invoke(
        Job $job
    ): array {
        $resource = (new BuildJobResponse)->execute($job);

        $collection = collect([]);

        foreach ($resource['phases'] as $phase) {
            foreach ($phase['assemblies'] as $assembly) {
                if (! $assembly['enabled']) {
                    continue;
                }

                foreach ($assembly['parts'] as $part) {
                    $item = $this->normalize($part, $assembly['quantity']);
                    $collection = $this->append($collection, $item);
                }
            }

            foreach ($phase['parts'] as $part) {
                if (! $part['enabled']) {
                    continue;
                }

                $item = $this->normalize($part, 1);
                $collection = $this->append($collection, $item);
            }
        }

        $collection = $collection->sortBy('name')->map(function ($item) {
            $item['cost'] = round($item['cost'], 2);
            $item['quantity'] = round($item['quantity'], 2);
            $item['cost_total'] = round($item['cost_total'], 2);

            return $item;
        });

        return $collection->toArray();
    }

    private function normalize($part, $multiplier)
    {
        $item = [];

        $item['uuid'] = $part['reference']['uuid'];
        $item['name'] = $part['reference']['name'];
        $item['description'] = $part['reference']['description'];

        $item['cost'] = bcmul($part['cost'], 1, 6);
        $item['quantity'] = bcmul($part['quantity'], $multiplier, 6);
        $item['cost_total'] = bcmul($part['cost'], $item['quantity'], 6);

        // for the material summary tab only

        $item['key'] = $part['reference']['id'];
        $item['upcs'] = collect($part['reference']['upcs'])->map(function ($upc) {
            return $upc['name'];
        })->toArray();

        //

        return $item;
    }

    private function append($collection, $item)
    {
        if ($collection->contains('uuid', $item['uuid'])) {
            $collection = $collection->map(function ($current) use ($item) {
                if ($current['uuid'] !== $item['uuid']) {
                    return $current;
                }

                try {
                    $current['cost_total'] = bcadd($current['cost_total'], $item['cost_total'], 6);
                    $current['quantity'] = bcadd($current['quantity'], $item['quantity'], 6);
                    $current['cost'] = bcdiv($current['cost_total'], $current['quantity'], 6);
                } catch (\Throwable $e) {
                    // ray($current);
                }

                return $current;
            });
        } else {
            $collection->push($item);
        }

        return $collection;
    }
}
