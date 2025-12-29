<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::table('members', function (Blueprint $table) {
            $table->boolean('enabled')
                ->after('mobile')
                ->default(true)
                ->index();
        });

        Schema::table('users', function (Blueprint $table) {
            $table->boolean('enabled')
                ->after('remember_token')
                ->default(true)
                ->index();
        });
    }
};
