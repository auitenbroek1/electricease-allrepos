<?php

namespace App\Actions\Jobs;

use App\Helpers\Math;

class SummarizeExpenses
{
    public function execute(
        $data,
        $expenses,
    ) {
        if (! count($expenses)) {
            return null;
        }

        //

        $items = collect($expenses)->filter(function ($item) {
            return $item['enabled'];
        })->map(function ($item) {
            return [
                'id' => $item['id'],
                'uuid' => $item['uuid'],
                'name' => $item['name'],
                'notes' => $item['notes'],
                'cost' => $item['cost'],
                'cost_total' => $item['cost_total'],
            ];
        });

        if (! $items->count()) {
            return null;
        }

        //

        $summary = [
            'cost' => 0,
            'cost_total' => 0,
        ];

        $summary = $items->reduce(function ($carry, $item) {
            $cost = $item['cost'];
            $cost_total = $item['cost_total'];

            return [
                'cost' => Math::add($carry['cost'], $cost),
                'cost_total' => Math::add($carry['cost_total'], $cost_total),
            ];
        }, $summary);

        $summary['cost'] = Math::round($summary['cost']);
        $summary['cost_total'] = Math::round($summary['cost_total']);

        //

        return [
            'items' => $items->all(),
            'summary' => $summary,
        ];
    }
}
