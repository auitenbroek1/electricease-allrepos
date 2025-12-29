<?php

namespace App\Http\Resources;

use App\Helpers\Math;
use Illuminate\Http\Resources\Json\JsonResource;

class JobPartResource extends JsonResource
{
    public function toArray($request): array
    {
        $cost = Math::normalize($this->cost ?? 0);
        $labor = Math::normalize($this->labor ?? 0);
        $quantity = Math::normalize($this->quantity ?? 0);
        $quantity_digital = Math::normalize($this->quantity_digital ?? 0);
        $labor_factor = Math::normalize($this->labor_factor ?? 1);

        $cost_total = Math::multiply($cost, $quantity);
        $labor_total = Math::multiply(Math::multiply($labor, $labor_factor), $quantity);

        //

        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'cost' => $cost,
            'labor' => $labor,
            'quantity' => $quantity,
            'quantity_digital' => $quantity_digital,
            'labor_factor' => $labor_factor,
            'annotation_type' => $this->annotation_type,
            'annotation_symbol_id' => $this->annotation_symbol_id,
            'annotation_color' => $this->annotation_color ?? '#000000',
            'annotation_length' => $this->annotation_length,
            'enabled' => $this->enabled,
            'reference' => new PartResource($this->reference),
            'cost_total' => $cost_total,
            'labor_total' => $labor_total,
        ];

        //

        return $data;
    }
}
