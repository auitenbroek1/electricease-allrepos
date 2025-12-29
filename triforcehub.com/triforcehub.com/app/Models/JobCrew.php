<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class JobCrew extends Model
{
    use HasFactory;

    protected $fillable = [
        'name',
        'quantity',
        'rate',
        'burden',
        'fringe',
        'notes',
        'enabled',
    ];
}
