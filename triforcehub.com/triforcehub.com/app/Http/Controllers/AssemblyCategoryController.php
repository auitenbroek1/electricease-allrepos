<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreAssemblyCategoryRequest;
use App\Http\Requests\UpdateAssemblyCategoryRequest;
use App\Http\Resources\AssemblyCategoryResource;
use App\Models\AssemblyCategory;
use Illuminate\Http\Request;
use Illuminate\Support\Str;

class AssemblyCategoryController extends Controller
{
    public function index(Request $request)
    {
        $q = $request->query('q');
        $size = $request->query('size', 6);

        $collection = AssemblyCategory::query()
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

        $results = AssemblyCategoryResource::collection($collection);

        return $results;
    }

    public function store(StoreAssemblyCategoryRequest $request)
    {
        $this->authorize('create', AssemblyCategory::class);

        $category = new AssemblyCategory;

        $category->uuid = Str::uuid();
        $category->member_id = $request->user()->member_id;
        $category->parent_id = $request->input('parent_id');
        $category->name = $request->input('name');
        $category->description = $request->input('description');

        $category->save();

        return new AssemblyCategoryResource($category);
    }

    public function show(AssemblyCategory $category)
    {
        return new AssemblyCategoryResource($category);
    }

    public function update(UpdateAssemblyCategoryRequest $request, AssemblyCategory $category)
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

        return new AssemblyCategoryResource($_category);
    }

    public function destroy(AssemblyCategory $category)
    {
        $this->authorize('delete', $category);

        $category->delete();

        return new AssemblyCategoryResource($category);
    }
}
