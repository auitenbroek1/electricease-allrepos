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
use App\Models\Member;
use App\Models\Part;
use App\Models\PartCategory;
use App\Models\State;
use App\Models\Unit;
use Illuminate\Console\Command;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Str;

class MigrateLiveWireElectric extends Command
{
    // job_locations name cannot be empty
    // job_labors Column 'hours', 'burden', 'fringe' cannot be null
    // jobs -  Column 'name' cannot be null
    // jobs - Column 'number' cannot be null
    protected $signature = 'triforce:migrate-live-wire-electric';

    protected $description = 'Migrate Live Wire Electric';

    private $member = 'Live Wire Electrical Contractors';

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

        $unit = Unit::where(
            'name', 'like', '%'.$this->default_unit_name.'%'
        )->first();

        // Moving predefined Assemblies & Its Materials
        echo "Migrating All Assemblies & Its Materials \n";

        $source_assembly_parts = $this->db->table('Assemblies_Parts')
            ->distinct('Assemblies_Parts.Part_ID')
            ->join('Parts_Details', 'Assemblies_Parts.Part_Number', '=', 'Parts_Details.Part_Number')
            ->select([
                'Assemblies_Parts.Part_ID',
                'Assemblies_Parts.Part_Number', 'Assemblies_Parts.Part_Cost', 'Assemblies_Parts.LaborUnit', 'Assemblies_Parts.Estimated_Qty',
                'Assemblies_Parts.Assemblies_Name', 'Assemblies_Parts.Assemblies_Category',
                'Parts_Details.Description', 'Parts_Details.Part_Category',
            ])
            ->where('Assemblies_Parts.Client_ID', '=', $member['source_member_id'])
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

            $assembly->categories()->syncWithoutDetaching($assembly_category);

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

            $part->categories()->syncWithoutDetaching($part_category);

            $isExists = $assembly->parts()->where('parts.id', $part->id)->exists();

            if ($isExists) {
                $assembly->parts()->updateExistingPivot($part, ['quantity' => $source_assembly_part->Estimated_Qty]);
            } else {
                $assembly->parts()->attach($part, ['quantity' => $source_assembly_part->Estimated_Qty]);
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
