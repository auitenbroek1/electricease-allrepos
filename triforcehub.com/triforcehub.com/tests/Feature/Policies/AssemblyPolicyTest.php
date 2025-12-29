<?php

use App\Models\Assembly;
use App\Models\Member;
use App\Models\User;

beforeEach(function () {
    $this->member1 = Member::factory()->create();
    $this->member2 = Member::factory()->create();
    $this->user1 = User::factory(['member_id' => $this->member1])->create();
    $this->user2 = User::factory(['member_id' => $this->member2])->create();
    $this->assemblyByMember1 = Assembly::factory(['member_id' => $this->member1])->create();
});

test('User can view all assemblies', function () {
    $this->actingAs($this->user1);
    $this
        ->get('/api/assemblies')
        ->assertStatus(200);
});

test('User can view its own assembly', function () {
    $assembly = Assembly::factory(['member_id' => $this->member1])->create();

    $this->actingAs($this->user1);

    $this
        ->get('/api/assemblies/'.$assembly->id)
        ->assertStatus(200);
});

test('User cannot view other member assembly', function () {
    $assembly = Assembly::factory(['member_id' => $this->member1])->create();

    $this->actingAs($this->user2);

    $this
        ->get('/api/assemblies/'.$assembly->id)
        ->assertStatus(200);
});

test('User can create an assembly', function () {
    $this->actingAs($this->user1);
    $this
        ->postJson('/api/assemblies', [
            'name' => 'assembly created by user1',
        ])
        ->assertStatus(201);
});

test('User can update an assembly created by its member', function () {
    $this->actingAs($this->user1);

    $this
        ->patchJson('/api/assemblies/'.$this->assemblyByMember1->id, [
            'name' => 'Updated Name',
        ])
        ->assertStatus(200);
});

test('user cannot update an assembly created by other member', function () {
    $this->actingAs($this->user2);

    $this
        ->patchJson('/api/assemblies/'.$this->assemblyByMember1->id, [
            'name' => 'Updated Name',
        ])
        ->assertStatus(403);
});

test('user can delete an assembly created by its member', function () {
    $assembly = Assembly::factory(['member_id' => $this->member1])->create();

    $this->actingAs($this->user1);

    $this
        ->deleteJson('/api/assemblies/'.$assembly->id)
        ->assertStatus(200);
});

test('user cannot delete an assembly created by its member', function () {
    $assembly = Assembly::factory(['member_id' => $this->member1])->create();

    $this->actingAs($this->user2);

    $this
        ->deleteJson('/api/assemblies/'.$assembly->id)
        ->assertStatus(403);
});

test('user can duplicate assembly', function () {
    $assembly = Assembly::factory(['member_id' => $this->member1])->create();

    $this->actingAs($this->user1);

    $this
        ->postJson('/api/assemblies/'.$assembly->id.'/duplicate')
        ->assertStatus(201);
});

test('user cannot duplicate assembly created by other member', function () {
    $assembly = Assembly::factory(['member_id' => $this->member1])->create();

    $this->actingAs($this->user2);

    $this
        ->postJson('/api/assemblies/'.$assembly->id.'/duplicate')
        ->assertStatus(403);
});
