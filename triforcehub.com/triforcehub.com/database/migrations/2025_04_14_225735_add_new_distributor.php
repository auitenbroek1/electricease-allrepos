<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Str;

return new class extends Migration
{
    public function up(): void
    {
        DB::insert('
            INSERT INTO distributors (
                uuid,
                name,
                created_at,
                updated_at
            ) VALUES (
                :uuid,
                :name,
                :created_at,
                :updated_at
            )
        ', [
            'uuid' => (string) Str::uuid(),
            'name' => '3E',
            'created_at' => now(),
            'updated_at' => now(),
        ]);

        foreach ([1, 322, 357] as $member_id) {
            DB::insert('
                INSERT INTO distributor_member (
                    distributor_id,
                    member_id,
                    username,
                    password,
                    customer,
                    enabled,
                    created_at,
                    updated_at
                ) VALUES (
                    :distributor_id,
                    :member_id,
                    :username,
                    :password,
                    :customer,
                    :enabled,
                    :created_at,
                    :updated_at
                )
            ', [
                'distributor_id' => 3,
                'member_id' => $member_id,
                'username' => '',
                'password' => '',
                'customer' => '',
                'enabled' => true,
                'created_at' => now(),
                'updated_at' => now(),
            ]);
        }
    }
};
