<?php

namespace Src\Bids\Actions;

use App\Models\Job;
use App\Models\JobCustomer;
use Wnx\SidecarBrowsershot\BrowsershotLambda;

class GenerateWorkOrderAttachmentAction
{
    public function __invoke(
        Job $job,
        JobCustomer $customer,
    ): array {
        $html = (new GenerateWorkOrderAttachmentHtmlAction)(
            job: $job,
            customer: $customer,
        );

        $data = BrowsershotLambda::html($html)->pdf();

        return [
            'data' => $data,
            'name' => 'work-order.pdf',
            'options' => [
                'mime' => 'application/pdf',
            ],
        ];
    }
}
