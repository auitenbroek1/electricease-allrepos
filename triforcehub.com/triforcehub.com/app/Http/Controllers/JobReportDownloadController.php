<?php

namespace App\Http\Controllers;

use App\Actions\Jobs\BuildJobReports;
use App\Exports\JobReportExport;
use App\Models\Job;
use Illuminate\Http\Request;
use Maatwebsite\Excel\Facades\Excel;

class JobReportDownloadController extends Controller
{
    public function __invoke(Job $job, Request $request)
    {
        $reports = (new BuildJobReports)->execute($job);

        //

        $rows = [];

        foreach ($reports as $report) {
            $rows[] = [$report['name']];
            foreach ($report['items'] as $item) {
                $rows[] = [$item['label'], $item['value']];
            }
            $rows[] = [' '];
        }

        //

        return Excel::download(
            new JobReportExport($rows),
            'report.xlsx',
            null,
            ['X-Vapor-Base64-Encode' => 'True']
        );
    }
}
