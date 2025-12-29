<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class StoreJobCrewRequest extends FormRequest
{
    public function authorize(): bool
    {
        return true;
    }

    public function rules(): array
    {
        return [
            'job_id' => 'required',
            'quantity' => 'required',
            'rate' => 'required',
            'burden' => 'required',
            'fringe' => 'required',
        ];
    }
}
