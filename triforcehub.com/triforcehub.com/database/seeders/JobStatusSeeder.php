<?php

namespace Database\Seeders;

use App\Models\JobStatus;
use Illuminate\Database\Seeder;

class JobStatusSeeder extends Seeder
{
    public function run(): void
    {
        $bidding = JobStatus::factory([
            'name' => 'Bidding',
        ])->create();

        $won = JobStatus::factory([
            'name' => 'Won',
        ])->create();

        JobStatus::factory([
            'name' => 'Lost',
        ])->create();

        //

        $parents = [
            [
                'id' => $bidding->id,
                'children' => [
                    'Quotes Requested',
                    'Quotes Received',
                    'Bid Submitted',
                    'Rebid',
                ],
            ],
            [
                'id' => $won->id,
                'children' => [
                    'Value Engineering',
                    'Walkthrough',
                    'Site / Groundwork',
                    'Rough-in',
                    'Trim',
                    'Punchlist',
                    'Project Clouseout',
                    'Waiting on Rentention',
                    'Complete',
                ],
            ],
        ];

        foreach ($parents as $parent) {
            foreach ($parent['children'] as $child) {
                JobStatus::factory([
                    'parent_id' => $parent['id'],
                    'name' => $child,
                ])->create();
            }
        }
    }
}
