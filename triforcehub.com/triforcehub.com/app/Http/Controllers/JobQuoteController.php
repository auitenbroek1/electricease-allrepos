<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobQuoteRequest;
use App\Http\Requests\UpdateJobQuoteRequest;
use App\Http\Resources\JobQuoteResource;
use App\Models\JobQuote;
use Illuminate\Support\Str;

class JobQuoteController extends Controller
{
    public function store(StoreJobQuoteRequest $request)
    {
        $quote = new JobQuote;
        $quote->job_id = $request->input('job_id');
        $quote->uuid = Str::uuid();
        $quote->name = $request->input('name') ?? '';
        $quote->cost = $request->input('cost');
        $quote->notes = $request->input('notes');
        $quote->enabled = $request->input('enabled');
        $quote->save();

        return new JobQuoteResource($quote);
    }

    public function update(UpdateJobQuoteRequest $request, JobQuote $quote)
    {
        $quote->name = $request->input('name');
        $quote->cost = $request->input('cost');
        $quote->notes = $request->input('notes');
        $quote->enabled = $request->input('enabled');
        $quote->save();

        return new JobQuoteResource($quote);
    }

    public function destroy(JobQuote $quote)
    {
        $quote->delete();
    }
}
