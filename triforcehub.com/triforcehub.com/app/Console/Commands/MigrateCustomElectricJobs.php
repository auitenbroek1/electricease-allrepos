<?php

namespace App\Console\Commands;

use App\Models\Assembly;
use App\Models\AssemblyCategory;
use App\Models\Job;
use App\Models\JobAssembly;
use App\Models\JobBlock;
use App\Models\JobCustomer;
use App\Models\JobExpense;
use App\Models\JobLabor;
use App\Models\JobLocation;
use App\Models\JobPart;
use App\Models\JobPhase;
use App\Models\JobSection;
use App\Models\JobStatus;
use App\Models\JobType;
use App\Models\Member;
use App\Models\Part;
use App\Models\PartCategory;
use App\Models\State;
use App\Models\Unit;
use Illuminate\Console\Command;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Str;

class MigrateCustomElectricJobs extends Command
{
    // job_locations name cannot be empty
    // job_labors Column 'hours', 'burden', 'fringe' cannot be null
    // jobs -  Column 'name' cannot be null
    protected $signature = 'triforce:migrate-custom-electric-jobs';

    protected $description = 'Migrate Custom Electric Jobs';

    private $member = 'Custom Electric Inc.';

    private $default_job_type = 'Base Bid';

    private $default_job_status = 'Bidding';

    private $default_job_section_note = 'Notes';

    private $default_job_section_scope_of_work = 'Scope of Work';

    private $default_job_section_terms_and_condition = 'Terms and Conditions';

    private $default_job_phase_name = 'Base Bid';

    private $default_unit_name = 'Each';

    public function __construct()
    {
        parent::__construct();
        $this->db = DB::connection('aspnet_production');
    }

