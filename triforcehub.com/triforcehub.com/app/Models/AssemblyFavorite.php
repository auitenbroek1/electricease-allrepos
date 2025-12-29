<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Relations\Pivot;

class AssemblyFavorite extends Pivot
{
    protected $table = 'assembly_favorite';

    public $incrementing = true;
}
