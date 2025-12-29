<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::table('job_annotations', function (Blueprint $table) {
            $table->dropForeign(['job_plan_id']);
            $table->foreign('job_plan_id')->references('id')->on('job_plans')->onDelete('cascade');
        });
    }
};
