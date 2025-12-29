<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasMany;

class JobAssembly extends Model
{
    use HasFactory;

    public function parts(): HasMany
    {
        return $this->hasMany(JobPart::class);
    }

    public function reference(): BelongsTo
    {
        return $this->belongsTo(Assembly::class);
    }
}
