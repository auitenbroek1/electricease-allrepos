<?php

namespace App\Jobs;

use App\Actions\Jobs\DuplicateJobAction;
use App\Models\Job;
use App\Models\Member;
use Illuminate\Bus\Queueable;
use Illuminate\Contracts\Queue\ShouldBeUnique;
use Illuminate\Contracts\Queue\ShouldQueue;
use Illuminate\Foundation\Bus\Dispatchable;
use Illuminate\Queue\InteractsWithQueue;
use Illuminate\Queue\SerializesModels;

class SetupJumpStart implements ShouldBeUnique, ShouldQueue
{
    use Dispatchable, InteractsWithQueue, Queueable, SerializesModels;

    public function __construct(public int $member_id)
    {
        //
    }

    public function handle(): void
    {
        $job_ids = [4214, 4215, 4216, 4217, 4218];

        foreach ($job_ids as $job_id) {
            $job = Job::find($job_id);

            if (! $job) {
                continue;
            }

            $_job = (new DuplicateJobAction)->execute($job);

            //

            $member_id = $this->member_id;

            if ($member_id) {
                $member = Member::find($member_id);
                if ($member) {
                    $_job->member_id = $member_id;
                    $_job->save();
                }
            }
        }
    }
}
