<?php

namespace App\Http\Resources;

use App\Helpers\Math;
use Illuminate\Http\Resources\Json\JsonResource;

class JobLaborResource extends JsonResource
{
    public function toArray($request): array
    {
        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'name' => $this->name,
            'hours' => $this->hours ? round($this->hours, 2) : '',
            'rate' => $this->rate ? round($this->rate, 2) : '',
            'burden' => $this->burden ? round($this->burden, 2) : '',
            'fringe' => $this->fringe ? round($this->fringe, 2) : '',
            'notes' => $this->notes,
            'enabled' => $this->enabled,
        ];

        //

        $cost = Math::round(Math::multiply($data['hours'], $data['rate']));
        $burden_total = Math::round(Math::multiply($cost, Math::divide($data['burden'], 100)));
        $fringe_total = Math::round(Math::multiply($data['hours'], $data['fringe']));
        $cost_total = Math::round(Math::add(Math::add($cost, $burden_total), $fringe_total));
        $rate_total = $data['hours'] > 0 ? Math::round(Math::divide($cost_total, $data['hours'])) : 0;

        $data['cost'] = $cost;
        $data['burden_total'] = $burden_total;
        $data['fringe_total'] = $fringe_total;
        $data['cost_total'] = $cost_total;
        $data['rate_total'] = $rate_total;

        //

        return $data;
    }
}
