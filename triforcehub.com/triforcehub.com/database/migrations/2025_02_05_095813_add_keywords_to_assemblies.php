<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::table('assemblies', function (Blueprint $table) {
            $table->after('description', function (Blueprint $table) {
                $table->text('keywords')->nullable();
            });
        });
    }
};
