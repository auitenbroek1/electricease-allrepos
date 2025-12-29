<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class UpdateJobCustomerRequest extends FormRequest
{
    public function authorize(): bool
    {
        return true;
    }

    public function rules(): array
    {
        return [
            'name' => 'required|max:1024',
            'email' => 'email',
            'address1' => 'nullable|string|max:1024',
            'address2' => 'nullable|string|max:1024',
            'city' => 'nullable|string|max:1024',
            'state_id' => 'nullable|numeric',
            'zip' => 'nullable|string|max:10',
            'mobile' => 'nullable|string|max:10',
            'office' => 'nullable|string|max:10',
        ];
    }

    public function messages(): array
    {
        return [
            'name.required' => 'Customer Name field is required.',
        ];
    }
}
