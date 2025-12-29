<?php

namespace App\Policies;

use App\Models\Job;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class JobPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user): bool
    {
        //
    }

    public function view(User $user, Job $job): bool
    {
        return $user->member_id === 1 || $job->member_id === $user->member_id;
    }

    public function create(User $user): bool
    {
        return true;
    }

    public function update(User $user, Job $job): bool
    {
        return $user->member_id === 1 || $job->member_id === $user->member_id;
    }

    public function delete(User $user, Job $job): bool
    {
        return $user->member_id === 1 || $job->member_id === $user->member_id;
    }

    public function restore(User $user, Job $job): bool
    {
        //
    }

    public function forceDelete(User $user, Job $job): bool
    {
        //
    }
}
