<?php

namespace App\Http\Controllers;

use App\Actions\Jobs\BuildJobReports;
use App\Models\Job;
use Illuminate\Http\Request;

class JobReports extends Controller
{
    public function __invoke(Job $job, Request $request)
    {
        return (new BuildJobReports)->execute($job);
    }
}
