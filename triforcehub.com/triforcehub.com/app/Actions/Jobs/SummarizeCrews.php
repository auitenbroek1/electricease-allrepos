<?php

namespace App\Actions\Jobs;

use App\Helpers\Math;

class SummarizeCrews
{
    public function execute(
        $data,
        $crews,
    ) {
        if (! count($crews)) {
            return null;
        }

        // TODO: revist
        if (! $data['materials']) {
            return null;
        }

        // calculate hours

        $tmp_quantity_total = 0;
        foreach ($crews as $crew) {
            $tmp_quantity_total = Math::add($tmp_quantity_total, $crew['quantity']);
        }

        if ($tmp_quantity_total == 0) {
            return null;
        }

        $tmp_hours_total = $data['materials']['summary']['labor'];

        foreach ($crews as &$crew) {
            $tmp_quantity = Math::divide($crew['quantity'], $tmp_quantity_total);
            $crew['hours'] = Math::round(Math::multiply($tmp_hours_total, $tmp_quantity));

            $crew['cost'] = Math::round(Math::multiply($crew['hours'], $crew['rate']));
            $crew['burden_total'] = Math::round(Math::multiply($crew['cost'], Math::divide($crew['burden'], 100)));
            $crew['fringe_total'] = Math::round(Math::multiply($crew['hours'], $crew['fringe']));
            $crew['cost_total'] = Math::round(Math::add(Math::add($crew['cost'], $crew['burden_total']), $crew['fringe_total']));
            $crew['rate_total'] = $crew['hours'] > 0 ? Math::round(Math::divide($crew['cost_total'], $crew['hours'])) : 0;
        }

        //

        $items = collect($crews)->filter(function ($item) {
            return $item['enabled'];
        })->map(function ($item) {
            return [
                'id' => $item['id'],
                'uuid' => $item['uuid'],
                'name' => $item['name'],
                'quantity' => $item['quantity'],
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
            'quantity' => 0,
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
            $quantity = $item['quantity'];
            $hours = $item['hours'];
            $rate = Math::multiply($item['rate'], $item['quantity']);
            $cost = $item['cost'];
            $burden = Math::multiply($item['burden'], $item['quantity']);
            $burden_total = $item['burden_total'];
            $fringe = Math::multiply($item['fringe'], $item['quantity']);
            $fringe_total = $item['fringe_total'];
            $rate_total = Math::multiply($item['rate_total'], $item['quantity']);
            $cost_total = $item['cost_total'];

            return [
                'quantity' => Math::add($carry['quantity'], $quantity),
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

        $summary['quantity'] = Math::round($summary['quantity']);
        $summary['hours'] = Math::round($summary['hours']);
        $summary['rate'] = Math::round(Math::divide($summary['rate'], $summary['quantity']));
        $summary['cost'] = Math::round($summary['cost']);
        $summary['burden'] = Math::round(Math::divide($summary['burden'], $summary['quantity']));
        $summary['burden_total'] = Math::round($summary['burden_total']);
        $summary['fringe'] = Math::round(Math::divide($summary['fringe'], $summary['quantity']));
        $summary['fringe_total'] = Math::round($summary['fringe_total']);
        $summary['rate_total'] = Math::round(Math::divide($summary['rate_total'], $summary['quantity']));
        $summary['cost_total'] = Math::round($summary['cost_total']);

        //

        return [
            'items' => $items->all(),
            'summary' => $summary,
        ];
    }
}
