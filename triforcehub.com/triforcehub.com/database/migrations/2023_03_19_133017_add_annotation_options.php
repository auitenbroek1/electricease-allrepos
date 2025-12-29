<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::table('job_assemblies', function (Blueprint $table) {
            $table->string('annotation_type', 1024)
                ->nullable()
                ->after('labor_factor');

            $table->unsignedBigInteger('annotation_symbol_id')
                ->nullable()
                ->after('annotation_type');

            $table->foreign('annotation_symbol_id')->references('id')->on('symbols');
        });

        Schema::table('job_parts', function (Blueprint $table) {
            $table->string('annotation_type', 1024)
                ->nullable()
                ->after('labor_factor');

            $table->unsignedBigInteger('annotation_symbol_id')
                ->nullable()
                ->after('annotation_type');

            $table->foreign('annotation_symbol_id')->references('id')->on('symbols');
        });
    }
};
