<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class StateResource extends JsonResource
{
    public function toArray($request): array
    {
        if (! isset($this->id)) {
            return [];
        }

        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'name' => $this->name,
            'abbreviation' => $this->abbreviation,
        ];

        return $data;
    }
}
