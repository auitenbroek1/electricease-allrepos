<?php

namespace App\Actions\Jobs;

use App\Helpers\Math;

class SummarizeLabors
{
    public function execute(
        $data,
        $labors,
    ) {
        if (! count($labors)) {
            return null;
        }

        //

        $items = collect($labors)->filter(function ($item) {
            return $item['enabled'];
        })->map(function ($item) {
            return [
                'id' => $item['id'],
                'uuid' => $item['uuid'],
                'name' => $item['name'],
                'hours' => $item['hours'],
                'rate' => $item['rate'],
                'cost' => $item['cost'],
                'burden' => $item['burden'],
                'burden_total' => $item['burden_total'],
                'fringe' => $item['fringe'],
                'fringe_total' => $item['fringe_total'],
                'rate_total' => $item['rate_total'],
                'cost_total' => $item['cost_total'],
            ];
        });

        if (! $items->count()) {
            return null;
        }

        //

        $summary = [
            'hours' => 0,
            'rate' => 0,
            'cost' => 0,
            'burden' => 0,
            'burden_total' => 0,
            'fringe' => 0,
            'fringe_total' => 0,
            'rate_total' => 0,
            'cost_total' => 0,
        ];

        $summary = $items->reduce(function ($carry, $item) {
            $hours = $item['hours'];
            $rate = $item['rate'];
            $cost = $item['cost'];
            $burden = $item['burden'];
            $burden_total = $item['burden_total'];
            $fringe = $item['fringe'];
            $fringe_total = $item['fringe_total'];
            $rate_total = $item['rate_total'];
            $cost_total = $item['cost_total'];

            return [
                'hours' => Math::add($carry['hours'], $hours),
                'rate' => Math::add($carry['rate'], $rate),
                'cost' => Math::add($carry['cost'], $cost),
                'burden' => Math::add($carry['burden'], $burden),
                'burden_total' => Math::add($carry['burden_total'], $burden_total),
                'fringe' => Math::add($carry['fringe'], $fringe),
                'fringe_total' => Math::add($carry['fringe_total'], $fringe_total),
                'rate_total' => Math::add($carry['rate_total'], $rate_total),
                'cost_total' => Math::add($carry['cost_total'], $cost_total),
            ];
        }, $summary);

        $summary['hours'] = Math::round($summary['hours']);
        $summary['rate'] = Math::round(Math::divide($summary['rate'], $items->count()));
        $summary['cost'] = Math::round($summary['cost']);
        $summary['burden'] = Math::round(Math::divide($summary['burden'], $items->count()));
        $summary['burden_total'] = Math::round($summary['burden_total']);
        $summary['fringe'] = Math::round(Math::divide($summary['fringe'], $items->count()));
        $summary['fringe_total'] = Math::round($summary['fringe_total']);
        $summary['rate_total'] = Math::round(Math::divide($summary['rate_total'], $items->count()));
        $summary['cost_total'] = Math::round($summary['cost_total']);

        //

        return [
            'items' => $items->all(),
            'summary' => $summary,
        ];
    }
}
