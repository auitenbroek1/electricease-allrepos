<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class DistributorResource extends JsonResource
{
    public function toArray($request): array
    {
        return [
            'id' => $this->id,
            'name' => $this->name,
            'username' => $this->whenPivotLoaded('distributor_member', function () {
                return $this->pivot->username;
            }),
            'enabled' => $this->whenPivotLoaded('distributor_member', function () {
                return $this->pivot->enabled ? true : false;
            }),
        ];

        return parent::toArray($request);
    }
}
