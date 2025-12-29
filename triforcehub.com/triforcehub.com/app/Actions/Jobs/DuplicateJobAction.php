<?php

namespace App\Actions\Jobs;

use Illuminate\Support\Facades\DB;
use Illuminate\Support\Str;

class DuplicateJobAction
{
    public function execute(
        $job,
    ) {
        return DB::transaction(function () use ($job) {
            $_job = $job->replicate();
            $_job->uuid = Str::uuid();
            $_job->name = $_job->name.' (COPY)';
            $_job->save();

            foreach ($job->customers as $customer) {
                $_customer = $customer->replicate();
                $_customer->uuid = Str::uuid();
                $_customer->job_id = $_job->id;
                $_customer->save();
            }

            foreach ($job->locations as $location) {
                $_location = $location->replicate();
                $_location->uuid = Str::uuid();
                $_location->job_id = $_job->id;
                $_location->save();
            }

            foreach ($job->phases as $phase) {
                $_phase = $phase->replicate();
                $_phase->uuid = Str::uuid();
                $_phase->job_id = $_job->id;
                $_phase->save();

                foreach ($phase->assemblies as $assembly) {
                    $_assembly = $assembly->replicate();
                    $_assembly->uuid = Str::uuid();
                    $_assembly->job_phase_id = $_phase->id;
                    $_assembly->save();

                    foreach ($assembly->parts as $part) {
                        $_part = $part->replicate();
                        $_part->uuid = Str::uuid();
                        $_part->job_phase_id = $_phase->id;
                        $_part->job_assembly_id = $_assembly->id;
                        $_part->save();
                    }
                }

                foreach ($phase->parts as $part) {
                    $_part = $part->replicate();
                    $_part->uuid = Str::uuid();
                    $_part->job_phase_id = $_phase->id;
                    $_part->save();
                }
            }

            foreach ($job->labors as $labor) {
                $_labors = $labor->replicate();
                $_labors->uuid = Str::uuid();
                $_labors->job_id = $_job->id;
                $_labors->save();
            }

            foreach ($job->expenses as $expense) {
                $_expense = $expense->replicate();
                $_expense->uuid = Str::uuid();
                $_expense->job_id = $_job->id;
                $_expense->save();
            }

            foreach ($job->quotes as $quote) {
                $_quotes = $quote->replicate();
                $_quotes->uuid = Str::uuid();
                $_quotes->job_id = $_job->id;
                $_quotes->save();
            }

            foreach ($job->crews as $crew) {
                $_crews = $crew->replicate();
                $_crews->uuid = Str::uuid();
                $_crews->job_id = $_job->id;
                $_crews->save();
            }

            foreach ($job->adjustments as $adjustment) {
                $_adjustments = $adjustment->replicate();
                $_adjustments->uuid = Str::uuid();
                $_adjustments->job_id = $_job->id;
                $_adjustments->save();
            }

            foreach ($job->blocks as $block) {
                $_blocks = $block->replicate();
                $_blocks->uuid = Str::uuid();
                $_blocks->job_id = $_job->id;
                $_blocks->save();
            }

            return $_job;
        });
    }
}
