<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreAssemblyTagRequest;
use App\Http\Requests\UpdateAssemblyTagRequest;
use App\Http\Resources\AssemblyTagResource;
use App\Models\AssemblyTag;
use Illuminate\Http\Request;
use Illuminate\Support\Str;

class AssemblyTagController extends Controller
{
    public function index(Request $request)
    {
        $q = $request->query('q');
        $size = $request->query('size', 6);

        $collection = AssemblyTag::query()
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

        $results = AssemblyTagResource::collection($collection);

        return $results;
    }

    public function store(StoreAssemblyTagRequest $request)
    {
        $this->authorize('create', AssemblyTag::class);

        $tag = new AssemblyTag;

        $tag->uuid = Str::uuid();
        $tag->member_id = $request->user()->member_id;
        $tag->name = $request->input('name');
        $tag->description = $request->input('description');
        $tag->color = $request->input('color');

        $tag->save();

        return new AssemblyTagResource($tag);
    }

    public function show(AssemblyTag $tag)
    {
        return new AssemblyTagResource($tag);
    }

    public function update(UpdateAssemblyTagRequest $request, AssemblyTag $tag)
    {
        $this->authorize('update', $tag);

        $_tag = clone $tag;

        if (! $_tag->member_id || $_tag->member_id === 1) {
            if ($request->user()->member_id !== 1) {
                $clone = $_tag->replicate();
                $clone->uuid = Str::uuid();
                $clone->member_id = $request->user()->member_id;
                $clone->save();

                $_tag = clone $clone;
            }
        }

        $_tag->name = $request->input('name');
        $_tag->description = $request->input('description');
        $_tag->color = $request->input('color');

        $_tag->save();

        return new AssemblyTagResource($_tag);
    }

    public function destroy(AssemblyTag $tag)
    {
        $this->authorize('delete', $tag);

        $tag->delete();

        return new AssemblyTagResource($tag);
    }
}
