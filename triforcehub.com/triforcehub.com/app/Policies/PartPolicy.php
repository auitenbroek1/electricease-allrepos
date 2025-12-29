<?php

namespace App\Policies;

use App\Models\Part;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class PartPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user): bool
    {
        //
    }

    public function view(User $user, Part $part): bool
    {
        return $user->member_id === 1 || $part->member_id === $user->member_id || $part->member_id === null;
    }

    public function create(User $user): bool
    {
        return true;
    }

    public function update(User $user, Part $part): bool
    {
        return true;

        return $user->member_id === 1 || $part->member_id === $user->member_id;
    }

    public function delete(User $user, Part $part): bool
    {
        return $user->member_id === 1 || $part->member_id === $user->member_id;
    }

    public function duplicate(User $user, Part $part)
    {
        return ($part->member_id == null || $part->member_id == 1)
            || $part->member_id === $user->member_id;
    }

    public function restore(User $user, Part $part): bool
    {
        //
    }

    public function forceDelete(User $user, Part $part): bool
    {
        //
    }
}
