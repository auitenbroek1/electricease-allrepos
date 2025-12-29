<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::table('job_annotations', function (Blueprint $table) {
            $table->uuid('entity_uuid')
                ->index()
                ->after('data');

            $table->decimal('entity_quantity', 24, 6)
                ->after('entity_uuid');
        });
    }
};
