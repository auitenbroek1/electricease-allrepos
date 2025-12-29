<?php

namespace App\Policies;

use App\Models\AssemblyTag;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class AssemblyTagPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user): bool
    {
        //
    }

    public function view(User $user, AssemblyTag $tag): bool
    {
        //
    }

    public function create(User $user): bool
    {
        return true;
    }

    public function update(User $user, AssemblyTag $tag): bool
    {
        return true;
    }

    public function delete(User $user, AssemblyTag $tag): bool
    {
        return $user->member_id === 1 || $tag->member_id === $user->member_id;
    }

    public function restore(User $user, AssemblyTag $tag): bool
    {
        //
    }

    public function forceDelete(User $user, AssemblyTag $tag): bool
    {
        //
    }
}
