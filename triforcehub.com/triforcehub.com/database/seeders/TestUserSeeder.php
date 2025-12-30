<?php

namespace Database\Seeders;

use App\Models\Member;
use App\Models\User;
use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Str;

class TestUserSeeder extends Seeder
{
    public function run(): void
    {
        // Create a test state directly via DB
        $state = DB::table('states')->where('abbreviation', 'IA')->first();
        if (! $state) {
            $stateId = DB::table('states')->insertGetId([
                'uuid' => Str::uuid(),
                'name' => 'Iowa',
                'abbreviation' => 'IA',
                'created_at' => now(),
                'updated_at' => now(),
            ]);
        } else {
            $stateId = $state->id;
        }

        // Create a test member
        $member = Member::where('email', 'test@test.com')->first();
        if (! $member) {
            $member = new Member();
            $member->name = 'Test Company';
            $member->email = 'test@test.com';
            $member->address1 = '123 Test Street';
            $member->address2 = '';
            $member->city = 'Des Moines';
            $member->state_id = $stateId;
            $member->zip = '50309';
            $member->office = '5551234567';
            $member->mobile = '';
            $member->principal = true;
            $member->save();
        }

        // Create the test user
        $user = User::where('email', 'test@test.com')->first();
        if (! $user) {
            $user = new User();
            $user->member_id = $member->id;
            $user->name = 'Test User';
            $user->email = 'test@test.com';
            $user->password = Hash::make('password');
            $user->principal = true;
            $user->save();
        }

        $this->command->info('Test user created: test@test.com / password');
    }
}
