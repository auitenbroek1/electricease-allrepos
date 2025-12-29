<?php

namespace Src\Bids\Exports;

use PhpOffice\PhpSpreadsheet\Style\NumberFormat;

class MaterialSummaryReportExport extends Export
{
    protected $fileName = 'material-summary-report.xlsx';

    public function array(): array
    {
        return collect($this->rows)
            ->map(function ($row) {
                return [$row['name'], $row['quantity']];
            })
            ->toArray();
    }

    public function columnFormats(): array
    {
        return [
            'B' => NumberFormat::FORMAT_NUMBER,
        ];
    }

    public function columnWidths(): array
    {
        return [
            'A' => 60,
            'B' => 15,
        ];
    }
}
