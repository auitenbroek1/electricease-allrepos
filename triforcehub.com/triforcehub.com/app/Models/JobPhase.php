<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasMany;

class JobPhase extends Model
{
    use HasFactory;

    public function assemblies(): HasMany
    {
        return $this->hasMany(JobAssembly::class);
    }

    public function parts(): HasMany
    {
        return $this->hasMany(JobPart::class)->where('job_assembly_id', null);
    }

    public function job(): BelongsTo
    {
        return $this->belongsTo(Job::class);
    }
}
