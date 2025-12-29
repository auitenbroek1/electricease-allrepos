<?php

namespace App\Http\Controllers;

use App\Http\Requests\StorePartCategoryRequest;
use App\Http\Requests\UpdatePartCategoryRequest;
use App\Http\Resources\PartCategoryResource;
use App\Models\PartCategory;
use Illuminate\Http\Request;
use Illuminate\Support\Str;

class PartCategoryController extends Controller
{
    public function index(Request $request)
    {
        $q = $request->query('q');
        $size = $request->query('size', 6);

        $collection = PartCategory::query()
            ->when($request->user()->member_id !== 1, function ($query) use ($request) {
                return $query->where(function ($query) use ($request) {
                    return $query
                        ->whereNull('member_id')
                        ->orWhereIn('member_id', [1, $request->user()->member_id]);
                });
            })
            ->where(function ($query) use ($q) {
                return $query->when($q, function ($query, $q) {
                    return $query
                        ->where('name', 'like', '%'.$q.'%')
                        ->orWhere('description', 'like', '%'.$q.'%');
                });
            })
            ->orderBy('name')
            ->paginate($size);

        $results = PartCategoryResource::collection($collection);

        return $results;
    }

    public function store(StorePartCategoryRequest $request)
    {
        $this->authorize('create', PartCategory::class);

        $category = new PartCategory;

        $category->uuid = Str::uuid();
        $category->member_id = $request->user()->member_id;
        $category->parent_id = $request->input('parent_id');
        $category->name = $request->input('name');
        $category->description = $request->input('description');

        $category->save();

        return new PartCategoryResource($category);
    }

    public function show(PartCategory $category)
    {
        return new PartCategoryResource($category);
    }

    public function update(UpdatePartCategoryRequest $request, PartCategory $category)
    {
        $this->authorize('update', $category);

        $_category = clone $category;

        if (! $_category->member_id || $_category->member_id === 1) {
            if ($request->user()->member_id !== 1) {
                $clone = $_category->replicate();
                $clone->uuid = Str::uuid();
                $clone->member_id = $request->user()->member_id;
                $clone->save();

                $_category = clone $clone;
            }
        }

        $_category->parent_id = $request->input('parent_id');
        $_category->name = $request->input('name');
        $_category->description = $request->input('description');

        $_category->save();

        return new PartCategoryResource($_category);
    }

    public function destroy(PartCategory $category)
    {
        $this->authorize('delete', $category);

        $category->delete();

        return new PartCategoryResource($category);
    }
}
