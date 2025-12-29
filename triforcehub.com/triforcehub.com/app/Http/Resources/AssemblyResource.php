<?php

namespace App\Http\Resources;

use App\Helpers\Math;
use Illuminate\Http\Resources\Json\JsonResource;

class AssemblyResource extends JsonResource
{
    public function toArray($request): array
    {
        $cost = 0;
        $labor = 0;

        if ($this->relationLoaded('parts')) {
            foreach ($this->parts as $part) {
                $cost = Math::add($cost, Math::multiply($part->cost, $part->pivot->quantity));
            }

            foreach ($this->parts as $part) {
                $labor = Math::add($labor, Math::multiply($part->labor, $part->pivot->quantity));
            }
        }

        $favorited = false;
        if ($this->relationLoaded('favorites')) {
            foreach ($this->favorites as $user) {
                if ($user->id === $request->user()->id) {
                    $favorited = true;
                    break;
                }
            }
        }

        return [
            'id' => $this->id,
            'name' => $this->name,
            'description' => $this->description,
            'cost' => $cost,
            'labor' => $labor,
            'quantity' => $this->whenPivotLoaded('assembly_part', function () {
                $quantity = Math::normalize($this->pivot->quantity);

                return $quantity;
            }),
            'parts' => PartResource::collection($this->whenLoaded('parts')),
            'categories' => AssemblyCategoryResource::collection($this->whenLoaded('categories')),
            'favorited' => $favorited,
            'tags' => AssemblyTagResource::collection($this->whenLoaded('tags')),
        ];
    }
}
