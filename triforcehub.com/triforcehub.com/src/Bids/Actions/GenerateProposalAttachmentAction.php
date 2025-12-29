<?php

namespace Src\Bids\Actions;

use App\Models\Job;
use App\Models\JobCustomer;
use Wnx\SidecarBrowsershot\BrowsershotLambda;

class GenerateProposalAttachmentAction
{
    public function __invoke(
        Job $job,
        JobCustomer $customer,
    ): array {
        $html = (new GenerateProposalAttachmentHtmlAction)(
            job: $job,
            customer: $customer,
        );

        $data = BrowsershotLambda::html($html)->pdf();

        return [
            'data' => $data,
            'name' => 'proposal.pdf',
            'options' => [
                'mime' => 'application/pdf',
            ],
        ];
    }
}
