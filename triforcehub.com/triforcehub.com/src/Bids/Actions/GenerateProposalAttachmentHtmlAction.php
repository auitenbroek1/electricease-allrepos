<?php

namespace Src\Bids\Actions;

use App\Actions\Jobs\BuildJobResponse;
use App\Models\Job;
use App\Models\JobCustomer;

class GenerateProposalAttachmentHtmlAction
{
    public function __invoke(
        Job $job,
        JobCustomer $customer,
    ): string {
        $job = (new BuildJobResponse)->execute($job);

        $locations = $job['parent'] ? $job['parent']['locations'] : $job['locations'];

        return view('emails.jobs.proposal.attachments.proposal')
            ->with('job', $job)
            ->with('customer', $customer)
            ->with('locations', $locations)
            ->render();
    }
}
