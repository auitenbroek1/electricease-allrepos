<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\Relations\BelongsTo;
use Illuminate\Database\Eloquent\Relations\HasMany;

class Job extends Model
{
    use HasFactory;

    public function adjustments(): HasMany
    {
        return $this->hasMany(JobAdjustment::class);
    }

    public function blocks(): HasMany
    {
        return $this->hasMany(JobBlock::class)->orderBy('order');
    }

    public function children(): HasMany
    {
        return $this->hasMany(Job::class, 'parent_id', 'id');
    }

    public function crews(): HasMany
    {
        return $this->hasMany(JobCrew::class);
    }

    public function customers(): HasMany
    {
        return $this->hasMany(JobCustomer::class);
    }

    public function events(): HasMany
    {
        return $this->hasMany(JobEvent::class);
    }

    public function expenses(): HasMany
    {
        return $this->hasMany(JobExpense::class);
    }

    public function files(): HasMany
    {
        return $this->hasMany(JobFile::class);
    }

    public function labors(): HasMany
    {
        return $this->hasMany(JobLabor::class);
    }

    public function locations(): HasMany
    {
        return $this->hasMany(JobLocation::class);
    }

    public function member(): BelongsTo
    {
        return $this->belongsTo(Member::class, 'member_id');
    }

    public function parent(): BelongsTo
    {
        return $this->belongsTo(Job::class, 'parent_id');
    }

    public function phases(): HasMany
    {
        return $this->hasMany(JobPhase::class);
    }

    public function plans(): HasMany
    {
        return $this->hasMany(JobPlan::class);
    }

    public function quotes(): HasMany
    {
        return $this->hasMany(JobQuote::class);
    }

    public function status(): BelongsTo
    {
        return $this->belongsTo(JobStatus::class, 'job_status_id');
    }

    public function type(): BelongsTo
    {
        return $this->belongsTo(JobType::class, 'job_type_id');
    }
}
