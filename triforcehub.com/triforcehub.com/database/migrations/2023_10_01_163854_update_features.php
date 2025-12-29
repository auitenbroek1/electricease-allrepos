<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::table('members', function (Blueprint $table) {
            $table->after('feature_digital_takeoff_enabled', function ($table) {
                $table->boolean('feature_linear_with_drops_enabled')
                    ->default(false)
                    ->nullable();

                $table->boolean('feature_auto_count_enabled')
                    ->default(false)
                    ->nullable();
            });
        });
    }
};
