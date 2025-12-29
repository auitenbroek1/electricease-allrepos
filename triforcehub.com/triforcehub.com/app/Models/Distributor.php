<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsToMany;

class Distributor extends Model
{
    use HasFactory;

    public function members(): BelongsToMany
    {
        return $this->belongsToMany(Member::class)->withPivot(['username', 'password', 'enabled']);
    }
}
