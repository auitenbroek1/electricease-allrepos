<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class StoreJobLocationRequest extends FormRequest
{
    public function authorize(): bool
    {
        return true;
    }

    public function rules(): array
    {
        return [
            'job_id' => 'required',
            'name' => 'required|string|max:1024',
            'address1' => 'nullable|string|max:1024',
            'address2' => 'nullable|string|max:1024',
            'city' => 'nullable|string|max:1024',
            'state_id' => 'nullable|numeric',
            'zip' => 'nullable|string|max:10',
        ];
    }

    public function messages(): array
    {
        return [
            'name.required' => 'Location Name field is required.',
        ];
    }
}
