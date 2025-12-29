<?php

namespace App\Http\Resources;

use Illuminate\Http\Resources\Json\JsonResource;

class UserResource extends JsonResource
{
    public function toArray($request): array
    {
        $impersonated = false;
        if ($request->session()->has('source_user_id') && $request->session()->has('target_user_id')) {
            if ($request->session()->get('source_user_id') !== $request->session()->get('target_user_id')) {
                $impersonated = true;
            }
        }

        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'principal' => $this->principal,
            'name' => $this->name,
            'email' => $this->email,
            'enabled' => $this->enabled,

            'member_id' => $this->member_id,
            'member' => new MemberResource($this->whenLoaded('member')),

            'administrator' => $this->administrator,

            'impersonated' => $impersonated,

            // 'session' => session()->all(),
        ];

        return $data;
    }
}
