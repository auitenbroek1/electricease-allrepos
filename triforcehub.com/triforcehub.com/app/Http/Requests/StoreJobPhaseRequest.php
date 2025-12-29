<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class StoreJobPhaseRequest extends FormRequest
{
    public function rules(): array
    {
        return [
            'job_id' => 'required',
            'name' => 'required',
            // 'area' => 'required',
            // 'cost_code' => 'required',
        ];
    }
}
