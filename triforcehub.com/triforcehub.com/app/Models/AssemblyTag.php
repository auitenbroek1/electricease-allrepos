<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsToMany;

class AssemblyTag extends Model
{
    use HasFactory;

    public function assemblies(): BelongsToMany
    {
        return $this->belongsToMany(Assembly::class);
    }
}
