<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class UpdateJobPartRequest extends FormRequest
{
    public function rules(): array
    {
        return [
            'cost' => 'numeric|gte:0',
            'labor' => 'numeric|gte:0',
            'quantity' => 'numeric|gte:0',
            'labor_factor' => 'numeric|gte:0',
            'enabled' => 'boolean',
        ];
    }
}
