<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class JobLabor extends Model
{
    use HasFactory;

    protected $fillable = [
        'name',
        'hours',
        'rate',
        'burden',
        'fringe',
        'notes',
        'enabled',
    ];
}
