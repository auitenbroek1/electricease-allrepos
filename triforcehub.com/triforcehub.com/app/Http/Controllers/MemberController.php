<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreMemberRequest;
use App\Http\Requests\UpdateMemberRequest;
use App\Http\Resources\MemberResource;
use App\Jobs\SetupJumpStart;
use App\Models\Job;
use App\Models\Member;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;

class MemberController extends Controller
{
    public function index(Request $request)
    {
        $this->authorize('viewAny', Member::class);

        $q = $request->query('q');
        $size = $request->query('size', 6);

        $collection = Member::with(['state', 'users'])
            ->orderBy('name')
            ->where(function ($query) use ($q) {
                return $query->when($q, function ($query, $q) {
                    return $query
                        ->where('id', 'like', '%'.$q.'%')
                        ->orWhere('name', 'like', '%'.$q.'%')
                        ->orWhere('email', 'like', '%'.$q.'%')
                        ->orWhere('customer', 'like', '%'.$q.'%')
                        ->orWhereHas('users', function ($query) use ($q) {
                            return $query
                                ->where('name', 'like', '%'.$q.'%')
                                ->orWhere('email', 'like', '%'.$q.'%');
                        });
                });
            })
            ->paginate($size);

        $results = MemberResource::collection($collection);

        return $results;
    }

    public function store(StoreMemberRequest $request)
    {
        $this->authorize('create', Member::class);

        $member = Member::create($request->validated());

        return new MemberResource($member);
    }

    public function show(Member $member)
    {
        $this->authorize('view', $member);

        return new MemberResource($member->load(['logo', 'state', 'users']));
    }

    public function update(UpdateMemberRequest $request, Member $member)
    {
        $this->authorize('update', $member);

        $member->update($request->validated());

        return new MemberResource($member);
    }

    public function destroy(Member $member)
    {
        $this->authorize('delete', $member);

        //

        DB::table('users')
            ->where('member_id', $member->id)
            ->delete();

        //

        // DB::table('jobs')->where('member_id', $member->id)->get()->each(function($job) {
        //     DB::table('job_parts')->where('job_id', $job->id)->delete();
        //     DB::table('job_assemblies')->where('job_id', $job->id)->delete();
        // });

        //

        DB::table('jobs')
            ->where('member_id', $member->id)
            ->whereNotNull('parent_id')
            ->delete();

        DB::table('jobs')
            ->where('member_id', $member->id)
            ->delete();

        //

        DB::table('parts')
            ->where('member_id', $member->id)
            ->delete();

        DB::table('part_categories')
            ->where('member_id', $member->id)
            ->delete();

        DB::table('part_tags')
            ->where('member_id', $member->id)
            ->delete();

        //

        DB::table('assemblies')
            ->where('member_id', $member->id)
            ->delete();

        DB::table('assembly_categories')
            ->where('member_id', $member->id)
            ->delete();

        DB::table('assembly_tags')
            ->where('member_id', $member->id)
            ->delete();

        //

        $member->delete();

        return new MemberResource($member);
    }

    public function jumpstart_index(Member $member)
    {
        $this->authorize('view', $member);

        $jobs = Job::where('member_id', $member->id)->where('name', 'like', '%JumpStart%')->get();

        return $jobs;
    }

    public function jumpstart_store(Member $member)
    {
        $this->authorize('update', $member);

        SetupJumpStart::dispatch($member->id);

        return [];
    }

    public function jumpstart_destroy(Member $member)
    {
        $this->authorize('delete', $member);

        Job::where('member_id', $member->id)->where('name', 'like', '%JumpStart%')->delete();

        return [];
    }
}
