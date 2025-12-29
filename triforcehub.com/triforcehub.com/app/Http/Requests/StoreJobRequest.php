<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class StoreJobRequest extends FormRequest
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
        ];
    }
}
