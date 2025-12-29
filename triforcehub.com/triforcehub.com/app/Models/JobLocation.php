<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;

class JobLocation extends Model
{
    use HasFactory;

    protected $fillable = [
        'name',
        'address1',
        'address2',
        'city',
        'state_id',
        'zip',
    ];

    public function state(): BelongsTo
    {
        return $this->belongsTo(State::class);
    }
}
