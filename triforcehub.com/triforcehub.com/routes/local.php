<?php

use App\Models\User;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Route;

Route::get('/auth/login/now', function () {
    abort_unless(app()->environment('local'), 404);

    Auth::login(User::where(['principal' => true])->first());

    return redirect()->intended();
});

Route::get('/dev-login', function () {
    abort_unless(app()->environment('local'), 404);

    $user = User::where('email', 'test@test.com')->first();

    if (! $user) {
        return response('Test user not found. Run: php artisan db:seed --class=TestUserSeeder', 404);
    }

    Auth::login($user);

    return redirect('/app');
});