    public function handle(): void
    {
        $member = $this->getMember();

        echo "RESETTING RECORDS \n";
        $this->reset($member['member_id']);
        echo "RESETTING COMPLETED \n";

        $job_type = JobType::where(
            'name', 'like', '%'.$this->default_job_type.'%'
        )->first();

        $job_status = JobStatus::where(
            'name', 'like', '%'.$this->default_job_status.'%'
        )->first();

        $job_section_note = JobSection::where(
            'name', 'like', '%'.$this->default_job_section_note.'%'
        )->first();

        $job_section_scope_of_work = JobSection::where(
            'name', 'like', '%'.$this->default_job_section_scope_of_work.'%'
        )->first();

        $job_section_terms_and_condition = JobSection::where(
            'name', 'like', '%'.$this->default_job_section_terms_and_condition.'%'
        )->first();

        $unit = Unit::where(
            'name', 'like', '%'.$this->default_unit_name.'%'
        )->first();

        $source_jobs = $this->db->table('Job_Master')->select([
            'Job_ID', 'Job_Number', 'Job_Description', 'Job_Status', 'TempPole',
            'Client_Name', 'Client_Email', 'Client_Address', 'Client_Address2', 'Client_City', 'Client_State', 'Client_ZipCode', 'Client_Phone', 'Client_Mobile',
            'Work_Location', 'Work_Address', 'Work_Address2', 'Work_State', 'Work_City', 'Work_ZipCode',
            'Directions_To', 'Doing_What', 'Created_Date',
        ])
            ->whereIn('Client_ID', [$member['source_member_id']])
            ->orderBy('Created_Date', 'DESC')
            ->limit(40)
            ->get();

        foreach ($source_jobs as $source_job) {
            echo 'SOURCE JOB - '.$source_job->Job_ID.PHP_EOL;
            // Job Creation
            $job = Job::factory()->create([
                'uuid' => Str::uuid(),
                'is_migrated' => true,
                'member_id' => $member['member_id'],
                'job_status_id' => $job_status->id,
                'job_type_id' => $job_type->id,
                'name' => $source_job->Job_Description ?: 'Job Name',
                'number' => $source_job->Job_Number,
                'temporary_power' => $source_job->TempPole,
                'temporary_lighting' => 0,
                'labor_factor' => 1,
            ]);

            // Job Customer Creation
            $client_state_id = '';
            if ($source_job->Client_State) {
                $state = State::where(
                    'abbreviation', 'like', '%'.$source_job->Client_State.'%'
                )->first();
                $client_state_id = $state->id;
            }

            JobCustomer::factory()->create([
                'uuid' => Str::uuid(),
                'is_migrated' => true,
                'job_id' => $job->id,
                'name' => $source_job->Client_Name,
                'email' => $source_job->Client_Email,
                'address1' => $source_job->Client_Address,
                'address2' => $source_job->Client_Address2,
                'city' => $source_job->Client_City,
                'state_id' => $client_state_id ?: null,
                'zip' => $source_job->Client_ZipCode,
                'office' => $source_job->Client_Phone,
                'mobile' => $source_job->Client_Mobile,
            ]);

            // Job Location Creation
            $work_state_id = '';
            if ($source_job->Work_State) {
                $state = State::where(
                    'abbreviation', 'like', '%'.$source_job->Work_State.'%'
                )->first();
                $work_state_id = $state->id;
            }

            JobLocation::factory()->create([
                'uuid' => Str::uuid(),
                'is_migrated' => true,
                'job_id' => $job->id,
                'name' => $source_job->Work_Location ?: $source_job->Client_Name,
                'address1' => $source_job->Work_Address,
                'address2' => $source_job->Work_Address2,
                'city' => $source_job->Work_City,
                'state_id' => $work_state_id ?: null,
                'zip' => $source_job->Work_ZipCode,
            ]);

            // notes
            if ($source_job->Directions_To) {
                JobBlock::factory()->create([
                    'uuid' => Str::uuid(),
                    'is_migrated' => true,
                    'job_id' => $job->id,
                    'job_section_id' => $job_section_note->id,
                    'order' => 1,
                    'content' => $source_job->Directions_To,
                ]);
            }

            // scope of work
            if ($source_job->Doing_What) {
                JobBlock::factory()->create([
                    'uuid' => Str::uuid(),
                    'is_migrated' => true,
                    'job_id' => $job->id,
                    'job_section_id' => $job_section_scope_of_work->id,
                    'order' => 1,
                    'content' => $source_job->Doing_What,
                ]);
            }

            // terms and conditions
            $source_legals = $this->db->table('Job_Legal')->select([
                'Legal_ID', 'Job_ID', 'Legal_Detail', 'Job_Legal_ID', 'Client_ID',
            ])
                ->where('Client_ID', '=', $member['source_member_id'])
                ->where('Job_ID', '=', $source_job->Job_ID)->get();

            foreach ($source_legals as $source_legal) {
                JobBlock::factory()->create([
                    'uuid' => Str::uuid(),
                    'is_migrated' => true,
                    'job_id' => $job->id,
                    'job_section_id' => $job_section_terms_and_condition->id,
                    'order' => 1,
                    'content' => $source_legal->Legal_Detail,
                ]);
            }

            // Direct Expense / Vendor Quote
            $source_expenses = $this->db->table('Job_DJE_VQ_Details')->select([
                'Job_DJE_VQ_Status', 'Expense', 'Vendor_Name', 'Client_ID', 'Job_ID', 'Resale_Total',
            ])
                ->where('Client_ID', '=', $member['source_member_id'])
                ->where('Job_ID', '=', $source_job->Job_ID)->get();

            foreach ($source_expenses as $source_expense) {
                if ($source_expense->Job_DJE_VQ_Status == 'DJE') {
                    JobExpense::factory()->create([
                        'uuid' => Str::uuid(),
                        'is_migrated' => true,
                        'job_id' => $job->id,
                        'name' => $source_expense->Expense,
                        'cost' => $source_expense->Resale_Total,
                        'notes' => '',
                        'enabled' => true,
                    ]);
                } elseif ($source_expense->Job_DJE_VQ_Status == 'VQ') {
                    JobExpense::factory()->create([
                        'uuid' => Str::uuid(),
                        'is_migrated' => true,
                        'job_id' => $job->id,
                        'name' => $source_expense->Vendor_Name,
                        'cost' => $source_expense->Resale_Total,
                        'notes' => '',
                        'enabled' => true,
                    ]);
                }
            }

            // labors
            $source_labors = $this->db->table('Job_Labor')->select([
                'Laborer_Name', 'Lobor_Resale', 'Burden',
            ])
                ->where('Client_ID', '=', $member['source_member_id'])
                ->where('Job_ID', '=', $source_job->Job_ID)->get();

            foreach ($source_labors as $source_labor) {
                JobLabor::factory()->create([
                    'uuid' => Str::uuid(),
                    'is_migrated' => true,
                    'job_id' => $job->id,
                    'name' => $source_labor->Laborer_Name,
                    'hours' => 0,
                    'rate' => $source_labor->Lobor_Resale,
                    'burden' => $source_labor->Burden ?: 1,
                    'fringe' => 1,
                    'notes' => null,
                    'enabled' => true,
                ]);
            }

            $job_phase = JobPhase::factory()->create([
                'uuid' => Str::uuid(),
                'is_migrated' => true,
                'job_id' => $job->id,
                'name' => $this->default_job_phase_name,
            ]);

            // parts
            $source_parts = $this->db->table('Job_Parts')
                ->distinct('Job_Parts.Part_Id')
                ->join('Parts_Details', 'Job_Parts.Part_Number', '=', 'Parts_Details.Part_Number')
                ->select([
                    'Job_Parts.Part_Number', 'Job_Parts.Part_Cost', 'Job_Parts.LaborUnit', 'Job_Parts.Estimated_Qty',
                    'Parts_Details.Description', 'Parts_Details.Part_Category',
                ])
                ->where('Job_Parts.Client_ID', '=', $member['source_member_id'])
                ->where('Job_Parts.Job_ID', '=', $source_job->Job_ID)
                ->where('Parts_Details.Client_ID', '=', $member['source_member_id'])
                ->get();

            foreach ($source_parts as $source_part) {
                $part = Part::where([
                    'name' => $source_part->Description,
                    'member_id' => $member['member_id'],
                ])->first();

                if (! $part) {
                    $part = Part::factory()->create([
                        'uuid' => Str::uuid(),
                        'is_migrated' => true,
                        'member_id' => $member['member_id'],
                        'name' => $source_part->Description,
                        'unit_id' => $unit->id,
                        'cost' => $source_part->Part_Cost,
                        'labor' => $source_part->LaborUnit,
                    ]);
                }

                $part_category = PartCategory::where([
                    'name' => $source_part->Part_Category,
                    'member_id' => $member['member_id'],
                ])->first();

                if (! $part_category) {
                    $part_category = PartCategory::factory()->create([
                        'uuid' => Str::uuid(),
                        'is_migrated' => true,
                        'member_id' => $member['member_id'],
                        'name' => $source_part->Part_Category,
                    ]);
                }

                $part
                    ->categories()
                    ->syncWithoutDetaching($part_category);

                JobPart::factory()->create([
                    'uuid' => Str::uuid(),
                    'is_migrated' => true,
                    'job_phase_id' => $job_phase->id,
                    'reference_id' => $part->id,
                    'labor' => $source_part->LaborUnit,
                    'quantity' => $source_part->Estimated_Qty,
                    'cost' => $source_part->Part_Cost,
                ]);
            }

            // assemblies part
            $source_assembly_parts = $this->db->table('Job_Assembly_Parts')
                ->distinct('Job_Assembly_Parts.AssembliesPart_ID')
                ->join('Job_AssembliesDetails', 'Job_Assembly_Parts.JobAssembly_Id', '=', 'Job_AssembliesDetails.JobAssembly_Id')
                ->join('Parts_Details', 'Job_Assembly_Parts.Part_Number', '=', 'Parts_Details.Part_Number')
                ->select([
                    'Job_Assembly_Parts.Part_Number', 'Job_Assembly_Parts.Part_Cost', 'Job_Assembly_Parts.LaborUnit', 'Job_Assembly_Parts.Estimated_Qty',
                    'Job_Assembly_Parts.Assemblies_Name', 'Job_Assembly_Parts.Assemblies_Category',
                    'Job_AssembliesDetails.Multiplier', 'Job_AssembliesDetails.Est_Qty',
                    'Parts_Details.Description', 'Parts_Details.Part_Category',
                ])
                ->where('Job_Assembly_Parts.Job_ID', '=', $source_job->Job_ID)
                ->where('Job_Assembly_Parts.Client_ID', '=', $member['source_member_id'])
                ->where('Job_AssembliesDetails.Client_ID', '=', $member['source_member_id'])
                ->where('Parts_Details.Client_ID', '=', $member['source_member_id'])
                ->get();

            foreach ($source_assembly_parts as $source_assembly_part) {
                $assembly = Assembly::where([
                    'name' => $source_assembly_part->Assemblies_Name,
                    'member_id' => $member['member_id'],
                ])->first();

                if (! $assembly) {
                    $assembly = Assembly::factory()->create([
                        'uuid' => Str::uuid(),
                        'is_migrated' => true,
                        'member_id' => $member['member_id'],
                        'name' => $source_assembly_part->Assemblies_Name,
                    ]);
                }

                $assembly_category = AssemblyCategory::where([
                    'name' => $source_assembly_part->Assemblies_Category,
                    'member_id' => $member['member_id'],
                ])->first();

                if (! $assembly_category) {
                    $assembly_category = AssemblyCategory::factory()->create([
                        'uuid' => Str::uuid(),
                        'is_migrated' => true,
                        'member_id' => $member['member_id'],
                        'name' => $source_assembly_part->Assemblies_Category,
                    ]);
                }

                $assembly
                    ->categories()
                    ->syncWithoutDetaching($assembly_category);

                $job_assembly = JobAssembly::where([
                    'reference_id' => $assembly->id,
                    'is_migrated' => true,
                    'job_phase_id' => $job_phase->id,
                    'labor_factor' => $source_assembly_part->Multiplier,
                    'quantity' => $source_assembly_part->Est_Qty,
                ])->first();

                if (! $job_assembly) {
                    $job_assembly = JobAssembly::factory()->create([
                        'uuid' => Str::uuid(),
                        'is_migrated' => true,
                        'job_phase_id' => $job_phase->id,
                        'reference_id' => $assembly->id,
                        'labor_factor' => $source_assembly_part->Multiplier,
                        'quantity' => $source_assembly_part->Est_Qty,
                    ]);
                }

                $part = Part::where([
                    'name' => $source_assembly_part->Description,
                    'member_id' => $member['member_id'],
                ])->first();

                if (! $part) {
                    $part = Part::factory()->create([
                        'uuid' => Str::uuid(),
                        'is_migrated' => true,
                        'member_id' => $member['member_id'],
                        'name' => $source_assembly_part->Description,
                        'unit_id' => $unit->id,
                        'cost' => $source_assembly_part->Part_Cost,
                        'labor' => $source_assembly_part->LaborUnit,
                    ]);
                }

                $part_category = PartCategory::where([
                    'name' => $source_assembly_part->Part_Category,
                    'member_id' => $member['member_id'],
                ])->first();

                if (! $part_category) {
                    $part_category = PartCategory::factory()->create([
                        'uuid' => Str::uuid(),
                        'is_migrated' => true,
                        'member_id' => $member['member_id'],
                        'name' => $source_assembly_part->Part_Category,
                    ]);
                }

                $part
                    ->categories()
                    ->syncWithoutDetaching($part_category);

                JobPart::factory()->create([
                    'uuid' => Str::uuid(),
                    'is_migrated' => true,
                    'job_phase_id' => $job_phase->id,
                    'job_assembly_id' => $job_assembly->id,
                    'reference_id' => $part->id,
                    'labor' => $source_assembly_part->LaborUnit,
                    'quantity' => $source_assembly_part->Estimated_Qty,
                    'cost' => $source_assembly_part->Part_Cost,
                ]);
            }
        }
    }

