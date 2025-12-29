<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::table('members', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('jobs', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('job_customers', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('job_locations', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('job_blocks', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('job_expenses', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('job_labors', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('job_phases', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('assemblies', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('assembly_categories', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('job_assemblies', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('parts', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('part_categories', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });

        Schema::table('job_parts', function (Blueprint $table) {
            $table->boolean('is_migrated')
                ->after('uuid')
                ->default(false)
                ->index();
        });
    }
};
