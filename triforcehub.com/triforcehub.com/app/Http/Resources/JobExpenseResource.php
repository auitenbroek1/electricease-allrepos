<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class JobExpenseResource extends JsonResource
{
    public function toArray($request): array
    {
        $cost_total = $this->cost;

        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'name' => $this->name,
            'cost' => round($this->cost, 2),
            'notes' => $this->notes,
            'enabled' => $this->enabled,

            'cost_total' => round($cost_total, 2),
        ];

        return $data;
    }
}
