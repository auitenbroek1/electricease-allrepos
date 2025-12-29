<?php

namespace App\Actions\Jobs;

class BuildJobSummary
{
    public function execute(
        $phases,
        $crews,
        $labors,
        $expenses,
        $quotes,
        $adjustments,
        $settings,
    ) {
        if (! is_array($phases)) {
            return [];
        }
        if (! is_array($crews)) {
            return [];
        }
        if (! is_array($labors)) {
            return [];
        }
        if (! is_array($expenses)) {
            return [];
        }
        if (! is_array($quotes)) {
            return [];
        }

        $data = [];

        $data['materials'] = (new SummarizeMaterials)->execute(
            data: $data,
            phases: $phases,
        );

        $data['crews'] = (new SummarizeCrews)->execute(
            data: $data,
            crews: $crews,
        );

        $data['labors'] = (new SummarizeLabors)->execute(
            data: $data,
            labors: $labors,
        );

        $data['expenses'] = (new SummarizeExpenses)->execute(
            data: $data,
            expenses: $expenses,
        );

        $data['quotes'] = (new SummarizeQuotes)->execute(
            data: $data,
            quotes: $quotes,
        );

        $data['adjustments'] = (new SummarizeAdjustments)->execute(
            data: $data,
            adjustments: $adjustments,
            settings: $settings,
        );

        // ray($data);

        return $data;
    }
}
