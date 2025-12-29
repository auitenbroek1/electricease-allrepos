<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;

class UpdateMemberRequest extends FormRequest
{
    public function authorize(): bool
    {
        $member = $this->route()->member;

        return $this->user()->can('update', $member);
    }

    public function rules(): array
    {
        return [
            'name' => 'required',
            'email' => 'required|email',
            'logo_id' => 'nullable',
            'address1' => 'required',
            'address2' => 'nullable',
            'city' => 'required',
            'state_id' => 'required',
            'zip' => 'required',
            'office' => 'required',
            'mobile' => 'nullable',

            'feature_digital_takeoff_enabled' => 'boolean',
            'feature_linear_with_drops_enabled' => 'boolean',
            'feature_auto_count_enabled' => 'boolean',
        ];
    }
}
