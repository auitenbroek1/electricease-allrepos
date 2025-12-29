<?php

namespace Src\Bids\Controllers;

use App\Http\Controllers\Controller;
use App\Models\Job;
use Src\Bids\Actions\GenerateMaterialSummaryReportAction;
use Src\Bids\Exports\MaterialSummaryReportExport;

class DownloadMaterialSummaryReportController extends Controller
{
    public function __invoke(
        Job $job,
        string $extension,
    ) {
        if ($extension === 'xlsx') {
            return new MaterialSummaryReportExport(
                (new GenerateMaterialSummaryReportAction)($job),
            );
        }

        abort(404);
    }
}
