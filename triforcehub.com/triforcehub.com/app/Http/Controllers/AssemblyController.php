<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreAssemblyRequest;
use App\Http\Requests\UpdateAssemblyRequest;
use App\Http\Resources\AssemblyResource;
use App\Models\Assembly;
use Illuminate\Http\Request;
use Illuminate\Support\Str;

class AssemblyController extends Controller
{
    public function index(Request $request)
    {
        if (in_array($request->user()->member_id, [357])) {
            return AssemblyResource::collection([]);
        }

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
            $collection = Assembly::search($q)
                ->when(! $request->user()->administrator, function ($query) use ($request) {
                    return $query->whereIn('member.id', [1, $request->user()->member_id]);
                })
                ->when($categories, function ($query) use ($categories) {
                    return $query->whereIn('categories.id', $categories);
                })
                ->query(function ($query) {
                    return $query->with(['categories', 'favorites', 'parts', 'tags']);
                })
                ->paginate($size);
        } else {
            $collection = Assembly::with(['categories', 'favorites', 'parts', 'tags'])
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
                            $query->whereIn('assembly_category_id', $categories);
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

        $results = AssemblyResource::collection($collection);

        return $results;
    }

    public function store(StoreAssemblyRequest $request)
    {
        $this->authorize('create', Assembly::class);

        $parts = collect($request->input('parts'))->mapWithKeys(function ($item, $key) {
            return [
                $item['id'] => ['quantity' => $item['quantity']],
            ];
        });

        $assembly = new Assembly;

        $assembly->uuid = Str::uuid();
        $assembly->member_id = $request->user()->member_id;
        $assembly->name = $request->input('name');
        $assembly->description = $request->input('description');
        $assembly->save();

        $assembly->categories()->sync($request->input('categories'));
        $assembly->tags()->sync($request->input('tags'));
        $assembly->parts()->sync($parts);
        $assembly->save();

        return new AssemblyResource($assembly->load(['categories', 'tags', 'parts']));
    }

    public function show(Assembly $assembly)
    {
        return new AssemblyResource($assembly->load(['categories', 'tags', 'parts']));
    }

    public function update(UpdateAssemblyRequest $request, Assembly $assembly)
    {
        $this->authorize('update', $assembly);

        $parts = collect($request->input('parts'))->mapWithKeys(function ($item, $key) {
            return [
                $item['id'] => ['quantity' => $item['quantity']],
            ];
        });

        $_assembly = clone $assembly;

        if (! $_assembly->member_id || $_assembly->member_id === 1) {
            if ($request->user()->member_id !== 1) {
                $clone = $_assembly->replicate();
                $clone->uuid = Str::uuid();
                $clone->member_id = $request->user()->member_id;
                $clone->save();

                $_assembly = clone $clone;
            }
        }

        $_assembly->name = $request->input('name');
        $_assembly->description = $request->input('description');
        $_assembly->categories()->sync($request->input('categories'));
        $_assembly->tags()->sync($request->input('tags'));
        $_assembly->parts()->sync($parts);

        $_assembly->save();

        return new AssemblyResource($_assembly->load(['categories', 'tags', 'parts']));
    }

    public function destroy(Assembly $assembly)
    {
        $this->authorize('delete', $assembly);

        $assembly->delete();

        return new AssemblyResource($assembly);
    }

    public function duplicate(Assembly $assembly, Request $request)
    {
        $this->authorize('duplicate', $assembly);

        $_assembly = $assembly->replicate();
        $_assembly->uuid = Str::uuid();
        $_assembly->name = 'Duplicate of '.$assembly->name;
        $_assembly->member_id = $request->user()->member_id;
        $_assembly->save();

        foreach ($assembly->categories as $category) {
            $_assembly->categories()->save($category);
        }

        foreach ($assembly->parts as $part) {
            $_assembly->parts()->save($part, ['quantity' => $part->pivot->quantity]);
        }

        return new AssemblyResource($_assembly->load(['categories', 'tags', 'parts']));
    }
}
