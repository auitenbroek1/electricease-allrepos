<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasMany;

class JobAnnotation extends Model
{
    use HasFactory;

    public function annotations(): HasMany
    {
        return $this->hasMany(JobAnnotation::class);
    }

    public function plan(): BelongsTo
    {
        return $this->belongsTo(JobPlan::class);
    }
}
