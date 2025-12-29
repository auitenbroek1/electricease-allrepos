<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class JobPhaseResource extends JsonResource
{
    public function toArray($request): array
    {
        // $original = parent::toArray($request);
        // ray($original);

        //

        $assemblies = JobAssemblyResource::collection($this->whenLoaded('assemblies'));
        $parts = JobPartResource::collection($this->whenLoaded('parts'));

        $cost = 0;
        $labor = 0;

        if ($this->relationLoaded('assemblies') && $this->relationLoaded('parts')) {
            $assemblies = $assemblies->toArray($request);
            $parts = $parts->toArray($request);

            if ($assemblies) {
                foreach ($assemblies as $assembly) {
                    if (! $assembly['enabled']) {
                        continue;
                    }
                    $cost = bcadd($cost, $assembly['cost_total'], 2);
                    $labor = bcadd($labor, $assembly['labor_total'], 2);
                }
            }

            if ($parts) {
                foreach ($parts as $part) {
                    if (! $part['enabled']) {
                        continue;
                    }
                    $cost = bcadd($cost, $part['cost_total'], 2);
                    $labor = bcadd($labor, $part['labor_total'], 2);
                }
            }
        }

        $cost_total = $cost;
        $labor_total = $labor;

        //

        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'name' => $this->name,
            'area' => $this->area,
            'cost_code' => $this->cost_code,
            'job_id' => $this->job->id,
            'assemblies' => $assemblies,
            'parts' => $parts,

            'cost' => $cost,
            'cost_total' => $cost_total,
            'labor' => $labor,
            'labor_total' => $labor_total,
        ];

        // ray($data);

        //

        return $data;
    }
}
