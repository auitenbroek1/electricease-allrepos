<?php

namespace App\Policies;

use App\Models\Assembly;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class AssemblyPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user): bool
    {
        //
    }

    public function view(User $user, Assembly $assembly): bool
    {
        return $user->member_id === 1 || $assembly->member_id === $user->member_id || $assembly->member_id === null;
    }

    public function create(User $user): bool
    {
        return true;
    }

    public function update(User $user, Assembly $assembly): bool
    {
        return true;

        return $user->member_id === 1 || $assembly->member_id === $user->member_id;
    }

    public function delete(User $user, Assembly $assembly): bool
    {
        return $user->member_id === 1 || $assembly->member_id === $user->member_id;
    }

    public function duplicate(User $user, Assembly $assembly)
    {
        return $user->member_id === 1
            || ($assembly->member_id == null || $assembly->member_id == 1)
            || $assembly->member_id === $user->member_id;
    }

    public function restore(User $user, Assembly $assembly): bool
    {
        //
    }

    public function forceDelete(User $user, Assembly $assembly): bool
    {
        //
    }
}
