<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreUserRequest;
use App\Http\Requests\UpdateUserRequest;
use App\Http\Resources\UserResource;
use App\Models\User;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Str;

class UserController extends Controller
{
    public function index(Request $request)
    {
        $q = $request->query('q');
        $size = $request->query('size', 6);

        $collection = User::with(['member'])
            ->when($q, function ($query, $q) {
                return $query
                    ->where('name', 'like', '%'.$q.'%');
            })
            ->paginate($size);

        $results = UserResource::collection($collection);

        return $results;
    }

    public function store(StoreUserRequest $request)
    {
        $this->authorize('create', User::class);

        $user = new User;

        $user->uuid = Str::uuid();
        $user->member_id = $request->input('member_id');
        $user->name = $request->input('name');
        $user->email = $request->input('email');
        $user->password = Hash::make($request->input('password'));

        $user->save();

        return new UserResource($user);
    }

    public function show(User $user)
    {
        return new UserResource($user->load(['member']));
    }

    public function update(UpdateUserRequest $request, User $user)
    {
        $this->authorize('update', $user);

        $user->name = $request->input('name');
        $user->email = $request->input('email');

        if ($request->input('password')) {
            $user->password = Hash::make($request->input('password'));
        }

        $user->save();

        return new UserResource($user);
    }

    public function destroy(User $user)
    {
        $this->authorize('delete', $user);

        $user->delete();

        return new UserResource($user);
    }

    public function profile()
    {
        $user = Auth::user()->load(['member', 'member.distributors']);

        // if (! $user->enabled) {
        //     abort(403);
        // }

        return new UserResource($user);
    }

    public function profileUpdate(Request $request)
    {
        // ray($request->all());

        if ($request->input('password1') && $request->input('password2')) {
            $password1 = $request->input('password1');
            $password2 = $request->input('password2');

            if ($password1 == $password2) {
                $request->user()->fill([
                    'password' => Hash::make($request->input('password1')),
                ])->save();

                return ['success' => true];
            }
        }

        return ['success' => false];
    }
}
