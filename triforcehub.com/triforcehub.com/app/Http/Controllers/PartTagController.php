<?php

namespace App\Http\Controllers;

use App\Http\Requests\StorePartTagRequest;
use App\Http\Requests\UpdatePartTagRequest;
use App\Http\Resources\PartTagResource;
use App\Models\PartTag;
use Illuminate\Http\Request;
use Illuminate\Support\Str;

class PartTagController extends Controller
{
    public function index(Request $request)
    {
        $q = $request->query('q');
        $size = $request->query('size', 6);

        $collection = PartTag::query()
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

        $results = PartTagResource::collection($collection);

        return $results;
    }

    public function store(StorePartTagRequest $request)
    {
        $this->authorize('create', PartTag::class);

        $tag = new PartTag;

        $tag->uuid = Str::uuid();
        $tag->member_id = $request->user()->member_id;
        $tag->name = $request->input('name');
        $tag->description = $request->input('description');
        $tag->color = $request->input('color');

        $tag->save();

        return new PartTagResource($tag);
    }

    public function show(PartTag $tag)
    {
        return new PartTagResource($tag);
    }

    public function update(UpdatePartTagRequest $request, PartTag $tag)
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

        return new PartTagResource($_tag);
    }

    public function destroy(PartTag $tag)
    {
        $this->authorize('delete', $tag);

        $tag->delete();

        return new PartTagResource($tag);
    }
}
