<?php

namespace Src\Bids\Mail;

use App\Models\Job;
use App\Models\JobCustomer;
use Illuminate\Bus\Queueable;
use Illuminate\Contracts\Queue\ShouldQueue;
use Illuminate\Mail\Mailable;
use Illuminate\Queue\SerializesModels;
use Src\Bids\Actions\GenerateMaterialSummaryAttachmentAction;
use Src\Bids\Actions\GenerateProposalAttachmentAction;

class SendProposal extends Mailable implements ShouldQueue
{
    use Queueable, SerializesModels;

    public $job;

    public $customer;

    private $_attachments;

    public function __construct(
        Job $job,
        JobCustomer $customer,
        string $subject,
        array $attachments,
    ) {
        $this->job = $job;
        $this->customer = $customer;

        $this->subject($subject);

        $this->_attachments = $attachments;
    }

    public function build()
    {
        $this->from(config('mail.from.address'), $this->job->member->name);

        $this->replyTo($this->job->member);

        if (in_array('proposal', $this->_attachments)) {
            $this->attachData(...(new GenerateProposalAttachmentAction)(
                job: $this->job,
                customer: $this->customer,
            ));
        }

        if (in_array('material-summary', $this->_attachments)) {
            $this->attachData(...(new GenerateMaterialSummaryAttachmentAction)(
                job: $this->job,
            ));
        }

        return $this->markdown('emails.jobs.proposal.markdown');
    }
}
