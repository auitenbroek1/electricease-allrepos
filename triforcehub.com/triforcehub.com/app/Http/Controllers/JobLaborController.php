<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobLaborRequest;
use App\Http\Requests\UpdateJobLaborRequest;
use App\Http\Resources\JobLaborResource;
use App\Models\JobLabor;
use Illuminate\Support\Str;

class JobLaborController extends Controller
{
    public function store(StoreJobLaborRequest $request)
    {
        $labor = new JobLabor;
        $labor->job_id = $request->input('job_id');
        $labor->uuid = Str::uuid();
        $labor->name = $request->input('name') ?? '';
        $labor->hours = $request->input('hours');
        $labor->rate = $request->input('rate');
        $labor->notes = $request->input('notes');
        $labor->enabled = $request->input('enabled');
        $labor->save();

        return new JobLaborResource($labor);
    }

    public function update(UpdateJobLaborRequest $request, JobLabor $labor)
    {
        $labor->update($request->validated());

        return new JobLaborResource($labor);
    }

    public function destroy(JobLabor $labor)
    {
        $labor->delete();
    }
}
