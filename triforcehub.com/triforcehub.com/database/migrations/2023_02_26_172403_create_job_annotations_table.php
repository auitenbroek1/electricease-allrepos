<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::create('job_annotations', function (Blueprint $table) {
            $table->id();
            $table->uuid('uuid');

            $table->unsignedBigInteger('job_plan_id');
            $table->longText('data');

            $table->timestamps();

            $table->foreign('job_plan_id')->references('id')->on('job_plans');
        });
    }
};
