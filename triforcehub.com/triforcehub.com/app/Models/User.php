<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Casts\Attribute;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Foundation\Auth\User as Authenticatable;
use Illuminate\Notifications\Notifiable;
use Illuminate\Support\Str;
use Laravel\Sanctum\HasApiTokens;

class User extends Authenticatable
{
    use HasApiTokens, HasFactory, Notifiable;

    protected $fillable = [
        'member_id',
        'name',
        'email',
        'password',
    ];

    protected $hidden = [
        'password',
        'remember_token',
    ];

    protected $with = [
        'member',
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
            'email_verified_at' => 'datetime',
            'principal' => 'boolean',
        ];
    }

    protected function administrator(): Attribute
    {
        return Attribute::make(
            get: function ($value, $attributes) {
                return $this->member->principal;
            }
        );
    }

    protected function root(): Attribute
    {
        return Attribute::make(
            get: function ($value, $attributes) {
                return $this->principal && $this->member->principal;
            }
        );
    }

    public function member(): BelongsTo
    {
        return $this->belongsTo(Member::class);
    }
}
