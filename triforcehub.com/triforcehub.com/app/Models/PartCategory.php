<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsToMany;
use Illuminate\Support\Str;

class PartCategory extends Model
{
    use HasFactory;

    protected $fillable = [
        'name',
    ];

    public static function boot()
    {
        parent::boot();

        self::creating(function ($model) {
            $model->uuid = Str::uuid();
        });
    }

    public function parts(): BelongsToMany
    {
        return $this->belongsToMany(Part::class);
    }

    public function scopeDefault($query)
    {
        return $query->where('member_id', 1)
            ->orWhereNull('member_id');
    }
}
