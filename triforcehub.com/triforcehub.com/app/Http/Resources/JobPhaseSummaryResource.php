<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class JobPhaseSummaryResource extends JsonResource
{
    public function toArray($request): array
    {
        $assemblies = JobAssemblyResource::collection($this->whenLoaded('assemblies'));
        $parts = JobPartResource::collection($this->whenLoaded('parts'));

        if ($this->relationLoaded('assemblies') && $this->relationLoaded('parts')) {
            $assemblies = $assemblies->toArray($request);
            $parts = $parts->toArray($request);

            $assemblies = array_filter($assemblies,
                function ($assembly) {
                    if ($assembly['enabled']) {
                        return $assembly;
                    }
                }
            );

            $parts = array_filter($parts,
                function ($part) {
                    if ($part['enabled']) {
                        return $part;
                    }
                }
            );
        }

        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'name' => $this->name,
            'area' => $this->area,
            'cost_code' => $this->cost_code,
            'job_id' => $this->job->id,
            'assemblies' => $assemblies,
            'parts' => $parts,
        ];

        return $data;
    }
}
