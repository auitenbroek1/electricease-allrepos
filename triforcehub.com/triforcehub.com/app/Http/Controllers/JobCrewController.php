<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobCrewRequest;
use App\Http\Requests\UpdateJobCrewRequest;
use App\Http\Resources\JobCrewResource;
use App\Models\JobCrew;
use Illuminate\Support\Str;

class JobCrewController extends Controller
{
    public function store(StoreJobCrewRequest $request)
    {
        $crew = new JobCrew;
        $crew->job_id = $request->input('job_id');
        $crew->uuid = Str::uuid();
        $crew->name = $request->input('name') ?? '';
        $crew->quantity = $request->input('quantity') ?? 1;
        $crew->rate = $request->input('rate');
        $crew->burden = $request->input('burden');
        $crew->fringe = $request->input('fringe');
        $crew->enabled = 1;
        $crew->save();

        return new JobCrewResource($crew);
    }

    public function update(UpdateJobCrewRequest $request, JobCrew $crew)
    {
        $crew->update($request->validated());

        return new JobCrewResource($crew);
    }

    public function destroy(JobCrew $crew)
    {
        $crew->delete();
    }
}
