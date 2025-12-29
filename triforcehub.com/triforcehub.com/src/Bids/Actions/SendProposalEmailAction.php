<?php

namespace Src\Bids\Actions;

use App\Models\Job;
use App\Models\JobCustomer;
use Illuminate\Support\Facades\Mail;
use Src\Bids\Mail\SendProposal;

class SendProposalEmailAction
{
    public function __invoke(
        Job $job,
        JobCustomer $customer,
        ?array $to = null,
        ?array $cc = null,
        ?string $subject = null,
        ?array $attachments = null,
    ): void {
        $to ??= [$customer->email];
        $subject ??= 'Bid Proposal';

        Mail::to($to)
            ->cc($cc)
            ->send(new SendProposal(
                job: $job,
                customer: $customer,
                subject: $subject,
                attachments: $attachments,
            ));
    }
}
