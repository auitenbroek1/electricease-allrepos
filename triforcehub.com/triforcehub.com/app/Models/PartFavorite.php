<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Relations\Pivot;

class PartFavorite extends Pivot
{
    protected $table = 'part_favorite';

    public $incrementing = true;
}
