<?php

namespace App\Actions\Jobs;

use App\Helpers\Math;

class SummarizeMaterials
{
    public function execute(
        $data,
        $phases,
    ) {
        if (! count($phases)) {
            return null;
        }

        // calculate percentages

        $tmp_cost_total = 0;
        $tmp_labor_total = 0;
        foreach ($phases as $phase) {
            $tmp_cost_total = Math::add($tmp_cost_total, $phase['cost']);
            $tmp_labor_total = Math::add($tmp_labor_total, $phase['labor']);
        }

        foreach ($phases as &$phase) {
            $tmp_cost_percent = $tmp_cost_total > 0
                ? round(bcdiv(bcmul($phase['cost'], 100, 2), $tmp_cost_total, 6), 2)
                : 0;

            $tmp_labor_percent = $tmp_labor_total > 0
                ? round(bcdiv(bcmul($phase['labor'], 100, 2), $tmp_labor_total, 6), 2)
                : 0;

            $phase['cost_percent'] = Math::round($tmp_cost_percent);
            $phase['labor_percent'] = Math::round($tmp_labor_percent);
        }

        //

        $items = collect($phases)->map(function ($item) {
            return [
                'id' => $item['id'],
                'uuid' => $item['uuid'],
                'name' => $item['name'],
                'cost' => $item['cost'],
                'cost_percent' => $item['cost_percent'],
                'cost_total' => $item['cost_total'],
                'labor' => $item['labor'],
                'labor_percent' => $item['labor_percent'],
                'labor_total' => $item['labor_total'],
            ];
        });

        if (! $items->count()) {
            return null;
        }

        //

        $summary = [
            'cost' => 0,
            'cost_percent' => 0,
            'cost_total' => 0,
            'labor' => 0,
            'labor_percent' => 0,
            'labor_total' => 0,
        ];

        $summary = $items->reduce(function ($carry, $item) {
            $cost = $item['cost'];
            $cost_percent = $item['cost_percent'];
            $cost_total = $item['cost_total'];
            $labor = $item['labor'];
            $labor_percent = $item['labor_percent'];
            $labor_total = $item['labor_total'];

            return [
                'cost' => Math::add($carry['cost'], $cost),
                'cost_percent' => Math::add($carry['cost_percent'], $cost_percent),
                'cost_total' => Math::add($carry['cost_total'], $cost_total),
                'labor' => Math::add($carry['labor'], $labor),
                'labor_percent' => Math::add($carry['labor_percent'], $labor_percent),
                'labor_total' => Math::add($carry['labor_total'], $labor_total),
            ];
        }, $summary);

        $summary['cost'] = Math::round($summary['cost']);
        $summary['cost_percent'] = 100; // Math::round($summary['cost_percent']);
        $summary['cost_total'] = Math::round($summary['cost_total']);
        $summary['labor'] = Math::round($summary['labor']);
        $summary['labor_percent'] = 100; // Math::round($summary['labor_percent']);
        $summary['labor_total'] = Math::round($summary['labor_total']);

        //

        return [
            'items' => $items->all(),
            'summary' => $summary,
        ];
    }
}
