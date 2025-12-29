<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobPartRequest;
use App\Http\Requests\UpdateJobPartEnabledRequest;
use App\Http\Requests\UpdateJobPartRequest;
use App\Http\Resources\JobPartResource;
use App\Models\JobPart;
use App\Models\Part;
use Illuminate\Support\Str;

class JobPartController extends Controller
{
    public function index()
    {
        //
    }

    public function store(StoreJobPartRequest $request)
    {
        $phase = $request->input('phase');
        $assembly = $request->input('assembly');
        $parts = $request->input('parts');
        $labor_factor = $request->input('labor_factor') ?? 1;
        $enabled = $request->input('enabled') ?? 1;

        foreach ($parts as $part_id) {
            $part = Part::findOrFail($part_id);

            $job_part = new JobPart;
            $job_part->uuid = Str::uuid();
            $job_part->job_phase_id = $phase;
            $job_part->job_assembly_id = $assembly;
            $job_part->reference_id = $part->id;
            $job_part->cost = $part->cost;
            $job_part->labor = $part->labor;
            $job_part->quantity = 0;
            $job_part->labor_factor = $labor_factor;
            $job_part->enabled = $enabled;
            $job_part->save();
        }
    }

    public function show(JobPart $part)
    {
        //
    }

    public function update(UpdateJobPartRequest $request, JobPart $part)
    {
        $part->cost = $request->input('cost');
        $part->labor = $request->input('labor');
        $part->quantity = $request->input('quantity') ?? 0;
        $part->labor_factor = $request->input('labor_factor') ?? 1;
        $part->enabled = $request->input('enabled') ?? 0;

        $part->save();

        return new JobPartResource($part);
    }

    public function update_partial(UpdateJobPartRequest $request, JobPart $part)
    {
        if ($request->exists('annotation_type')) {
            $part->annotation_type = $request->input('annotation_type');
        }

        if ($request->exists('annotation_symbol_id')) {
            $part->annotation_symbol_id = $request->input('annotation_symbol_id');
        }

        if ($request->exists('annotation_color')) {
            $part->annotation_color = $request->input('annotation_color');
        }

        if ($request->exists('annotation_length')) {
            $part->annotation_length = $request->input('annotation_length');
        }

        $part->save();

        return new JobPartResource($part);
    }

    public function destroy(JobPart $part)
    {
        $part->delete();
    }

    //

    //

    public function enabled(UpdateJobPartEnabledRequest $request)
    {
        $items = $request->all();

        foreach ($items as $item) {
            $part = JobPart::find($item['id']);
            if ($part) {
                $part->enabled = $item['enabled'];
                $part->save();
            }
        }
    }
}
