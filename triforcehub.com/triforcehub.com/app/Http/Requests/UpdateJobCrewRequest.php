<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class UpdateJobCrewRequest extends FormRequest
{
    public function authorize(): bool
    {
        return true;
    }

    public function rules(): array
    {
        return [
            'name' => 'required|max:1024',
            'quantity' => 'numeric|gt:0',
            'rate' => 'numeric|gte:0',
            'burden' => 'numeric|gte:0',
            'fringe' => 'numeric|gte:0',
            'notes' => 'string|max:1024',
            'enabled' => 'boolean',
        ];
    }
}
