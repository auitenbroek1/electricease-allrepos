<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class UpdateJobRequest extends FormRequest
{
    public function authorize(): bool
    {
        return true;
    }

    public function rules(): array
    {
        return [
            'job_type_id' => 'numeric',
            'name' => 'required',
            'temporary_power' => 'boolean',
            'temporary_lighting' => 'boolean',

            'job_status_id' => 'numeric',

            'bid_due_date' => 'nullable',
            'job_starting_date' => 'nullable',
            'job_completion_date' => 'nullable',
            'winning_contractor' => 'nullable',
            'winning_amount' => 'nullable',
        ];
    }
}
