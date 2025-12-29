<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class JobCrewResource extends JsonResource
{
    public function toArray($request): array
    {
        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'name' => $this->name,
            'quantity' => round($this->quantity, 2),
            'rate' => round($this->rate, 2),
            'burden' => round($this->burden, 2),
            'fringe' => round($this->fringe, 2),
            'notes' => $this->notes,
            'enabled' => $this->enabled,
        ];

        return $data;
    }
}
