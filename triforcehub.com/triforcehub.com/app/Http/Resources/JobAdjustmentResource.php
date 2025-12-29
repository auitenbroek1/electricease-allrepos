<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class JobAdjustmentResource extends JsonResource
{
    public function toArray($request): array
    {
        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'slug' => $this->slug,
            'override' => $this->override ? round($this->override, 2) : null,
            'overhead' => $this->overhead ? round($this->overhead, 2) : null,
            'profit' => $this->profit ? round($this->profit, 2) : null,
            'tax' => $this->tax ? round($this->tax, 4) : null,
            'enabled' => $this->enabled,
        ];

        return $data;
    }
}
