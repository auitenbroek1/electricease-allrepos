<?php

namespace App\Http\Resources;

use App\Helpers\Math;
use Illuminate\Http\Resources\Json\JsonResource;

class JobAssemblyResource extends JsonResource
{
    public function toArray($request): array
    {
        // $original = parent::toArray($request);
        // ray($original);

        //

        $parts = JobPartResource::collection($this->whenLoaded('parts'));

        $cost = 0;
        $labor = 0;

        if ($this->relationLoaded('parts')) {
            $parts = $parts->toArray($request);

            foreach ($parts as $part) {
                $cost = Math::add($cost, $part['cost_total']);
                $labor = Math::add($labor, $part['labor_total']);
            }
        }

        $cost_total = Math::multiply($cost, $this->quantity);
        $labor_total = Math::multiply(Math::multiply($labor, $this->labor_factor ?? 1), $this->quantity);

        //

        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'cost' => $cost,
            'labor' => $labor,
            'quantity' => Math::normalize($this->quantity ?? 0),
            'quantity_digital' => Math::normalize($this->quantity_digital ?? 0),
            'labor_factor' => Math::normalize($this->labor_factor ?? 0),
            'annotation_type' => $this->annotation_type,
            'annotation_symbol_id' => $this->annotation_symbol_id,
            'annotation_color' => $this->annotation_color ?? '#000000',
            'annotation_length' => $this->annotation_length,
            'enabled' => $this->enabled,
            'reference' => new AssemblyResource($this->reference),
            'parts' => $parts,
            'cost_total' => $cost_total,
            'labor_total' => $labor_total,
        ];

        // ray($data);

        //

        return $data;
    }
}
