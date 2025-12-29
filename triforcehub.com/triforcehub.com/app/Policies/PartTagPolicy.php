<?php

namespace App\Policies;

use App\Models\PartTag;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class PartTagPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user): bool
    {
        //
    }

    public function view(User $user, PartTag $tag): bool
    {
        //
    }

    public function create(User $user): bool
    {
        return true;
    }

    public function update(User $user, PartTag $tag): bool
    {
        return true;
    }

    public function delete(User $user, PartTag $tag): bool
    {
        return $user->member_id === 1 || $tag->member_id === $user->member_id;
    }

    public function restore(User $user, PartTag $tag): bool
    {
        //
    }

    public function forceDelete(User $user, PartTag $tag): bool
    {
        //
    }
}
