<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class StoreJobAdjustmentRequest extends FormRequest
{
    public function authorize(): bool
    {
        return true;
    }

    public function rules(): array
    {
        return [
            'job_id' => 'required',
            'slug' => 'required',
            'override' => 'nullable|numeric|gte:0',
            'overhead' => 'nullable|numeric|gte:0',
            'profit' => 'nullable|numeric|gte:0',
            'tax' => 'nullable|numeric|gte:0',
            'enabled' => 'nullable|boolean',
        ];
    }
}
