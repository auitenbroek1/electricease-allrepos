<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreImpersonationRequest;
use App\Models\Impersonation;
use App\Models\User;
use Illuminate\Support\Facades\Auth;

class ImpersonationController extends Controller
{
    public function store(StoreImpersonationRequest $request)
    {
        $target = User::find($request->input('subject'));

        $this->authorize('create', [Impersonation::class, $target]);

        session()->put('source_user_id', $request->user()->id);
        session()->put('target_user_id', $target->id);

        Auth::guard('web')->login($target);
    }

    public function destroy()
    {
        $this->authorize('delete', Impersonation::class);

        $source = User::find(session()->get('source_user_id'));

        Auth::guard('web')->login($source);

        session()->forget('source_user_id');
        session()->forget('target_user_id');
    }
}
