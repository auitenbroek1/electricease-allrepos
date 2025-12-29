<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\BelongsToMany;
use Illuminate\Support\Facades\Auth;
use Laravel\Scout\EngineManager;
use Laravel\Scout\Searchable;

class Part extends Model
{
    use HasFactory, Searchable;

    protected $fillable = [
        'uuid',
        'name',
        'unit_id',
        'cost',
        'labor',
    ];

    protected $with = [
        'categories',
        'tags',
    ];

    public function assemblies(): BelongsToMany
    {
        return $this->belongsToMany(Assembly::class)->withPivot('quantity');
    }

    public function categories(): BelongsToMany
    {
        return $this->belongsToMany(PartCategory::class);
    }

    public function favorites(): BelongsToMany
    {
        return $this->belongsToMany(User::class, 'part_favorite')->using(PartFavorite::class);
    }

    public function member(): BelongsTo
    {
        return $this->belongsTo(Member::class);
    }

    public function tags(): BelongsToMany
    {
        return $this->belongsToMany(PartTag::class);
    }

    public function upcs(): BelongsToMany
    {
        return $this->belongsToMany(PartUPC::class, 'part_part_upc', 'part_id', 'part_upc_id');
    }

    //

    public function scopeDefault($query)
    {
        return $query->where('member_id', 1)
            ->orWhereNull('member_id');
    }

    public function searchableUsing()
    {
        $engine = 'hybrid';

        if ($user = Auth::user()) {
            $feature = $user->member->features()->where('slug', 'semantic-search')->first();
            if ($feature) {
                $engine = 'upstash';
            }
        }

        return app(EngineManager::class)->engine($engine);
    }

    public function toSearchableArray()
    {
        $array = $this->toArray();

        $categories = collect($array['categories'] ?? [])->map(function ($category) {
            return collect($category)->only(['id', 'name']);
        })->toArray();

        $tags = collect($array['tags'] ?? [])->map(function ($tag) {
            return collect($tag)->only(['id', 'name']);
        })->toArray();

        return [
            'id' => $array['id'],
            'member' => [
                'id' => $array['member_id'] ?? 1,
            ],
            'name' => $array['name'],
            'categories' => $categories,
            'tags' => $tags,
        ];
    }
}
