<?php

namespace Database\Seeders;

use App\Models\Distributor;
use App\Models\Member;
use Illuminate\Database\Seeder;

class DistributorSeeder extends Seeder
{
    public function run(): void
    {
        $member = Member::where(['principal' => true])->first();

        Distributor::factory([
            'name' => 'Echo',
        ])->create()->members()->attach([[
            'member_id' => $member->id,
            'username' => '82833',
            'password' => 'ECHO82833',
            'customer' => '82833',  // Armor Electric
            'enabled' => true,
        ]]);

        Distributor::factory([
            'name' => 'Van Meter',
        ])->create()->members()->attach([[
            'member_id' => $member->id,
            'username' => 'ELEEASE',
            'password' => 'Y@gat1yAgts',
            'customer' => '89953', // Armor Electric
            'enabled' => true,
        ]]);
    }
}
