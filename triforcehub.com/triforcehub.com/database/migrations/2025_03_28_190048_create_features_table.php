<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Schema;

return new class extends Migration
{
    public function up(): void
    {
        Schema::create('features', function (Blueprint $table) {
            $table->id();
            $table->string('name')->unique();
            $table->string('slug')->unique();
            $table->string('description')->nullable();
            $table->boolean('enabled')->default(false);
            $table->timestamps();
        });

        Schema::create('feature_member', function (Blueprint $table) {
            $table->id();
            $table->foreignId('member_id')->constrained()->onDelete('cascade');
            $table->foreignId('feature_id')->constrained()->onDelete('cascade');
            $table->timestamps();
        });

        DB::table('features')->insert([
            [
                'name' => 'QuickSearch AI',
                'description' => 'QuickSearch AI uses smart, natural language search to help you locate materials, assemblies, specs, and more — all without digging through endless lists.',
                'slug' => 'semantic-search',
                'enabled' => true,
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'Member Portal',
                'description' => 'Member portal for managing your own pricing.',
                'slug' => 'member-portal',
                'enabled' => false,
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'Purchase Orders',
                'description' => 'Send purchase orders directly to your distributor.',
                'slug' => 'purchase-orders',
                'enabled' => false,
                'created_at' => now(),
                'updated_at' => now(),
            ],
            [
                'name' => 'Quantum Count',
                'description' => 'The next evolution of auto count.',
                'slug' => 'quantum-count',
                'enabled' => false,
                'created_at' => now(),
                'updated_at' => now(),
            ],
        ]);
    }
};
