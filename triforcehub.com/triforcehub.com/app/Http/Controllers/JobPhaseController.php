<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobPhaseRequest;
use App\Http\Requests\UpdateJobPhaseRequest;
use App\Http\Resources\JobPhaseResource;
use App\Models\JobPhase;
use Illuminate\Http\Request;
use Illuminate\Support\Str;

class JobPhaseController extends Controller
{
    public function index()
    {
        //
    }

    public function store(StoreJobPhaseRequest $request)
    {
        $phase = new JobPhase;
        $phase->job_id = $request->input('job_id');
        $phase->uuid = Str::uuid();
        $phase->name = $request->input('name');
        $phase->area = $request->input('area');
        $phase->cost_code = $request->input('cost_code');
        $phase->save();

        return new JobPhaseResource($phase);
    }

    public function show(JobPhase $phase)
    {
        //
    }

    public function update(UpdateJobPhaseRequest $request, JobPhase $phase)
    {
        $phase->name = $request->input('name');
        $phase->area = $request->input('area');
        $phase->cost_code = $request->input('cost_code');
        $phase->save();

        return new JobPhaseResource($phase);
    }

    public function destroy(JobPhase $phase)
    {
        $phase->delete();
    }

    public function duplicate(JobPhase $phase, Request $request)
    {
        $this->authorize('duplicate', $phase);

        $_phase = $phase->replicate();
        $_phase->uuid = Str::uuid();
        $_phase->name = 'COPY OF '.$phase->name;
        $_phase->job_id = $phase->job_id;
        $_phase->save();

        foreach ($phase->assemblies as $assembly) {
            $_assembly = $assembly->replicate();
            $_assembly->uuid = Str::uuid();
            $_assembly->save();
            foreach ($assembly->parts as $part) {
                $_part = $part->replicate();
                $_part->uuid = Str::uuid();
                $_part->job_assembly_id = $_assembly->id;
                $_assembly->parts()->save($_part);
            }
            $_phase->assemblies()->save($_assembly);
        }

        foreach ($phase->parts as $part) {
            $_part = $part->replicate();
            $_part->uuid = Str::uuid();
            $_phase->parts()->save($_part);
        }

        return new JobPhaseResource($_phase);
    }
}
