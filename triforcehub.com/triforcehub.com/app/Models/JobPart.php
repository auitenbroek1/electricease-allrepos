<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;

class JobPart extends Model
{
    use HasFactory;

    public function assembly(): BelongsTo
    {
        return $this->belongsTo(JobAssembly::class, 'job_assembly_id');
    }

    public function reference(): BelongsTo
    {
        return $this->belongsTo(Part::class, 'reference_id');
    }
}
