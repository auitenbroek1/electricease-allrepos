<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobAnnotationRequest;
use App\Http\Requests\UpdateJobAnnotationRequest;
use App\Http\Resources\JobAnnotationResource;
use App\Models\JobAnnotation;
use Illuminate\Http\Request;

class JobAnnotationController extends Controller
{
    public function index()
    {
        //
    }

    public function store(StoreJobAnnotationRequest $request)
    {
        $annotation = new JobAnnotation;
        $annotation->uuid = $request->input('uuid');
        $annotation->job_plan_id = $request->input('job_plan_id');
        $annotation->data = $request->input('data');
        $annotation->entity_uuid = $request->input('entity_uuid');
        $annotation->entity_quantity = $request->input('entity_quantity') ?? 0;
        $annotation->save();

        return new JobAnnotationResource($annotation);
    }

    public function show(JobAnnotation $annotation)
    {
        //
    }

    public function update(UpdateJobAnnotationRequest $request, JobAnnotation $annotation)
    {
        $annotation->data = $request->input('data');
        $annotation->entity_uuid = $request->input('entity_uuid');
        $annotation->entity_quantity = $request->input('entity_quantity') ?? 0;
        $annotation->save();

        return new JobAnnotationResource($annotation);
    }

    public function destroy(JobAnnotation $annotation)
    {
        $annotation->delete();
    }

    public function bulk_destroy(Request $request)
    {
        $payload = $request->json();

        $uuids = collect($payload)->pluck('uuid')->toArray();

        // foreach ($uuids as $uuid) {
        //     JobAnnotation::where('uuid', $uuid)->delete();
        // }

        JobAnnotation::whereIn('uuid', $uuids)->delete();
    }
}
