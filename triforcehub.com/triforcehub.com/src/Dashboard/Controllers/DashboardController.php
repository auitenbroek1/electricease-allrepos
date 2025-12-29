<?php

namespace Src\Dashboard\Controllers;

use App\Http\Controllers\Controller;
use App\Models\Job;
use Illuminate\Http\Request;
use Illuminate\Support\Carbon;

class DashboardController extends Controller
{
    public function __invoke(
        Request $request
    ) {
        $upcoming7 = Job::with(['status'])
            ->where(function ($query) use ($request) {
                return $query
                    ->whereNull('member_id')
                    ->orWhere('member_id', $request->user()->member_id);
            })
            ->where('bid_due_date', '<', Carbon::now()->addDays(7))
            ->count();

        $upcoming30 = Job::with(['status'])
            ->where(function ($query) use ($request) {
                return $query
                    ->whereNull('member_id')
                    ->orWhere('member_id', $request->user()->member_id);
            })
            ->where('bid_due_date', '<', Carbon::now()->addDays(30))
            ->count();

        $wonAll = Job::with(['status'])
            ->where(function ($query) use ($request) {
                return $query
                    ->whereNull('member_id')
                    ->orWhere('member_id', $request->user()->member_id);
            })
            ->where(function ($query) {
                return $query
                    ->where('job_status_id', 2)
                    ->orWhereHas('status', function ($query) {
                        return $query->where('parent_id', 2);
                    });
            })
            ->count();

        $lostAll = Job::with(['status'])
            ->where(function ($query) use ($request) {
                return $query
                    ->whereNull('member_id')
                    ->orWhere('member_id', $request->user()->member_id);
            })
            ->where(function ($query) {
                return $query
                    ->where('job_status_id', 3)
                    ->orWhereHas('status', function ($query) {
                        return $query->where('parent_id', 3);
                    });
            })
            ->count();

        $completedAll = Job::with(['status'])
            ->where(function ($query) use ($request) {
                return $query
                    ->whereNull('member_id')
                    ->orWhere('member_id', $request->user()->member_id);
            })
            ->where(function ($query) {
                return $query
                    ->where('job_status_id', 16)
                    ->orWhereHas('status', function ($query) {
                        return $query->where('parent_id', 16);
                    });
            })
            ->count();

        $data = [
            'upcoming' => [
                'all' => 0,
                'within7days' => $upcoming7,
                'within30days' => $upcoming30 - $upcoming7,
            ],
            'won' => [
                'all' => $wonAll,
            ],
            'lost' => [
                'all' => $lostAll,
            ],
            'completed' => [
                // 'all' => $completedAll,
                'all' => ($wonAll + $lostAll),
            ],
        ];

        return $data;
    }
}
