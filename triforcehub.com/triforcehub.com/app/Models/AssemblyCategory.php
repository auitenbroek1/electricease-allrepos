<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsToMany;

class AssemblyCategory extends Model
{
    use HasFactory;

    public function assemblies(): BelongsToMany
    {
        return $this->belongsToMany(Assembly::class);
    }

    public function scopeDefault($query)
    {
        return $query->where('member_id', 1)
            ->orWhereNull('member_id');
    }
}
