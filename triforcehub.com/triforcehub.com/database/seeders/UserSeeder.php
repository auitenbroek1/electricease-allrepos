<?php

namespace Database\Seeders;

use App\Models\Member;
use App\Models\User;
use Illuminate\Database\Seeder;

class UserSeeder extends Seeder
{
    public function run(): void
    {
        $principal = Member::where(['principal' => true])->first();

        User::factory([
            'member_id' => $principal->id,
            'email' => 'bruno@electric-ease.com',
            'name' => 'Bruno Zugay',
        ])->principal()->create();

        User::factory([
            'member_id' => $principal->id,
            'email' => 'kevin@electric-ease.com',
            'name' => 'Kevin Bucklew',
        ])->create();

        User::factory([
            'member_id' => $principal->id,
            'email' => 'jake@electric-ease.com',
            'name' => 'Jake Biwer',
        ])->create();

        User::factory([
            'member_id' => $principal->id,
            'email' => 'anthony@electric-ease.com',
            'name' => 'Anthony Chiovaro',
        ])->create();

        User::factory([
            'member_id' => $principal->id,
            'email' => 'matt@electric-ease.com',
            'name' => 'Matt Wilcox',
        ])->create();

        //

        $other = Member::where(['principal' => false])->first();

        User::factory([
            'member_id' => $other->id,
            'email' => 'demo+principal@electric-ease.com',
            'name' => 'Demo Principal',
        ])->principal()->create();

        User::factory([
            'member_id' => $other->id,
            'email' => 'demo+user@electric-ease.com',
            'name' => 'Demo User',
        ])->create();
    }
}
