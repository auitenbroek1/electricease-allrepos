<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;
use Illuminate\Support\Carbon;

class MemberResource extends JsonResource
{
    public function toArray($request): array
    {
        $original = parent::toArray($request);

        // ray($original);

        $created_at = Carbon::parse($this->created_at);
        $now = Carbon::now();
        $diff = $created_at->diffInDays($now);
        $show_get_started = $diff < 15;

        //

        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'principal' => $this->principal,
            'customer' => $this->customer,
            'name' => $this->name,
            'email' => $this->email,
            'logo_id' => $this->logo_id,
            'address1' => $this->address1,
            'address2' => $this->address2,
            'city' => $this->city,
            'state_id' => $this->state_id,
            'zip' => $this->zip,
            'office' => $this->office,
            'mobile' => $this->mobile,
            'enabled' => $this->enabled,

            'distributors' => new DistributorCollection($this->whenLoaded('distributors')),
            'features' => $this->features,
            'logo' => new LogoResource($this->whenLoaded('logo')),
            'state' => new StateResource($this->whenLoaded('state')),
            'users' => new UserCollection($this->whenLoaded('users')),

            'show_get_started' => $show_get_started,

            'feature_digital_takeoff_enabled' => $this->feature_digital_takeoff_enabled,
            'feature_linear_with_drops_enabled' => $this->feature_linear_with_drops_enabled,
            'feature_auto_count_enabled' => $this->feature_auto_count_enabled,
        ];

        // ray($data);

        return $data;
    }
}