    private function reset($member_id)
    {
        $jobs = Job::where([
            'is_migrated' => true,
            'member_id' => $member_id,
        ])->get();

        foreach ($jobs as $job) {
            $job_customers = JobCustomer::where([
                'is_migrated' => true,
                'job_id' => $job->id,
            ])->get();
            foreach ($job_customers as $job_customer) {
                $job_customer->delete();
            }

            $job_locations = JobLocation::where([
                'is_migrated' => true,
                'job_id' => $job->id,
            ])->get();
            foreach ($job_locations as $job_location) {
                $job_location->delete();
            }

            $job_blocks = JobBlock::where([
                'is_migrated' => true,
                'job_id' => $job->id,
            ])->get();
            foreach ($job_blocks as $job_block) {
                $job_block->delete();
            }

            $job_expenses = JobExpense::where([
                'is_migrated' => true,
                'job_id' => $job->id,
            ])->get();
            foreach ($job_expenses as $job_expense) {
                $job_expense->delete();
            }

            $job_labors = JobLabor::where([
                'is_migrated' => true,
                'job_id' => $job->id,
            ])->get();
            foreach ($job_labors as $job_labor) {
                $job_labor->delete();
            }

            $job_phases = JobPhase::where([
                'is_migrated' => true,
                'job_id' => $job->id,
            ])->get();

            foreach ($job_phases as $job_phase) {
                $job_parts = JobPart::where([
                    'is_migrated' => true,
                    'job_phase_id' => $job_phase->id,
                ])->get();
                foreach ($job_parts as $job_part) {
                    $job_part->delete();
                }

                $job_assemblies = JobAssembly::where([
                    'is_migrated' => true,
                    'job_phase_id' => $job_phase->id,
                ])->get();
                foreach ($job_assemblies as $job_assembly) {
                    $job_assembly->delete();
                }

                $job_phase->delete();
            }

            $job->delete();
        }

        $assemblies = Assembly::where([
            'is_migrated' => true,
            'member_id' => $member_id,
        ])->get();
        foreach ($assemblies as $assembly) {
            $assembly->delete();
        }

        $assembly_categories = AssemblyCategory::where([
            'is_migrated' => true,
            'member_id' => $member_id,
        ])->get();
        foreach ($assembly_categories as $assembly_category) {
            $assembly_category->delete();
        }

        $parts = Part::where([
            'is_migrated' => true,
            'member_id' => $member_id,
        ])->get();
        foreach ($parts as $part) {
            $part->delete();
        }

        $part_categories = PartCategory::where([
            'is_migrated' => true,
            'member_id' => $member_id,
        ])->get();
        foreach ($part_categories as $part_categorie) {
            $part_categorie->delete();
        }
    }

    private function getMember()
    {
        $source_members = $this->db->table('Client_Master')->select(['Client_ID', 'Client_Company', 'Email', 'Address', 'State', 'City', 'ZipCode', 'Phone'])
            ->whereIn('Client_Company', [$this->member])
            ->get();

        $source_member = reset($source_members);

        if (! $source_member) {
            exit('Member not found in source');
        }

        $source_member_id = $source_member[0]->Client_ID;

        $member = Member::where(
            'name', 'like', '%'.$this->member.'%'
        )->first();

        if (! $member) {
            $state = State::where(
                'abbreviation', 'like', '%'.$source_member[0]->State.'%'
            )->first();

            $member = Member::factory()->create([
                'uuid' => Str::uuid(),
                'name' => $source_member[0]->Client_Company,
                'email' => $source_member[0]->Email,
                'address1' => $source_member[0]->Address,
                'address2' => '',
                'city' => $source_member[0]->City,
                'state_id' => $state->id,
                'zip' => $source_member[0]->ZipCode,
                'office' => $source_member[0]->Phone,
                'mobile' => $source_member[0]->Phone,
                'is_migrated' => true,
            ]);
        }

        return [
            'source_member_id' => $source_member_id,
            'member_id' => $member->id,
        ];
    }
}
