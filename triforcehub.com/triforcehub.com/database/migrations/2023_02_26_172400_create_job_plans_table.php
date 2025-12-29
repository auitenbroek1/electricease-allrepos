<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::create('job_plans', function (Blueprint $table) {
            $table->id();
            $table->uuid('uuid');

            $table->unsignedBigInteger('job_id');
            $table->unsignedBigInteger('upload_id');

            $table->timestamps();

            $table->foreign('job_id')->references('id')->on('jobs');
            $table->foreign('upload_id')->references('id')->on('uploads');
        });
    }
};
