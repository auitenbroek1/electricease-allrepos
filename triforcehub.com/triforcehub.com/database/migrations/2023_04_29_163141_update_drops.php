<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::table('job_plans', function (Blueprint $table) {
            $table->dropForeign(['job_id']);
            $table->foreign('job_id')->references('id')->on('jobs')->onDelete('cascade');

            $table->dropForeign(['upload_id']);
            $table->foreign('upload_id')->references('id')->on('uploads')->onDelete('cascade');
        });
    }
};
