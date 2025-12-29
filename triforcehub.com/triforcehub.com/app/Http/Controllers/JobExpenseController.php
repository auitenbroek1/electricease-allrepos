<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobExpenseRequest;
use App\Http\Requests\UpdateJobExpenseRequest;
use App\Http\Resources\JobExpenseResource;
use App\Models\JobExpense;
use Illuminate\Support\Str;

class JobExpenseController extends Controller
{
    public function store(StoreJobExpenseRequest $request)
    {
        $expense = new JobExpense;
        $expense->job_id = $request->input('job_id');
        $expense->uuid = Str::uuid();
        $expense->name = $request->input('name') ?? '';
        $expense->cost = $request->input('cost');
        $expense->notes = $request->input('notes');
        $expense->enabled = $request->input('enabled');
        $expense->save();

        return new JobExpenseResource($expense);
    }

    public function update(UpdateJobExpenseRequest $request, JobExpense $expense)
    {
        $expense->name = $request->input('name');
        $expense->cost = $request->input('cost');
        $expense->notes = $request->input('notes');
        $expense->enabled = $request->input('enabled');
        $expense->save();

        return new JobExpenseResource($expense);
    }

    public function destroy(JobExpense $expense)
    {
        $expense->delete();
    }
}
