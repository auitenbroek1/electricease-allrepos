<?php

namespace App\Policies;

use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;
use Illuminate\Auth\Access\Response;

class ImpersonationPolicy
{
    use HandlesAuthorization;

    public function create(User $user, ?User $subject): Response
    {
        if (! $user->administrator) {
            return Response::deny('You must be an administrator.');
        }

        if (! $subject) {
            return Response::deny();
        }

        if ($subject->administrator) {
            return Response::deny();
        }

        return Response::allow();
    }

    public function delete(User $user): Response
    {
        return Response::allow();
    }
}
