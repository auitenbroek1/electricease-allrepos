<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class UpdateJobAssemblyRequest extends FormRequest
{
    public function rules(): array
    {
        return [
            'quantity' => 'numeric|gte:0',
            'labor_factor' => 'numeric|gte:0',
            'enabled' => 'boolean',
        ];
    }
}
