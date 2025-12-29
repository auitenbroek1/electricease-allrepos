<?php

use App\Models\Job;
use App\Models\JobPhase;
use App\Models\JobStatus;
use App\Models\JobType;
use App\Models\Member;
use App\Models\User;

beforeEach(function () {
    $this->member1 = Member::factory()->create();
    $this->member2 = Member::factory()->create();
    $this->user1 = User::factory(['member_id' => $this->member1])->create();
    $this->user2 = User::factory(['member_id' => $this->member2])->create();
    $job_type = JobType::factory(['name' => 'Base Bid'])->create();
    $job_status = JobStatus::factory(['name' => 'Bidding'])->create();
    $this->job = Job::factory(['member_id' => $this->member1, 'job_type_id' => $job_type->id, 'job_status_id' => $job_status->id])->create();
    $this->job_phase = JobPhase::factory(['job_id' => $this->job->id])->create();
});

test('User can duplicate the phase', function () {
    $this->actingAs($this->user1);
    $this
        ->postJson('api/jobs/phases/'.$this->job_phase->id.'/duplicate')
        ->assertStatus(201);
});

test('Duplicated phase should belong to same job', function () {
    $this->actingAs($this->user1);
    $response = $this->postJson('api/jobs/phases/'.$this->job_phase->id.'/duplicate');
    $this->assertEquals($this->job->id, $response['data']['job_id']);
});

test('User cannot duplicate the phase of another member', function () {
    $this->actingAs($this->user2);
    $this
        ->postJson('api/jobs/phases/'.$this->job_phase->id.'/duplicate')
        ->assertStatus(403);
});
