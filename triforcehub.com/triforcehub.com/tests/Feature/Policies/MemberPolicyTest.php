<?php

use App\Models\Member;
use App\Models\User;

test('administrator can manage members', function () {
    $user = User::factory()->administrator()->create();
    $member = Member::factory()->create();

    $this->actingAs($user);

    $this->getJson('/api/members')->assertStatus(200);
    $this->postJson('/api/members')->assertStatus(422);
    $this->getJson('/api/members/'.$member->id)->assertStatus(200);
    $this->patchJson('/api/members/'.$member->id)->assertStatus(422);
    $this->deleteJson('/api/members/'.$member->id)->assertStatus(200);

    $this->getJson('/api/members/'.$user->member->id)->assertStatus(200);
    $this->patchJson('/api/members/'.$user->member->id)->assertStatus(422);
    $this->deleteJson('/api/members/'.$user->member->id)->assertStatus(403);
});

test('principal can manage members', function () {
    $user = User::factory()->principal()->create();
    $member = Member::factory()->create();

    $this->actingAs($user);

    $this->getJson('/api/members')->assertStatus(403);
    $this->postJson('/api/members')->assertStatus(403);
    $this->getJson('/api/members/'.$member->id)->assertStatus(403);
    $this->patchJson('/api/members/'.$member->id)->assertStatus(403);
    $this->deleteJson('/api/members/'.$member->id)->assertStatus(403);

    $this->getJson('/api/members/'.$user->member->id)->assertStatus(200);
    $this->patchJson('/api/members/'.$user->member->id)->assertStatus(422);
    $this->deleteJson('/api/members/'.$user->member->id)->assertStatus(403);
});

test('others cannot', function () {
    $user = User::factory()->create();
    $member = Member::factory()->create();

    $this->actingAs($user);

    $this->getJson('/api/members')->assertStatus(403);
    $this->postJson('/api/members')->assertStatus(403);
    $this->getJson('/api/members/'.$member->id)->assertStatus(403);
    $this->patchJson('/api/members/'.$member->id)->assertStatus(403);
    $this->deleteJson('/api/members/'.$member->id)->assertStatus(403);

    $this->getJson('/api/members/'.$user->member->id)->assertStatus(200);
    $this->patchJson('/api/members/'.$user->member->id)->assertStatus(403);
    $this->deleteJson('/api/members/'.$user->member->id)->assertStatus(403);
});
