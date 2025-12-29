<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class JobAdjustment extends Model
{
    use HasFactory;

    protected $fillable = [
        'override',
        'overhead',
        'profit',
        'tax',
        'enabled',
    ];
}
