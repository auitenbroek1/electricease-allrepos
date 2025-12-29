<?php

use App\Models\User;

test('it can render auth login', function () {
    $this->get('/auth/login')->assertStatus(200);
});

test('it will redirect if not authenticated', function () {
    $this->get('/app')->assertRedirect();
});

test('it will not redirect if authenticated', function () {
    $this->withoutVite();

    $user = User::factory()->create();

    $this->actingAs($user)->get('/app')->assertStatus(200);
});
