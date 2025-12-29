<?php

namespace App\Actions\Jobs;

use App\Http\Resources\JobResource;

class BuildJobResponse
{
    public function execute(
        $job,
    ) {
        $data = (new JobResource($job->load([
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
            'member',
            'member.logo',
            'member.state',
            'parent',
            'parent.locations',
            'parent.locations.state',
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
        ])))->resolve();

        return $data;
    }
}
