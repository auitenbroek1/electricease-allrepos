<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreJobCustomerRequest;
use App\Http\Requests\UpdateJobCustomerRequest;
use App\Http\Resources\JobCustomerResource;
use App\Models\JobCustomer;
use Illuminate\Support\Str;

class JobCustomerController extends Controller
{
    public function store(StoreJobCustomerRequest $request)
    {
        $customer = new JobCustomer;
        $customer->job_id = $request->input('job_id');
        $customer->uuid = Str::uuid();
        $customer->name = $request->input('name');
        $customer->email = $request->input('email');
        $customer->address1 = $request->input('address1');
        $customer->address2 = $request->input('address2');
        $customer->city = $request->input('city');
        $customer->state_id = $request->input('state_id');
        $customer->zip = $request->input('zip');
        $customer->mobile = $request->input('mobile');
        $customer->office = $request->input('office');
        $customer->save();

        return new JobCustomerResource($customer);
    }

    public function update(UpdateJobCustomerRequest $request, JobCustomer $customer)
    {
        $customer->update($request->validated());

        return new JobCustomerResource($customer);
    }

    public function destroy(JobCustomer $customer)
    {
        $customer->delete();
    }
}
