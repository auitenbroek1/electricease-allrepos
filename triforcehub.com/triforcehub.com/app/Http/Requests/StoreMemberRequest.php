<?php

namespace App\Http\Requests;

use App\Models\Member;
use Illuminate\Foundation\Http\FormRequest;

class StoreMemberRequest extends FormRequest
{
    public function authorize(): bool
    {
        return $this->user()->can('create', Member::class);
    }

    public function rules(): array
    {
        return [
            'principal' => 'prohibited',
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
