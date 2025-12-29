<?php

namespace App\Http\Controllers;

use App\Http\Requests\StorePartRequest;
use App\Http\Requests\UpdatePartRequest;
use App\Http\Resources\PartResource;
use App\Models\Part;
use Illuminate\Http\Request;
use Illuminate\Support\Str;

class PartController extends Controller
{
    private function splitStringIntoWords($input)
    {
        // Check if the input string already contains spaces
        if (str_contains($input, ' ')) {
            // If it does, return the input as is
            return $input;
        }

        // Use regular expression to split numbers and letters
        $parts = preg_split('/((?<=\d)(?=[a-zA-Z])|(?<=[a-zA-Z])(?=\d))/', $input);

        // Join the parts with a space
        return implode(' ', $parts);
    }

    public function index(Request $request)
    {
        $use_scout = env('SCOUT_DRIVER');

        $q = $request->query('q');
        $size = $request->query('size', 6);

        $categories = $request->query('categories', '');
        $categories = collect(explode(',', $categories))->filter()->toArray();

        $favorites = $request->query('favorites', '');
        if ($favorites === 'true') {
            $use_scout = false;
        }

        if ($use_scout) {
            $q = $this->splitStringIntoWords($q);

            $collection = Part::search($q)
                ->when(! $request->user()->administrator, function ($query) use ($request) {
                    return $query->whereIn('member.id', [1, $request->user()->member_id]);
                })
                ->when($categories, function ($query) use ($categories) {
                    return $query->whereIn('categories.id', $categories);
                })
                ->query(function ($query) {
                    return $query->with(['assemblies', 'categories', 'favorites', 'tags', 'upcs']);
                })
                ->paginate($size);
        } else {
            $collection = Part::with(['assemblies', 'categories', 'favorites', 'tags', 'upcs'])
                ->when($request->user()->member_id !== 1, function ($query) use ($request) {
                    return $query->where(function ($query) use ($request) {
                        return $query
                            ->whereNull('member_id')
                            ->orWhereIn('member_id', [1, $request->user()->member_id]);
                    });
                })
                ->where(function ($query) use ($q, $categories) {
                    return $query->when($q, function ($query, $q) {
                        return $query
                            ->where('name', 'like', '%'.$q.'%')
                            ->orWhere('description', 'like', '%'.$q.'%')
                            ->orWhereHas('categories', function ($query) use ($q) {
                                $query
                                    ->where('name', 'like', '%'.$q.'%')
                                    ->orWhere('description', 'like', '%'.$q.'%');
                            })
                            ->orWhereHas('tags', function ($query) use ($q) {
                                $query
                                    ->where('name', 'like', '%'.$q.'%')
                                    ->orWhere('description', 'like', '%'.$q.'%');
                            });
                    })->when($categories, function ($query, $categories) {
                        return $query->whereHas('categories', function ($query) use ($categories) {
                            $query->whereIn('part_category_id', $categories);
                        });
                    });
                })
                ->where(function ($query) use ($request, $favorites) {
                    $query->when($favorites === 'true', function ($query) use ($request) {
                        return $query->whereHas('favorites', function ($query) use ($request) {
                            $query->where('user_id', $request->user()->id);
                        });
                    });
                })
                ->orderBy('name')
                ->paginate($size);
        }

        $results = PartResource::collection($collection);

        return $results;
    }

    public function store(StorePartRequest $request)
    {
        $this->authorize('create', Part::class);

        $part = new Part;

        $part->uuid = Str::uuid();
        $part->member_id = $request->user()->member_id;
        $part->name = $request->input('name');
        $part->description = $request->input('description');
        $part->unit_id = 1;
        $part->cost = $request->input('cost');
        $part->labor = $request->input('labor');
        $part->save();

        $part->categories()->sync($request->input('categories'));
        $part->tags()->sync($request->input('tags'));
        $part->save();

        return new PartResource($part);
    }

    public function show(Part $part)
    {
        return new PartResource($part->load(['assemblies', 'categories', 'tags', 'upcs']));
    }

    public function update(UpdatePartRequest $request, Part $part)
    {
        $this->authorize('update', $part);

        $_part = clone $part;

        if (! $_part->member_id || $_part->member_id === 1) {
            if ($request->user()->member_id !== 1) {
                $clone = $_part->replicate();
                $clone->uuid = Str::uuid();
                $clone->member_id = $request->user()->member_id;
                $clone->save();

                $_part = clone $clone;
            }
        }

        $_part->name = $request->input('name');
        $_part->description = $request->input('description');
        $_part->cost = $request->input('cost');
        $_part->labor = $request->input('labor');
        $_part->categories()->sync($request->input('categories'));
        $_part->tags()->sync($request->input('tags'));

        $_part->save();

        return new PartResource($_part);
    }

    public function destroy(Part $part)
    {
        $this->authorize('delete', $part);

        $part->delete();

        return new PartResource($part);
    }

    public function duplicate(Part $part, Request $request)
    {
        $this->authorize('duplicate', $part);

        $_part = $part->replicate();
        $_part->uuid = Str::uuid();
        $_part->name = 'Duplicate of '.$_part->name;
        $_part->member_id = $request->user()->member_id;
        $_part->save();

        foreach ($part->categories as $category) {
            $_part->categories()->save($category);
        }

        return new PartResource($_part);
    }
}
