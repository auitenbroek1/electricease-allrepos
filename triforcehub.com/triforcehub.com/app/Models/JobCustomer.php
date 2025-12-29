<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;

class JobCustomer extends Model
{
    use HasFactory;

    protected $fillable = [
        'name',
        'email',
        'address1',
        'address2',
        'city',
        'state_id',
        'zip',
        'mobile',
        'office',
    ];

    public function state(): BelongsTo
    {
        return $this->belongsTo(State::class);
    }
}
