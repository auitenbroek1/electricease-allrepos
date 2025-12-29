<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class UpdateJobPhaseRequest extends FormRequest
{
    public function rules(): array
    {
        return [
            'name' => 'required',
            // 'area' => 'required',
            // 'cost_code' => 'required',
        ];
    }
}
