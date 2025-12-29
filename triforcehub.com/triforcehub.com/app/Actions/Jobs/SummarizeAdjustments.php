<?php

namespace App\Actions\Jobs;

use App\Helpers\Math;

class SummarizeAdjustments
{
    public function execute(
        $data,
        $adjustments,
        $settings,
    ) {
        $items = [
            [
                'name' => 'Material',
                'slug' => 'materials',
            ],
            [
                'name' => 'Labor',
                'slug' => 'crews',
            ],
            [
                'name' => 'Additional Labor',
                'slug' => 'labors',
            ],
            [
                'name' => 'Direct Expenses',
                'slug' => 'expenses',
            ],
            [
                'name' => 'Vendor Quotes',
                'slug' => 'quotes',
            ],
        ];

        $db = collect($adjustments)->keyBy('slug')->all();

        foreach ($items as &$item) {
            $slug = $item['slug'];

            $id = isset($db[$slug]) ? $db[$slug]['id'] : null;
            $uuid = isset($db[$slug]) ? $db[$slug]['uuid'] : null;
            $cost = isset($data[$slug]) ? $data[$slug]['summary']['cost_total'] : 0;
            $override = isset($db[$slug]) ? $db[$slug]['override'] : '';
            $overhead = isset($db[$slug]) ? $db[$slug]['overhead'] : '';
            $profit = isset($db[$slug]) ? $db[$slug]['profit'] : '';
            $tax = isset($db[$slug]) ? $db[$slug]['tax'] : '';
            $cost_total = null;

            //

            $base = $override ?: $cost;
            $overhead_total = $overhead ? Math::round(Math::multiply($base, Math::divide($overhead, 100))) : 0;
            $base_with_overhead = $base + $overhead_total;
            $profit_total = $profit ? Math::round(Math::multiply($base_with_overhead, Math::divide($profit, 100))) : 0;
            $tax_total = $tax ? Math::round(Math::multiply(Math::add($base_with_overhead, $profit_total), Math::divide($tax, 100))) : 0;
            $cost_total = Math::round(Math::add(Math::add($base_with_overhead, $profit_total), $tax_total));

            if ($slug == 'materials') {
                if ($settings['exclude_material_subtotal_from_total']) {
                    $cost_total = 0;
                    $cost_total = Math::round(Math::add($cost_total, $overhead_total));
                    $cost_total = Math::round(Math::add($cost_total, $profit_total));
                    $cost_total = Math::round(Math::add($cost_total, $tax_total));
                }
            }

            //

            $item['id'] = $id;
            $item['uuid'] = $uuid;
            $item['cost'] = $cost;
            $item['override'] = $override;
            $item['base'] = $base;
            $item['overhead'] = $overhead;
            $item['overhead_total'] = $overhead_total;
            $item['profit'] = $profit;
            $item['profit_total'] = $profit_total;
            $item['tax'] = $tax;
            $item['tax_total'] = $tax_total;
            $item['cost_total'] = $cost_total;
        }

        //

        $items = collect($items)->map(function ($item) {
            return [
                'id' => $item['id'],
                'uuid' => $item['uuid'],
                'name' => $item['name'],
                'slug' => $item['slug'],
                'cost' => $item['cost'],
                'override' => $item['override'],
                'base' => $item['base'],
                'overhead' => $item['overhead'],
                'overhead_total' => $item['overhead_total'],
                'profit' => $item['profit'],
                'profit_total' => $item['profit_total'],
                'tax' => $item['tax'],
                'tax_total' => $item['tax_total'],
                'cost_total' => $item['cost_total'],
            ];
        });

        //

        $summary = [
            'cost' => 0,
            'override' => 0,
            'overhead' => 0,
            'overhead_total' => 0,
            'profit' => 0,
            'profit_total' => 0,
            'tax' => 0,
            'tax_total' => 0,
            'cost_total' => 0,
        ];

        $summary = $items->reduce(function ($carry, $item) {
            $cost = $item['cost'];
            $override = $item['override'] ?: $item['cost'];
            $overhead = $item['overhead'] ?? '';
            $overhead_total = $item['overhead_total'] ?? '';
            $profit = $item['profit'] ?? '';
            $profit_total = $item['profit_total'] ?? '';
            $tax = $item['tax'] ?? '';
            $tax_total = $item['tax_total'] ?? '';
            $cost_total = $item['cost_total'] ?? '';

            return [
                'cost' => Math::add($carry['cost'], $cost),
                'override' => Math::add($carry['override'], $override),
                'overhead' => Math::add($carry['overhead'], $overhead),
                'overhead_total' => Math::add($carry['overhead_total'], $overhead_total),
                'profit' => Math::add($carry['profit'], $profit),
                'profit_total' => Math::add($carry['profit_total'], $profit_total),
                'tax' => Math::add($carry['tax'], $tax),
                'tax_total' => Math::add($carry['tax_total'], $tax_total),
                'cost_total' => Math::add($carry['cost_total'], $cost_total),
            ];
        }, $summary);

        $summary['cost'] = Math::round($summary['cost']);
        $summary['override'] = Math::round($summary['override']);
        $summary['overhead'] = Math::round(Math::divide($summary['overhead'], $items->count()));
        $summary['overhead_total'] = Math::round($summary['overhead_total']);
        $summary['profit'] = Math::round(Math::divide($summary['profit'], $items->count()));
        $summary['profit_total'] = Math::round($summary['profit_total']);
        $summary['tax'] = Math::round(Math::divide($summary['tax'], $items->count()));
        $summary['tax_total'] = Math::round($summary['tax_total']);
        $summary['cost_total'] = Math::round($summary['cost_total']);

        // TODO: review
        if ($summary['cost_total']) {
            $summary['profit'] = Math::round(Math::divide($summary['profit_total'] * 100, $summary['cost_total']));
        }

        //

        return [
            'items' => $items->all(),
            'summary' => $summary,
        ];
    }
}
