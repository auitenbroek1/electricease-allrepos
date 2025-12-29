<?php

namespace App\Policies;

use App\Models\AssemblyCategory;
use App\Models\User;
use Illuminate\Auth\Access\HandlesAuthorization;

class AssemblyCategoryPolicy
{
    use HandlesAuthorization;

    public function viewAny(User $user): bool
    {
        //
    }

    public function view(User $user, AssemblyCategory $category): bool
    {
        //
    }

    public function create(User $user): bool
    {
        return true;
    }

    public function update(User $user, AssemblyCategory $category): bool
    {
        return true;
    }

    public function delete(User $user, AssemblyCategory $category): bool
    {
        return $user->member_id === 1 || $category->member_id === $user->member_id;
    }

    public function restore(User $user, AssemblyCategory $category): bool
    {
        //
    }

    public function forceDelete(User $user, AssemblyCategory $category): bool
    {
        //
    }
}
