<?php

namespace App\Actions\Jobs;

class BuildJobReports
{
    public function execute(
        $job,
    ) {
        $data = (new BuildJobResponse)->execute($job);

        $reports = [];
        $reports[] = $this->material($data);
        $reports[] = $this->labor($data);
        $reports[] = $this->expense($data);
        $reports[] = $this->quote($data);

        // ray($reports);

        return $reports;
    }

    private function material($data)
    {
        $adjustments = collect($data['summary']['adjustments']['items'])->first(function ($item) {
            return $item['slug'] == 'materials';
        });

        $base = $adjustments['base'];
        $overhead = $adjustments['overhead_total'];
        $profit = $adjustments['profit_total'];
        $total = $base + $overhead + $profit;

        //

        $items = [];

        $items[] = [
            'label' => 'Material Cost',
            'value' => $base,
        ];

        $items[] = [
            'label' => 'Total Overhead on Material',
            'value' => $overhead,
        ];

        $items[] = [
            'label' => 'Total Profit on Material',
            'value' => $profit,
        ];

        $items[] = [
            'label' => 'Material Grand Total',
            'value' => $total,
            'bold' => true,
        ];

        //

        $report = [
            'name' => 'Material',
            'items' => $items,
        ];

        return $report;
    }

    private function labor($data)
    {
        $crewAdjustments = collect($data['summary']['adjustments']['items'])->first(function ($item) {
            return $item['slug'] == 'crews';
        });

        $laborAdjustments = collect($data['summary']['adjustments']['items'])->first(function ($item) {
            return $item['slug'] == 'labors';
        });

        $labor1Base = $crewAdjustments['base'];
        $labor1Overhead = $crewAdjustments['overhead_total'];
        $labor1Profit = $crewAdjustments['profit_total'];
        $labor1Total = $labor1Base + $labor1Overhead + $labor1Profit;

        $labor2Base = $laborAdjustments['base'];
        $labor2Overhead = $laborAdjustments['overhead_total'];
        $labor2Profit = $laborAdjustments['profit_total'];
        $labor2Total = $labor2Base + $labor2Overhead + $labor2Profit;

        //

        $items = [];

        $items[] = [
            'label' => 'Job Labor Cost',
            'value' => $labor1Base,
        ];

        $items[] = [
            'label' => 'Total Overhead on Job Labor',
            'value' => $labor1Overhead,
        ];

        $items[] = [
            'label' => 'Total Profit on Job Labor',
            'value' => $labor1Profit,
        ];

        $items[] = [
            'label' => 'Job Labor Grand Total',
            'value' => $labor1Total,
        ];

        $items[] = [
            'label' => 'Additional Labor Cost',
            'value' => $labor2Base,
        ];

        $items[] = [
            'label' => 'Total Overhead on Additional Labor',
            'value' => $labor2Overhead,
        ];

        $items[] = [
            'label' => 'Total Profit on Additional Labor',
            'value' => $labor2Profit,
        ];

        $items[] = [
            'label' => 'Additional Labor Grand Total',
            'value' => $labor2Total,
        ];

        $items[] = [
            'label' => 'Total Labor Costs',
            'value' => $labor1Total + $labor2Total,
            'bold' => true,
        ];

        //

        $report = [
            'name' => 'Labor',
            'items' => $items,
        ];

        return $report;
    }

    private function expense($data)
    {
        $laborAdjustments = collect($data['summary']['adjustments']['items'])->first(function ($item) {
            return $item['slug'] == 'labors';
        });

        $expenseAdjustments = collect($data['summary']['adjustments']['items'])->first(function ($item) {
            return $item['slug'] == 'expenses';
        });

        $quoteAdjustments = collect($data['summary']['adjustments']['items'])->first(function ($item) {
            return $item['slug'] == 'quotes';
        });

        $expenseBase1 = $laborAdjustments['cost_total'];
        $expenseBase2 = $expenseAdjustments['cost_total'];
        $expenseBase3 = $quoteAdjustments['cost_total'];
        $expenseTotal = $expenseBase1 + $expenseBase2 + $expenseBase3;

        //

        $items = [];

        $items[] = [
            'label' => 'Additional Labor',
            'value' => $expenseBase1,
        ];

        $items[] = [
            'label' => 'Direct Job Expenses',
            'value' => $expenseBase2,
        ];

        $items[] = [
            'label' => 'Vendor Quotes',
            'value' => $expenseBase3,
        ];

        $items[] = [
            'label' => 'Expense Grand Total',
            'value' => $expenseTotal,
            'bold' => true,
        ];

        //

        $report = [
            'name' => 'Expense',
            'items' => $items,
        ];

        return $report;
    }

    private function quote($data)
    {
        $items = [];

        if (isset($data['summary']['quotes'])) {
            foreach ($data['summary']['quotes']['items'] as $item) {
                $items[] = [
                    'label' => $item['name'],
                    'value' => $item['cost_total'],
                ];
            }
        }

        $items[] = [
            'label' => 'Vendor Quote Grand Total',
            'value' => $data['summary']['quotes']['summary']['cost_total'] ?? 0,
            'bold' => true,
        ];

        //

        $report = [
            'name' => 'Quote',
            'items' => $items,
        ];

        return $report;
    }
}
