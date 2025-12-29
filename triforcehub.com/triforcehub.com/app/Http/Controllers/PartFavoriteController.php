<?php

namespace App\Http\Controllers;

use App\Models\Part;
use App\Models\PartFavorite;
use Illuminate\Http\Request;
use Illuminate\Http\Response;

class PartFavoriteController extends Controller
{
    public function store(Request $request, Part $part): Response
    {
        $this->authorize('create', [PartFavorite::class, $part]);

        $part->favorites()->attach($request->user()->id);

        return response('', 201);
    }

    public function destroy(Request $request, Part $part): Response
    {
        $this->authorize('delete', [PartFavorite::class, $part]);

        $part->favorites()->detach($request->user()->id);

        return response('', 200);
    }
}
