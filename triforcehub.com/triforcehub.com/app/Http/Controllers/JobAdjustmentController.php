<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobAdjustmentRequest;
use App\Http\Requests\UpdateJobAdjustmentRequest;
use App\Http\Resources\JobAdjustmentResource;
use App\Models\JobAdjustment;
use Illuminate\Support\Str;

class JobAdjustmentController extends Controller
{
    public function store(StoreJobAdjustmentRequest $request)
    {
        $adjustment = new JobAdjustment;
        $adjustment->job_id = $request->input('job_id');
        $adjustment->uuid = Str::uuid();
        $adjustment->slug = $request->input('slug');
        $adjustment->override = $request->input('override');
        $adjustment->overhead = $request->input('overhead');
        $adjustment->profit = $request->input('profit');
        $adjustment->tax = $request->input('tax');
        $adjustment->enabled = $request->input('enabled');
        $adjustment->save();

        return new JobAdjustmentResource($adjustment);
    }

    public function update(UpdateJobAdjustmentRequest $request, JobAdjustment $adjustment)
    {
        $adjustment->update($request->validated());

        return new JobAdjustmentResource($adjustment);
    }

    public function destroy(JobAdjustment $adjustment)
    {
        $adjustment->delete();
    }
}
