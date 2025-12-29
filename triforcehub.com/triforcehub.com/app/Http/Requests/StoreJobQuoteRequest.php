<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class StoreJobQuoteRequest extends FormRequest
{
    public function rules(): array
    {
        return [
            'job_id' => 'required',
            'cost' => 'required|numeric|min:0',
            'enabled' => 'boolean',
        ];
    }
}
