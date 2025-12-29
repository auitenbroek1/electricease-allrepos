<?php

use App\Models\Member;
use App\Models\Part;
use App\Models\Unit;
use App\Models\User;

beforeEach(function () {
    $this->member1 = Member::factory()->create();
    $this->member2 = Member::factory()->create();
    $this->user1 = User::factory(['member_id' => $this->member1->id])->create();
    $this->user2 = User::factory(['member_id' => $this->member2->id])->create();
    $this->unit = Unit::factory()->create();
    $this->partByMember1 = Part::factory(['member_id' => $this->member1->id, 'unit_id' => $this->unit->id])->create();
});

test('User can view all parts', function () {
    $this->actingAs($this->user1);
    $this
        ->get('/api/parts')
        ->assertStatus(200);
});

test('User can view its own part', function () {
    $part = Part::factory(['member_id' => $this->member1, 'unit_id' => $this->unit->id])->create();

    $this->actingAs($this->user1);

    $this
        ->get('/api/parts/'.$part->id)
        ->assertStatus(200);
});

test('User cannot view other member part', function () {
    $part = Part::factory(['member_id' => $this->member1, 'unit_id' => $this->unit->id])->create();

    $this->actingAs($this->user2);

    $this
        ->get('/api/parts/'.$part->id)
        ->assertStatus(200);
});

test('User can update an part created by its member', function () {
    $this->actingAs($this->user1);

    $this
        ->patchJson('/api/parts/'.$this->partByMember1->id, [
            'name' => 'Updated Name',
            'cost' => 10,
            'labor' => 1,
        ])
        ->assertStatus(200);
});

test('user cannot update an part created by other member', function () {
    $this->actingAs($this->user2);

    $this
        ->patchJson('/api/parts/'.$this->partByMember1->id, [
            'name' => 'Updated Name',
            'cost' => 10,
            'labor' => 1,
            'unit_id' => $this->unit->id,
        ])
        ->assertStatus(403);
});

test('user can duplicate part', function () {
    $this->actingAs($this->user1);

    $this
        ->postJson('/api/parts/'.$this->partByMember1->id.'/duplicate')
        ->assertStatus(201);
});

test('user cannot duplicate part created by other member', function () {
    $this->actingAs($this->user2);

    $this
        ->postJson('/api/parts/'.$this->partByMember1->id.'/duplicate')
        ->assertStatus(403);
});

test('user can delete an part created by its member', function () {
    $this->actingAs($this->user1);

    $this
        ->deleteJson('/api/parts/'.$this->partByMember1->id)
        ->assertStatus(200);
});

test('user cannot delete an part created by its member', function () {
    $this->actingAs($this->user2);

    $this
        ->deleteJson('/api/parts/'.$this->partByMember1->id)
        ->assertStatus(403);
});
