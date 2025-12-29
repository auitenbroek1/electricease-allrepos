<?php

use App\Models\Member;
use App\Models\Part;
use App\Models\Unit;
use App\Models\User;

beforeEach(function () {
    $this->member0 = Member::factory()->principal()->create();
    $this->member1 = Member::factory()->create();
    $this->member2 = Member::factory()->create();

    $this->user0 = User::factory(['member_id' => $this->member0])->create();
    $this->user1 = User::factory(['member_id' => $this->member1])->create();
    $this->user2 = User::factory(['member_id' => $this->member2])->create();

    $unit = Unit::factory()->create();

    $this->part0 = Part::factory(['member_id' => $this->member0, 'unit_id' => $unit->id])->create();
    $this->part1 = Part::factory(['member_id' => $this->member1, 'unit_id' => $unit->id])->create();
    $this->part2 = Part::factory(['member_id' => $this->member2, 'unit_id' => $unit->id])->create();
});

test('administrator can manage a favorite for all parts', function (Part $part) {
    $this->actingAs($this->user0);

    $endpoint = '/api/parts/'.$part->id.'/favorites';

    $this->postJson($endpoint)->assertStatus(201);
    expect($part->favorites()->count())->toBe(1);

    $this->deleteJson($endpoint)->assertStatus(200);
    expect($part->favorites()->count())->toBe(0);
})->with([
    'default' => fn () => $this->part0,
    'member 1' => fn () => $this->part1,
    'member 2' => fn () => $this->part2,
]);

test('user can manage a favorite', function (Part $part) {
    $this->actingAs($this->user1);

    $endpoint = '/api/parts/'.$part->id.'/favorites';

    $this->postJson($endpoint)->assertStatus(201);
    expect($part->favorites()->count())->toBe(1);

    $this->deleteJson($endpoint)->assertStatus(200);
    expect($part->favorites()->count())->toBe(0);
})->with([
    'default part' => fn () => $this->part0,
    'their part' => fn () => $this->part1,
]);

test('user cannot manage a favorite', function (Part $part) {
    $this->actingAs($this->user1);

    $endpoint = '/api/parts/'.$part->id.'/favorites';

    $this->postJson($endpoint)->assertStatus(403);
    $this->deleteJson($endpoint)->assertStatus(403);
})->with([
    'other part' => fn () => $this->part2,
]);
