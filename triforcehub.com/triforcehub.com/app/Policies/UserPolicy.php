<?php

namespace App\Policies;

use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class UserPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user): bool
    {
        //
    }

    public function view(User $user, User $context): bool
    {
        //
    }

    public function create(User $user): bool
    {
        return true;
    }

    public function update(User $user, User $context): bool
    {
        return true;
    }

    public function delete(User $user, User $context): bool
    {
        return true;
    }

    public function restore(User $user, User $context): bool
    {
        //
    }

    public function forceDelete(User $user, User $context): bool
    {
        //
    }

    //

    public function uploadFiles(User $user)
    {
        return true;
    }
}
