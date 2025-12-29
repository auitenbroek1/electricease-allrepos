<?php

namespace App\Http\Controllers;

use App\Models\Assembly;
use App\Models\AssemblyFavorite;
use Illuminate\Http\Request;
use Illuminate\Http\Response;

class AssemblyFavoriteController extends Controller
{
    public function store(Request $request, Assembly $assembly): Response
    {
        $this->authorize('create', [AssemblyFavorite::class, $assembly]);

        $assembly->favorites()->attach($request->user()->id);

        return response('', 201);
    }

    public function destroy(Request $request, Assembly $assembly): Response
    {
        $this->authorize('delete', [AssemblyFavorite::class, $assembly]);

        $assembly->favorites()->detach($request->user()->id);

        return response('', 200);
    }
}
