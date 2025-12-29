<?php

namespace App\Policies;

use App\Models\Part;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;
use Illuminate\Auth\Access\Response;

class PartFavoritePolicy
{
    use HandlesAuthorization;

    public function create(User $user, Part $part): Response
    {
        if ($user->administrator) {
            return Response::allow();
        }

        if (! $part->member || $part->member->principal) {
            return Response::allow();
        }

        if ($user->member_id === $part->member_id) {
            return Response::allow();
        }

        return Response::deny();
    }

    public function delete(User $user, Part $part): Response
    {
        if ($user->administrator) {
            return Response::allow();
        }

        if (! $part->member || $part->member->principal) {
            return Response::allow();
        }

        if ($user->member_id === $part->member_id) {
            return Response::allow();
        }

        return Response::deny();
    }
}
