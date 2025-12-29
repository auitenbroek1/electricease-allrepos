<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobLocationRequest;
use App\Http\Requests\UpdateJobLocationRequest;
use App\Http\Resources\JobLocationResource;
use App\Models\JobLocation;
use Illuminate\Support\Str;

class JobLocationController extends Controller
{
    public function store(StoreJobLocationRequest $request)
    {
        $location = new JobLocation;
        $location->job_id = $request->input('job_id');
        $location->uuid = Str::uuid();
        $location->name = $request->input('name');
        $location->address1 = $request->input('address1');
        $location->address2 = $request->input('address2');
        $location->city = $request->input('city');
        $location->state_id = $request->input('state_id');
        $location->zip = $request->input('zip');
        $location->save();

        return new JobLocationResource($location);
    }

    public function update(UpdateJobLocationRequest $request, JobLocation $location)
    {
        $location->update($request->validated());

        return new JobLocationResource($location);
    }

    public function destroy(JobLocation $location)
    {
        $location->delete();
    }
}
