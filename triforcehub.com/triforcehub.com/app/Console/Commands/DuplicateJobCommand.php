<?php

namespace App\Console\Commands;

use App\Actions\Jobs\DuplicateJobAction;
use App\Models\Job;
use App\Models\Member;
use Illuminate\Console\Command;

class DuplicateJobCommand extends Command
{
    protected $signature = 'triforce:duplicate-job
        {job_id}
        {member_id=null}';

    protected $description = 'Duplicate a job';

    public function handle(): int
    {
        $job_id = $this->argument('job_id');
        $job = Job::find($job_id);

        if (! $job) {
            $this->error('does not exist');

            return Command::FAILURE;
        }

        $this->line('found job_id: '.$job->id);
        $_job = (new DuplicateJobAction)->execute($job);
        $this->line('duplicated job_id: '.$_job->id);

        //

        $member_id = $this->argument('member_id');

        if ($member_id) {
            $member = Member::find($member_id);
            if ($member) {
                $this->line('set member_id to: '.$member_id);
                $_job->member_id = $member_id;
                $_job->save();
            }
        }

        return Command::SUCCESS;
    }
}
