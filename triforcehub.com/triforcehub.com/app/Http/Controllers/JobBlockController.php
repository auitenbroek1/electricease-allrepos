<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobBlockRequest;
use App\Http\Requests\UpdateJobBlockRequest;
use App\Http\Resources\JobBlockResource;
use App\Models\JobBlock;
use Illuminate\Http\Request;
use Illuminate\Support\Str;

class JobBlockController extends Controller
{
    public function store(StoreJobBlockRequest $request)
    {
        $block = new JobBlock;
        $block->job_id = $request->input('job_id');
        $block->job_section_id = $request->input('job_section_id');
        $block->uuid = Str::uuid();
        $block->content = $request->input('content') ?? '';
        $block->order = $request->input('order') ?? 999;
        $block->save();

        return new JobBlockResource($block);
    }

    public function update(UpdateJobBlockRequest $request, JobBlock $block)
    {
        $block->update($request->validated());

        return new JobBlockResource($block);
    }

    public function destroy(JobBlock $block)
    {
        $block->delete();
    }

    //

    public function sort(Request $request)
    {
        foreach ($request->all() as $order => $id) {
            JobBlock::find($id)->update(['order' => $order]);
        }
    }
}
