<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class UpdateJobMaterialPriceRequest extends FormRequest
{
    public function rules(): array
    {
        return [
            '*.price' => 'numeric|min:0',
        ];
    }

    public function messages(): array
    {
        return [
            'min' => [
                'numeric' => 'The cost must be greater than or equal to 0.',
            ],
        ];
    }
}
