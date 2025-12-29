<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\BelongsToMany;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Support\Str;

class Member extends Model
{
    use HasFactory;

    protected $fillable = [
        'customer',
        'name',
        'email',
        'logo_id',
        'address1',
        'address2',
        'city',
        'state_id',
        'zip',
        'office',
        'mobile',

        'feature_digital_takeoff_enabled',
        'feature_linear_with_drops_enabled',
        'feature_auto_count_enabled',
    ];

    public static function boot()
    {
        parent::boot();

        self::creating(function ($model) {
            $model->uuid = Str::uuid();
        });
    }

    protected function casts(): array
    {
        return [
            'principal' => 'boolean',

            'feature_digital_takeoff_enabled' => 'boolean',
            'feature_linear_with_drops_enabled' => 'boolean',
            'feature_auto_count_enabled' => 'boolean',
        ];
    }

    public function distributors(): BelongsToMany
    {
        return $this->belongsToMany(Distributor::class)->withPivot(['username', 'password', 'customer', 'enabled']);
    }

    public function features(): BelongsToMany
    {
        return $this->belongsToMany(Feature::class);
    }

    public function logo(): BelongsTo
    {
        return $this->belongsTo(Upload::class);
    }

    public function state(): BelongsTo
    {
        return $this->belongsTo(State::class);
    }

    public function users(): HasMany
    {
        return $this->hasMany(User::class);
    }
}
