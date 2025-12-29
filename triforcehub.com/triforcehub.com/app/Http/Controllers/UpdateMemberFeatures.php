<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

class UpdateMemberFeatures extends Controller
{
    public function __invoke(Request $request)
    {
        $member = $request->user()->member;
        $features = $request->input('features', []);

        $member->features()->sync($features);

        return response()->json(['message' => 'Features updated successfully.']);
    }
}
