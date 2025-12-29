<?php

namespace Src\Bids\Actions;

use App\Models\Job;

class GenerateMaterialSummaryAttachmentHtmlAction
{
    public function __invoke(
        Job $job,
    ): string {
        $report = (new GenerateMaterialSummaryReportAction)(
            job: $job,
        );

        return view('emails.jobs.proposal.attachments.material-summary')
            ->with('job', $job)
            ->with('report', $report)
            ->render();
    }
}
