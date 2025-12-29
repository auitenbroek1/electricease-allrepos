<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::table('job_assemblies', function (Blueprint $table) {
            $table->boolean('enabled')
                ->after('labor_factor')
                ->default(true)
                ->index();
        });

        Schema::table('job_parts', function (Blueprint $table) {
            $table->boolean('enabled')
                ->after('labor_factor')
                ->default(true)
                ->index();
        });
    }
};
