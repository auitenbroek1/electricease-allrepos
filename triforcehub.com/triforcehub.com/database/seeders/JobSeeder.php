<?php

namespace Database\Seeders;

use App\Models\Assembly;
use App\Models\Job;
use App\Models\JobAssembly;
use App\Models\JobCrew;
use App\Models\JobCustomer;
use App\Models\JobExpense;
use App\Models\JobLabor;
use App\Models\JobLocation;
use App\Models\JobPart;
use App\Models\JobPhase;
use App\Models\JobQuote;
use App\Models\Part;
use Illuminate\Database\Seeder;

class JobSeeder extends Seeder
{
    public function run(): void
    {
        Job::factory()
            ->count(33)
            ->create()
            ->each(function ($job) {
                $phases = JobPhase::factory([
                    'job_id' => $job->id,
                ])
                    ->count(rand(6, 12))
                    ->create()
                    ->each(function ($phase) {
                        $random_assemblies = Assembly::all()
                            ->random(rand(1, 10));

                        $assemblies = [];
                        foreach ($random_assemblies as $random_assembly) {
                            $assemblies[] = JobAssembly::factory([
                                'job_phase_id' => $phase->id,
                                'reference_id' => $random_assembly->id,
                                'quantity' => rand(1, 3),
                            ])->create();
                        }
                        $phase->assemblies()
                            ->saveMany($assemblies);

                        //

                        $assembly_parts = [];
                        foreach ($random_assemblies as $random_assembly) {
                            $job_assemblies = JobAssembly::where([
                                'job_phase_id' => $phase->id,
                                'reference_id' => $random_assembly['id'],
                            ])->get();
                            foreach ($random_assembly->parts()->get()->toArray() as $random_assembly_part) {
                                foreach ($job_assemblies as $job_assembly) {
                                    $assembly_parts[] = JobPart::factory([
                                        'job_phase_id' => $phase->id,
                                        'job_assembly_id' => $job_assembly->id,
                                        'reference_id' => $random_assembly_part['id'],
                                        'cost' => $random_assembly_part['cost'],
                                        'labor' => $random_assembly_part['labor'],
                                        'quantity' => $random_assembly_part['pivot']['quantity'],
                                    ])->create();
                                }
                            }
                        }
                        $phase->parts()
                            ->saveMany($assembly_parts);

                        //

                        $random_parts = Part::all()
                            ->random(rand(1, 10));

                        $parts = [];
                        foreach ($random_parts as $random_part) {
                            $parts[] = JobPart::factory([
                                'job_phase_id' => $phase->id,
                                'reference_id' => $random_part->id,
                                'cost' => $random_part->cost,
                                'labor' => $random_part->labor,
                                'quantity' => rand(1, 10),
                            ])->create();
                        }

                        $phase->parts()
                            ->saveMany($parts);
                    });

                $job->phases()->saveMany($phases);

                //

                $customers = JobCustomer::factory([
                    'job_id' => $job->id,
                ])->count(rand(1, 3))->create();

                $job->customers()->saveMany($customers);

                $locations = JobLocation::factory([
                    'job_id' => $job->id,
                ])->count(rand(1, 3))->create();

                $job->locations()->saveMany($locations);

                //

                $crew = JobCrew::factory([
                    'job_id' => $job->id,
                ])->count(rand(6, 12))->create();

                $job->crews()->saveMany($crew);

                $labor = JobLabor::factory([
                    'job_id' => $job->id,
                ])->count(rand(6, 12))->create();

                $job->labors()->saveMany($labor);

                $expenses = JobExpense::factory([
                    'job_id' => $job->id,
                ])->count(rand(6, 12))->create();

                $job->expenses()->saveMany($expenses);

                $quotes = JobQuote::factory([
                    'job_id' => $job->id,
                ])->count(rand(6, 12))->create();

                $job->quotes()->saveMany($quotes);
            });
    }
}
