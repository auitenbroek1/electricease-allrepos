<?php

namespace App\Http\Controllers;

use App\Http\Requests\UpdateJobMaterialPriceRequest;
use App\Models\Job;
use Illuminate\Support\Facades\DB;

class JobMaterialPriceController extends Controller
{
    public function __invoke(UpdateJobMaterialPriceRequest $request, Job $job)
    {
        // ray($job->id, $request->all());

        $items = $request->all();

        // ray()->showQueries();

        foreach ($items as $item) {
            $key = $item['key'] ?? null;
            $price = $item['price'] ?? 0;

            if (! $key) {
                continue;
            }

            DB::table('job_parts')
                ->where('reference_id', $key)
                ->whereIn('job_phase_id', DB::table('job_phases')->select('id')->where('job_id', $job->id))
                ->update(['cost' => $price]);
        }

        return [];
    }
}
