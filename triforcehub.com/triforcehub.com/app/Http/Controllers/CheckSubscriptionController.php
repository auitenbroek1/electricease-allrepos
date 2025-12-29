<?php

namespace App\Http\Controllers;

use Illuminate\Http\Request;

class CheckSubscriptionController extends Controller
{
    public function __invoke(Request $request)
    {
        return view('auth.check');
    }
}
