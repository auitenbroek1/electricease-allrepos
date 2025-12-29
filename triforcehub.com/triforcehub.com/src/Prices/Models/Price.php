<?php

namespace Src\Prices\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Support\Str;

class Price extends Model
{
    use HasFactory;

    protected $fillable = [
        'member_id',
        'distributor_id',
        'part_upc_id',
        'cost',
    ];

    public static function boot()
    {
        parent::boot();

        self::creating(function ($model) {
            $model->uuid = Str::uuid();
        });
    }
}
