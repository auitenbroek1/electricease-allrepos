<?php

namespace Src\Bids\Controllers;

use App\Http\Controllers\Controller;
use App\Models\Job;
use App\Models\JobCustomer;
use Illuminate\Http\Request;
use Src\Bids\Actions\SendProposalEmailAction;

class SendProposalEmailController extends Controller
{
    public function __invoke(Job $job, JobCustomer $customer, Request $request)
    {
        $to = $request->input('to');
        $cc = $request->input('cc');
        $subject = $request->input('subject');
        $attachments = $request->input('attachments');

        (new SendProposalEmailAction)(
            job: $job,
            customer: $customer,
            to: explode(',', $to),
            cc: $cc ? explode(',', $cc) : null,
            subject: $subject,
            attachments: $attachments,
        );

        $job->proposal_sent_at = now();
        $job->save();
    }
}
