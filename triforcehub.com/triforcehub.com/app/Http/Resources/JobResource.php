<?php

namespace App\Http\Resources;

use App\Actions\Jobs\BuildJobSummary;
use Illuminate\Http\Resources\Json\JsonResource;
use Illuminate\Support\Carbon;

class JobResource extends JsonResource
{
    public function toArray($request): array
    {
        // $original = parent::toArray($request);
        // ray($original);

        //

        $data = [
            'id' => $this->id,
            'uuid' => $this->uuid,
            'number' => $this->number,
            'name' => $this->name,
            'description' => $this->description,
            'temporary_power' => $this->temporary_power,
            'temporary_lighting' => $this->temporary_lighting,
            'sqft' => $this->sqft,
            'labor_factor' => $this->labor_factor,

            'bid_due_date' => $this->bid_due_date,
            'proposal_sent_at' => $this->proposal_sent_at ? Carbon::parse($this->proposal_sent_at)->setTimezone('America/Chicago')->toDayDateTimeString() : null,
            'job_starting_date' => $this->job_starting_date,
            'job_completion_date' => $this->job_completion_date,
            'winning_contractor' => $this->winning_contractor,
            'winning_amount' => $this->winning_amount,

            'adjustments' => JobAdjustmentResource::collection($this->whenLoaded('adjustments')),
            'blocks' => JobBlockResource::collection($this->whenLoaded('blocks')),
            'children' => JobResource::collection($this->whenLoaded('children')),
            'crews' => JobCrewResource::collection($this->whenLoaded('crews')),
            'customers' => JobCustomerResource::collection($this->whenLoaded('customers')),
            'events' => JobEventResource::collection($this->whenLoaded('events')),
            'expenses' => JobExpenseResource::collection($this->whenLoaded('expenses')),
            'files' => JobFileResource::collection($this->whenLoaded('files')),
            'labors' => JobLaborResource::collection($this->whenLoaded('labors')),
            'locations' => JobLocationResource::collection($this->whenLoaded('locations')),
            'member' => new MemberResource($this->whenLoaded('member')),
            'parent' => new JobResource($this->whenLoaded('parent')),
            'phases' => JobPhaseResource::collection($this->whenLoaded('phases')),
            'quotes' => JobQuoteResource::collection($this->whenLoaded('quotes')),
            'status' => new JobStatusResource($this->whenLoaded('status')),
            'type' => new JobTypeResource($this->whenLoaded('type')),
        ];

        //

        $relationships = [
            'adjustments',
            'blocks',
            'crews',
            'customers',
            'expenses',
            'labors',
            'locations',
            'member',
            'phases',
            'quotes',
        ];

        foreach ($relationships as $relationship) {
            if ($this->relationLoaded($relationship)) {
                $data[$relationship] = $data[$relationship]->toArray($request);
            }
        }

        //

        $data['settings'] = [
            'exclude_material_subtotal_from_total' => $this->exclude_material_subtotal_from_total,
            'hide_tax_in_proposal' => $this->hide_tax_in_proposal,
        ];

        $data['summary'] = (new BuildJobSummary)->execute(
            phases: $data['phases'],
            crews: $data['crews'],
            labors: $data['labors'],
            expenses: $data['expenses'],
            quotes: $data['quotes'],
            adjustments: $data['adjustments'],
            settings: $data['settings'],
        );

        //

        // ray($data);

        return $data;
    }
}
