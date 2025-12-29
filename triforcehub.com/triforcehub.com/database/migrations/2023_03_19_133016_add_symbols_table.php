<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::create('symbols', function (Blueprint $table) {
            $table->id();
            $table->uuid('uuid');

            $table->string('name', 1024);
            $table->longText('data')->nullable();
            $table->string('asset', 1024)->nullable();
            $table->integer('position');

            $table->timestamps();
        });
    }
};
