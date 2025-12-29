<?php

namespace App\Http\Controllers;

use App\Helpers\Math;
use App\Http\Requests\StoreJobPlanRequest;
use App\Http\Requests\UpdateJobPlanRequest;
use App\Http\Resources\JobAnnotationResource;
use App\Http\Resources\JobPlanResource;
use App\Models\Job;
use App\Models\JobAnnotation;
use App\Models\JobAssembly;
use App\Models\JobPart;
use App\Models\JobPlan;
use Illuminate\Http\Request;
use Illuminate\Support\Str;

class JobPlanController extends Controller
{
    public function index(Job $job)
    {
        $collection = JobPlan::where(['job_id' => $job->id])->with(['upload'])->get();

        $results = JobPlanResource::collection($collection);

        return $results;
    }

    public function store(StoreJobPlanRequest $request)
    {
        $plan = new JobPlan;
        $plan->uuid = Str::uuid();
        $plan->job_id = $request->input('job_id');
        $plan->upload_id = $request->input('upload_id');
        $plan->save();

        return new JobPlanResource($plan);
    }

    public function show(Job $job, JobPlan $plan)
    {
        return new JobPlanResource($plan->load(['upload']));
    }

    public function update(UpdateJobPlanRequest $request, JobPlan $plan)
    {
        // $plan->update($request->validated());

        // return new JobPlanResource($plan);
    }

    public function destroy(Job $job, JobPlan $plan)
    {
        $plan->delete();
    }

    //

    public function annotations(Job $job, JobPlan $plan, Request $request)
    {
        $size = $request->query('size', 200);

        $collection = JobAnnotation::where(['job_plan_id' => $plan->id])->paginate($size);

        return JobAnnotationResource::collection($collection);
    }

    public function update_quantities(Job $job, JobPlan $plan, Request $request)
    {
        $totals = [];

        //

        foreach ($job->phases as $phase) {
            foreach ($phase->assemblies as $assembly) {
                $uuid = $assembly->uuid;
                if (! isset($totals[$uuid])) {
                    $totals[$uuid] = Math::normalize(0);
                }
            }
            foreach ($phase->parts as $part) {
                $uuid = $part->uuid;
                if (! isset($totals[$uuid])) {
                    $totals[$uuid] = Math::normalize(0);
                }
            }
        }

        // ray($totals);

        //

        $entities = [];

        $job->plans()->get()->each(function ($_plan) use (&$entities) {
            $_plan->annotations()->get()->each(function ($annotation) use (&$entities) {
                $entities[] = [
                    'uuid' => $annotation['entity_uuid'],
                    'quantity' => Math::normalize($annotation['entity_quantity']),
                ];
            });
        });

        // ray($entities);

        //

        foreach ($entities as $entity) {
            $uuid = $entity['uuid'];
            $quantity = $entity['quantity'];
            if (! isset($totals[$uuid])) {
                $totals[$uuid] = Math::normalize(0);
            }
            $totals[$uuid] = Math::add($totals[$uuid], $quantity);
        }

        // ray($totals);

        //

        $entity_uuids_with_annotations = $plan
            ->annotations()
            ->get('entity_uuid')
            ->pluck('entity_uuid')
            ->unique()
            ->toArray();

        $touched_entity_uuids = $request->input('touched_entity_uuids', []);

        array_push($entity_uuids_with_annotations, ...$touched_entity_uuids);
        // override for now
        $entity_uuids_with_annotations = $touched_entity_uuids;

        foreach ($totals as $uuid => $quantity) {
            if (! in_array($uuid, $entity_uuids_with_annotations)) {
                continue;
            }

            $job_assembly = JobAssembly::where('uuid', $uuid)->first();
            if ($job_assembly) {
                $job_assembly->quantity = $quantity;
                $job_assembly->quantity_digital = $quantity;
                $job_assembly->save();

                continue;
            }

            $job_part = JobPart::where('uuid', $uuid)->first();
            if ($job_part) {
                $job_part->quantity = $quantity;
                $job_part->quantity_digital = $quantity;
                $job_part->save();

                continue;
            }
        }
    }
}
