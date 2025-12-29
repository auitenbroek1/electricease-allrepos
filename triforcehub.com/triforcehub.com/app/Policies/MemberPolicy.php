<?php

namespace App\Policies;

use App\Models\Member;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;
use Illuminate\Auth\Access\Response;

class MemberPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user): Response
    {
        if ($user->administrator) {
            return Response::allow();
        }

        return Response::deny();
    }

    public function view(User $user, Member $member): Response
    {
        if ($user->administrator) {
            return Response::allow();
        }

        if ($user->member_id === $member->id) {
            return Response::allow();
        }

        return Response::deny();
    }

    public function create(User $user): Response
    {
        if ($user->administrator) {
            return Response::allow();
        }

        return Response::deny();
    }

    public function update(User $user, Member $member): Response
    {
        if ($user->administrator) {
            return Response::allow();
        }

        // if (! $user->principal) {
        //     return Response::deny();
        // }

        if ($user->member_id === $member->id) {
            return Response::allow();
        }

        return Response::deny();
    }

    public function delete(User $user, Member $member): Response
    {
        if ($user->member_id === $member->id) {
            return Response::deny();
        }

        if ($user->administrator) {
            return Response::allow();
        }

        return Response::deny();
    }
}
