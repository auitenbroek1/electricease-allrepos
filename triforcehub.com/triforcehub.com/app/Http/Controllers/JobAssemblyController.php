<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobAssemblyRequest;
use App\Http\Requests\UpdateJobAssemblyEnabledRequest;
use App\Http\Requests\UpdateJobAssemblyRequest;
use App\Http\Resources\JobAssemblyResource;
use App\Models\Assembly;
use App\Models\JobAssembly;
use App\Models\JobPart;
use Illuminate\Support\Str;

class JobAssemblyController extends Controller
{
    public function index()
    {
        //
    }

    public function store(StoreJobAssemblyRequest $request)
    {
        $job_phase_id = $request->input('job_phase_id');
        $assemblies = $request->input('assemblies');
        $labor_factor = $request->input('labor_factor') ?? 1;

        foreach ($assemblies as $assembly_id) {
            $job_assembly = new JobAssembly;
            $job_assembly->uuid = Str::uuid();
            $job_assembly->job_phase_id = $job_phase_id;
            $job_assembly->reference_id = $assembly_id;
            $job_assembly->quantity = 0;
            $job_assembly->labor_factor = $labor_factor;
            $job_assembly->save();

            $assembly = Assembly::findOrFail($assembly_id);

            foreach ($assembly->parts()->get()->toArray() as $assembly_part) {
                $job_part = new JobPart;
                $job_part->uuid = Str::uuid();
                $job_part->job_phase_id = $job_phase_id;
                $job_part->job_assembly_id = $job_assembly->id;
                $job_part->reference_id = $assembly_part['id'];
                $job_part->cost = $assembly_part['cost'];
                $job_part->labor = $assembly_part['labor'];
                $job_part->quantity = $assembly_part['pivot']['quantity'];
                $job_part->labor_factor = 1;
                $job_part->save();
            }
        }
    }

    public function show(JobAssembly $assembly)
    {
        //
    }

    public function update(UpdateJobAssemblyRequest $request, JobAssembly $assembly)
    {
        $assembly->quantity = $request->input('quantity') ?? 0;
        $assembly->labor_factor = $request->input('labor_factor') ?? 1;
        $assembly->enabled = $request->input('enabled') ?? 0;

        foreach ($assembly->parts as $part) {
            $part->enabled = $request->input('enabled') ?? 0;
            $part->save();
        }

        $assembly->save();

        return new JobAssemblyResource($assembly);
    }

    public function update_partial(UpdateJobAssemblyRequest $request, JobAssembly $assembly)
    {
        if ($request->exists('annotation_type')) {
            $assembly->annotation_type = $request->input('annotation_type');
        }

        if ($request->exists('annotation_symbol_id')) {
            $assembly->annotation_symbol_id = $request->input('annotation_symbol_id');
        }

        if ($request->exists('annotation_color')) {
            $assembly->annotation_color = $request->input('annotation_color');
        }

        if ($request->exists('annotation_length')) {
            $assembly->annotation_length = $request->input('annotation_length');
        }

        $assembly->save();

        return new JobAssemblyResource($assembly);
    }

    public function destroy(JobAssembly $assembly)
    {
        $assembly->delete();
    }

    //

    public function enabled(UpdateJobAssemblyEnabledRequest $request)
    {
        $items = $request->all();

        foreach ($items as $item) {
            $assembly = JobAssembly::find($item['id']);
            if ($assembly) {
                $assembly->enabled = $item['enabled'];
                $assembly->save();
            }
        }
    }
}
