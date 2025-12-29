<?php

use App\Models\User;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Route;

Route::get('/auth/login/now', function () {
    abort_unless(app()->environment('local'), 404);

    Auth::login(User::where(['principal' => true])->first());

    return redirect()->intended();
});
