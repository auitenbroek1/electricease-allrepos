<?php

namespace App\Policies;

use App\Models\Assembly;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;
use Illuminate\Auth\Access\Response;

class AssemblyFavoritePolicy
{
    use HandlesAuthorization;

    public function create(User $user, Assembly $assembly): Response
    {
        if ($user->administrator) {
            return Response::allow();
        }

        if (! $assembly->member || $assembly->member->principal) {
            return Response::allow();
        }

        if ($user->member_id === $assembly->member_id) {
            return Response::allow();
        }

        return Response::deny();
    }

    public function delete(User $user, Assembly $assembly): Response
    {
        if ($user->administrator) {
            return Response::allow();
        }

        if (! $assembly->member || $assembly->member->principal) {
            return Response::allow();
        }

        if ($user->member_id === $assembly->member_id) {
            return Response::allow();
        }

        return Response::deny();
    }
}
