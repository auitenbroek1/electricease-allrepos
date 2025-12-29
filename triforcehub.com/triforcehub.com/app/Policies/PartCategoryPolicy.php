<?php

namespace App\Policies;

use App\Models\PartCategory;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class PartCategoryPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user): bool
    {
        //
    }

    public function view(User $user, PartCategory $category): bool
    {
        //
    }

    public function create(User $user): bool
    {
        return true;
    }

    public function update(User $user, PartCategory $category): bool
    {
        return true;
    }

    public function delete(User $user, PartCategory $category): bool
    {
        return $user->member_id === 1 || $category->member_id === $user->member_id;
    }

    public function restore(User $user, PartCategory $category): bool
    {
        //
    }

    public function forceDelete(User $user, PartCategory $category): bool
    {
        //
    }
}
