<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class JobCustomerResource extends JsonResource
{
    public function toArray($request): array
    {
        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'name' => $this->name,
            'email' => $this->email,
            'address1' => $this->address1,
            'address2' => $this->address2,
            'city' => $this->city,
            'state' => new StateResource($this->whenLoaded('state')),
            'zip' => $this->zip,
            'mobile' => $this->mobile,
            'office' => $this->office,
        ];

        //

        $relationships = [
            'state',
        ];

        foreach ($relationships as $relationship) {
            if ($this->relationLoaded($relationship)) {
                $data[$relationship] = $data[$relationship]->toArray($request);
            }
        }

        //

        return $data;
    }
}
