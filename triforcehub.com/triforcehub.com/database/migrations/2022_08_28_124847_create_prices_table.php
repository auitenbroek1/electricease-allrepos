<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::create('prices', function (Blueprint $table) {
            $table->id();
            $table->uuid('uuid');

            $table->unsignedBigInteger('member_id');
            $table->unsignedBigInteger('distributor_id');
            $table->unsignedBigInteger('part_upc_id');

            $table->decimal('cost', 24, 6);

            $table->timestamps();

            $table->foreign('member_id')->references('id')->on('members');
            $table->foreign('distributor_id')->references('id')->on('distributors');
            $table->foreign('part_upc_id')->references('id')->on('part_upcs');
        });
    }
};
