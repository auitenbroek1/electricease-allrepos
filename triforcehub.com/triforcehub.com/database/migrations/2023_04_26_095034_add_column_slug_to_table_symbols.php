<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::table('symbols', function (Blueprint $table) {
            $table->string('slug', 100)
                ->after('name')
                ->nullable()
                ->index();
        });
    }
};
