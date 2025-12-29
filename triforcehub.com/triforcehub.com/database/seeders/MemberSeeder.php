<?php

namespace Database\Seeders;

use App\Models\Member;
use App\Models\State;
use Illuminate\Database\Seeder;

class MemberSeeder extends Seeder
{
    public function run(): void
    {
        $state = State::where(['abbreviation' => 'IA'])->first();

        Member::factory([
            'name' => 'Electric Ease',
            'email' => 'admin@electric-ease.com',
            'address1' => '101 NE Circle Dr',
            'address2' => '',
            'city' => 'Grimes',
            'state_id' => $state->id,
            'zip' => '50111',
            'mobile' => '',
            'office' => '8334443273',
        ])->principal()->create();

        Member::factory([
            'name' => 'Demo',
            'email' => 'demo@electric-ease.com',
            'address1' => '101 NE Circle Dr',
            'address2' => '',
            'city' => 'Grimes',
            'state_id' => $state->id,
            'zip' => '50111',
            'mobile' => '',
            'office' => '8334443273',
        ])->create();
    }
}
