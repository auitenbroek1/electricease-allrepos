<?php

namespace App\Http\Resources;

use App\Helpers\Math;
use Illuminate\Http\Resources\Json\JsonResource;

class PartResource extends JsonResource
{
    public function toArray($request): array
    {
        $cost = Math::normalize($this->cost);
        $labor = Math::normalize($this->labor);

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
            'assemblies' => AssemblyResource::collection($this->whenLoaded('assemblies')),
            'categories' => PartCategoryResource::collection($this->whenLoaded('categories')),
            'favorited' => $favorited,
            'tags' => PartTagResource::collection($this->whenLoaded('tags')),
            'upcs' => PartUPCResource::collection($this->whenLoaded('upcs')),
        ];
    }
}
