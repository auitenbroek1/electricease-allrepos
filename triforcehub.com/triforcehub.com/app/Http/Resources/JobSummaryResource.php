<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class JobSummaryResource extends JsonResource
{
    public function toArray($request): array
    {
        $data = [
            'phases' => JobPhaseSummaryResource::collection($this->whenLoaded('phases')),
        ];

        $relationships = [
            'phases',
        ];

        foreach ($relationships as $relationship) {
            if ($this->relationLoaded($relationship)) {
                $data[$relationship] = $data[$relationship]->toArray($request);
            }
        }

        return $data;
    }
}
