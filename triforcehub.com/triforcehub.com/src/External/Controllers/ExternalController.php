<?php

namespace Src\External\Controllers;

use App\Http\Controllers\Controller;
use App\Models\Job;
use App\Models\JobCustomer;
use App\Models\State;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Validator;
use Illuminate\Support\Str;

class ExternalController extends Controller
{
    private function bail($data, $status = 403)
    {
        abort(response()->json($data, $status));
    }

    private function validate_ip($request)
    {
        if (app()->environment('local')) {
            return;
        }

        $allowed = [
            '70.39.11.110',
        ];

        if (! in_array($request->ip(), $allowed)) {
            $this->bail($request->ip());
        }
    }

    private function validate_member(Request $request)
    {
        $this->validate_ip($request);

        $key = $request->header('x-api-key');

        if ($key === '3671b0f9-2c73-412e-a88c-109f8309c74c') {
            return 8;
        }

        $this->bail(0);
    }

    private function validate_bid(int $bid_id, int $member_id)
    {
        $job = Job::query()
            ->where([
                'id' => $bid_id,
                'member_id' => $member_id,
            ])
            ->first();

        if ($job) {
            return $job->id;
        }

        $this->bail(1);
    }

    private function validate_customer(int $customer_id, int $bid_id)
    {
        $customer = JobCustomer::query()
            ->where([
                'id' => $customer_id,
                'job_id' => $bid_id,
            ])
            ->first();

        if ($customer) {
            return $customer->id;
        }

        $this->bail(2);
    }

    public function bids(Request $request)
    {
        $member_id = $this->validate_member($request);

        //

        $bids = Job::query()
            ->select([
                'id',
                'name',
                'description',
                'created_at',
            ])
            ->where(['member_id' => $member_id])
            ->get();

        return $bids;
    }

    public function customers(int $bid_id, Request $request)
    {
        $member_id = $this->validate_member($request);
        $job_id = $this->validate_bid($bid_id, $member_id);

        //

        $customers = JobCustomer::query()
            ->select([
                'id',
                'name',
                'email',
                'address1',
                'address2',
                'city',
                'state_id',
                'zip',
                'office',
                'mobile',
                'created_at',
            ])
            ->where([
                'job_id' => $job_id,
            ])
            ->get();

        return $customers;
    }

    public function customers_create(int $bid_id, Request $request)
    {
        $member_id = $this->validate_member($request);
        $job_id = $this->validate_bid($bid_id, $member_id);

        //

        $validator = Validator::make($request->all(), [
            'name' => 'required',
            'email' => 'required|email',
            'address1' => 'required',
            'address2' => '',
            'city' => 'required',
            'state' => 'required|max:2',
            'zip' => 'required|numeric|digits:5',
            'mobile' => 'required|numeric|digits:10',
            'office' => 'required|numeric|digits:10',
        ]);

        if ($validator->fails()) {
            $this->bail($validator->errors(), 400);
        }

        $data = $validator->validated();

        //

        try {
            $state = State::where(['abbreviation' => $data['state']])->first();

            $customer = new JobCustomer;
            $customer->job_id = $job_id;
            $customer->uuid = Str::uuid();
            $customer->name = $data['name'];
            $customer->email = $data['email'];
            $customer->address1 = $data['address1'];
            $customer->address2 = $data['address2'];
            $customer->city = $data['city'];
            $customer->state_id = $state->id;
            $customer->zip = $data['zip'];
            $customer->mobile = $data['mobile'];
            $customer->office = $data['office'];
            $customer->save();
        } catch (\Throwable $exception) {
            $this->bail(['errors' => [$exception->getMessage()]], 400);
        }

        return $customer;
    }

    public function customers_delete(int $bid_id, int $customer_id, Request $request)
    {
        $member_id = $this->validate_member($request);
        $job_id = $this->validate_bid($bid_id, $member_id);
        $customer_id = $this->validate_customer($customer_id, $job_id);

        //

        try {
            $customer = JobCustomer::find($customer_id);
            $customer->delete();
        } catch (\Throwable $exception) {
            $this->bail(['errors' => [$exception->getMessage()]], 400);
        }

        $this->bail(['messages' => ['Done.']], 200);
    }
}
