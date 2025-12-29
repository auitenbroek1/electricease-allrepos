<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class JobPlanResource extends JsonResource
{
    public function toArray($request): array
    {
        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'job_id' => $this->job_id,

            'annotations' => new UploadResource($this->whenLoaded('annotations')),
            'upload' => new UploadResource($this->whenLoaded('upload')),
        ];

        return $data;
    }
}
