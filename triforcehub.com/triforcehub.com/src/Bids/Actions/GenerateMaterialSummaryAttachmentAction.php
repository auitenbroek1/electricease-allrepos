<?php

namespace Src\Bids\Actions;

use App\Models\Job;
use Wnx\SidecarBrowsershot\BrowsershotLambda;

class GenerateMaterialSummaryAttachmentAction
{
    public function __invoke(
        Job $job,
    ): array {
        $html = (new GenerateMaterialSummaryAttachmentHtmlAction)(
            job: $job,
        );

        $data = BrowsershotLambda::html($html)->pdf();

        return [
            'data' => $data,
            'name' => 'material-summary.pdf',
            'options' => [
                'mime' => 'application/pdf',
            ],
        ];
    }
}
