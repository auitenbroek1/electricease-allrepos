<?php

namespace App\Http\Controllers;

use App\Actions\Jobs\DuplicateJobAction;
use App\Http\Requests\StoreJobRequest;
use App\Http\Requests\UpdateJobRequest;
use App\Http\Resources\JobIndexResource;
use App\Http\Resources\JobResource;
use App\Http\Resources\JobSummaryResource;
use App\Models\Job;
use App\Models\JobPhase;
use Illuminate\Http\Request;
use Illuminate\Support\Str;
use Src\Bids\Actions\GenerateMaterialSummaryReportAction;

class JobController extends Controller
{
    public function index(Request $request)
    {
        $q = $request->query('q');
        $size = $request->query('size', 6);

        $collection = Job::with([
            'adjustments',
            'blocks',
            'children.status',
            'children',
            'crews',
            'customers.state',
            'customers',
            'events',
            'expenses',
            'files',
            'labors',
            'locations.state',
            'locations',
            'parent',
            'parent.customers',
            'phases.assemblies.parts.reference',
            'phases.assemblies.parts',
            'phases.assemblies.reference',
            'phases.assemblies',
            'phases.parts.reference',
            'phases.parts',
            'phases',
            'quotes',
            'status',
            'status.parent',
            'type',

            'children.adjustments',
            'children.crews',
            'children.expenses',
            'children.labors',
            'children.phases',
            'children.phases.assemblies.parts.reference',
            'children.phases.assemblies.parts',
            'children.phases.assemblies.reference',
            'children.phases.assemblies',
            'children.phases.parts.reference',
            'children.phases.parts',
            'children.quotes',

            'children.parent.customers',
        ])
            ->where(['parent_id' => null])
            ->where(function ($query) use ($request) {
                return $query
                    ->whereNull('member_id')
                    ->orWhere('member_id', $request->user()->member_id);
            })
            ->where(function ($query) use ($q) {
                return $query->when($q, function ($query, $q) {
                    return $query
                        ->where('number', 'like', '%'.$q.'%')
                        ->orWhere('name', 'like', '%'.$q.'%')
                        ->orWhereHas('customers', function ($query) use ($q) {
                            return $query
                                ->where('name', 'like', '%'.$q.'%');
                        });
                });
            })
            ->paginate($size);

        $results = JobIndexResource::collection($collection);

        return $results;
    }

    public function store(StoreJobRequest $request)
    {
        $this->authorize('create', Job::class);

        $job = new Job;

        $job->uuid = Str::uuid();
        $job->member_id = $request->user()->member_id;
        $job->parent_id = $request->input('parent_id');
        $job->job_type_id = $request->input('job_type_id') ?? 1;
        $job->number = $request->input('number') ?? '';
        $job->name = $request->input('name');
        $job->description = $request->input('description');
        $job->temporary_power = $request->input('temporary_power');
        $job->temporary_lighting = $request->input('temporary_lighting');
        $job->sqft = $request->input('sqft');
        $job->labor_factor = $request->input('labor_factor') ?? 1;
        $job->job_status_id = 1;
        $job->save();

        $phase = new JobPhase;
        $phase->uuid = Str::uuid();
        $phase->job_id = $job->id;
        $phase->name = $job->type->name;
        $phase->save();

        return new JobResource($job->load([
            'adjustments',
            'blocks',
            'children',
            'crews',
            'customers',
            'customers.state',
            'events',
            'expenses',
            'files',
            'labors',
            'locations',
            'locations.state',
            'parent',
            'parent.customers',
            'phases',
            'phases.assemblies',
            'phases.assemblies.reference',
            'phases.assemblies.parts',
            'phases.assemblies.parts.reference',
            'phases.parts',
            'phases.parts.reference',
            'quotes',
            'status',
            'type',
        ]));
    }

    public function show($id)
    {
        $job = Job::findOrFail($id);

        $this->authorize('view', $job);

        return new JobResource($job->load([
            'adjustments',
            'blocks',
            'children',
            'crews',
            'customers',
            'customers.state',
            'events',
            'expenses',
            'files',
            'labors',
            'locations',
            'locations.state',
            'parent',
            'parent.customers',
            'phases',
            'phases.assemblies',
            'phases.assemblies.reference',
            'phases.assemblies.parts',
            'phases.assemblies.parts.reference',
            'phases.assemblies.parts.reference.upcs',
            'phases.parts',
            'phases.parts.reference',
            'phases.parts.reference.upcs',
            'quotes',
            'status',
            'type',
        ]));
    }

    public function summary($id)
    {
        $job = Job::findOrFail($id);

        $this->authorize('view', $job);

        $report = (new GenerateMaterialSummaryReportAction)(
            job: $job,
        );

        return array_values($report);

        return new JobSummaryResource($job->load([
            'phases',
            'phases.assemblies',
            'phases.assemblies.reference',
            'phases.assemblies.parts',
            'phases.assemblies.parts.reference',
            'phases.assemblies.parts.reference.upcs',
            'phases.parts',
            'phases.parts.reference',
            'phases.parts.reference.upcs',
        ]));
    }

    public function update(UpdateJobRequest $request, Job $job)
    {
        $this->authorize('update', $job);

        $job->number = $request->input('number') ?? '';
        $job->name = $request->input('name');
        $job->description = $request->input('description');
        $job->temporary_power = $request->input('temporary_power');
        $job->temporary_lighting = $request->input('temporary_lighting');
        $job->sqft = $request->input('sqft');
        $job->labor_factor = $request->input('labor_factor');

        $job->job_status_id = $request->input('job_status_id');

        $job->bid_due_date = $request->input('bid_due_date');
        $job->job_starting_date = $request->input('job_starting_date');
        $job->job_completion_date = $request->input('job_completion_date');
        $job->winning_contractor = $request->input('winning_contractor');
        $job->winning_amount = $request->input('winning_amount');

        $job->save();

        return new JobResource($job->load([
            'adjustments',
            'blocks',
            'children',
            'crews',
            'customers',
            'customers.state',
            'events',
            'expenses',
            'files',
            'labors',
            'locations',
            'locations.state',
            'parent',
            'parent.customers',
            'phases',
            'phases.assemblies',
            'phases.assemblies.reference',
            'phases.assemblies.parts',
            'phases.assemblies.parts.reference',
            'phases.parts',
            'phases.parts.reference',
            'quotes',
            'status',
            'type',
        ]));
    }

    public function destroy(Job $job)
    {
        $this->authorize('delete', $job);

        $job->delete();

        return new JobResource($job);
    }

    //

    public function updateSettings(Job $job, Request $request)
    {
        $this->authorize('update', $job);

        // ray($request->all());

        if ($request->filled('exclude_material_subtotal_from_total')) {
            $job->exclude_material_subtotal_from_total = $request->input('exclude_material_subtotal_from_total');
        }

        if ($request->filled('hide_tax_in_proposal')) {
            $job->hide_tax_in_proposal = $request->input('hide_tax_in_proposal');
        }

        $job->save();

        return new JobResource($job);
    }

    public function duplicate(Job $job)
    {
        $this->authorize('create', Job::class);

        $_job = (new DuplicateJobAction)->execute($job);

        return new JobResource($_job);
    }
}
