<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\Rule;

class UpdateUserRequest extends FormRequest
{
    public function authorize(): bool
    {
        return true;
    }

    public function rules(): array
    {
        return [
            'name' => [
                'required',
            ],

            'email' => [
                'required',
                'email',
                Rule::unique('users')->ignore($this->route()->user->id),
            ],

            'password' => [
                'nullable',
                'confirmed',
                'min:8',
            ],
        ];
    }
}
