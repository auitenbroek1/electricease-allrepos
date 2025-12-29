<?php

use App\Models\Assembly;
use App\Models\Member;
use App\Models\User;

beforeEach(function () {
    $this->member0 = Member::factory()->principal()->create();
    $this->member1 = Member::factory()->create();
    $this->member2 = Member::factory()->create();

    $this->user0 = User::factory(['member_id' => $this->member0])->create();
    $this->user1 = User::factory(['member_id' => $this->member1])->create();
    $this->user2 = User::factory(['member_id' => $this->member2])->create();

    $this->assembly0 = Assembly::factory(['member_id' => $this->member0])->create();
    $this->assembly1 = Assembly::factory(['member_id' => $this->member1])->create();
    $this->assembly2 = Assembly::factory(['member_id' => $this->member2])->create();
});

test('administrator can manage a favorite for all assemblies', function (Assembly $assembly) {
    $this->actingAs($this->user0);

    $endpoint = '/api/assemblies/'.$assembly->id.'/favorites';

    $this->postJson($endpoint)->assertStatus(201);
    expect($assembly->favorites()->count())->toBe(1);

    $this->deleteJson($endpoint)->assertStatus(200);
    expect($assembly->favorites()->count())->toBe(0);
})->with([
    'default' => fn () => $this->assembly0,
    'member 1' => fn () => $this->assembly1,
    'member 2' => fn () => $this->assembly2,
]);

test('user can manage a favorite', function (Assembly $assembly) {
    $this->actingAs($this->user1);

    $endpoint = '/api/assemblies/'.$assembly->id.'/favorites';

    $this->postJson($endpoint)->assertStatus(201);
    expect($assembly->favorites()->count())->toBe(1);

    $this->deleteJson($endpoint)->assertStatus(200);
    expect($assembly->favorites()->count())->toBe(0);
})->with([
    'default assembly' => fn () => $this->assembly0,
    'their assembly' => fn () => $this->assembly1,
]);

test('user cannot manage a favorite', function (Assembly $assembly) {
    $this->actingAs($this->user1);

    $endpoint = '/api/assemblies/'.$assembly->id.'/favorites';

    $this->postJson($endpoint)->assertStatus(403);
    $this->deleteJson($endpoint)->assertStatus(403);
})->with([
    'other assembly' => fn () => $this->assembly2,
]);
