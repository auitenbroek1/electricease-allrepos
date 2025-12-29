<?php

namespace App\Http\Controllers;

use App\Models\Member;
use App\Models\User;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Str;
use Throwable;

class FreeTrialController extends Controller
{
    public function __invoke(Request $request)
    {
        // region

        $payload = $request->getContent();

        Log::info($payload);

        try {
            $data = simplexml_load_string($payload)
                ->children('http://schemas.xmlsoap.org/soap/envelope/')
                ->Body
                ->children('http://soap.sforce.com/2005/09/outbound')
                ->notifications
                ->Notification
                ->sObject
                ->children('urn:sobject.enterprise.soap.sforce.com');

            // ray($data);

            $company = (string) $data->Company;
            $email = (string) $data->Email;
            $first_name = (string) $data->FirstName;
            $last_name = (string) $data->LastName;
            $phone = (string) $data->Phone;

            if (! Str::length($company)) {
                $company = (string) $data->Opportunity_Company__c;
            }

            if (! Str::length($email)) {
                $email = (string) $data->Opportunity_Email__c;
            }

            if (! Str::length($first_name)) {
                $first_name = (string) $data->Opportunity_First_Name__c;
            }

            if (! Str::length($last_name)) {
                $last_name = (string) $data->Opportunity_Last_Name__c;
            }

            if (! Str::length($phone)) {
                $phone = (string) $data->Opportunity_Phone_Number__c;
            }
        } catch (Throwable $e) {
            // ray($e);

            return $this->response();
        }

        // endregion

        // region

        if (! Str::length($company)) {
            // ray('missing company');

            return $this->response();
        }

        if (! Str::length($email)) {
            // ray('missing email');

            return $this->response();
        }

        if (! Str::length($first_name)) {
            // ray('missing first_name');

            return $this->response();
        }

        if (! Str::length($last_name)) {
            // ray('missing last_name');

            return $this->response();
        }

        if ($phone) {
            $phone = preg_replace('/[^0-9]/', '', $phone);
        }

        if (! Str::length($phone)) {
            // ray('missing phone');

            return $this->response();
        }

        // ray($company);
        // ray($email);
        // ray($first_name);
        // ray($last_name);
        // ray($phone);

        // endregion

        // region

        if ($member = Member::where('email', $email)->first()) {
            // ray('member already exists', $member);

            return $this->response();
        }

        if ($user = User::where('email', $email)->first()) {
            // ray('user exists', $user);

            return $this->response();
        }

        // endregion

        // region

        DB::beginTransaction();

        try {
            $member = Member::create([
                'name' => $company,
                'email' => $email,
                'address1' => '101 NE Cir Dr',
                'city' => 'Grimes',
                'state_id' => 16,
                'zip' => '50111',
                'office' => $phone,
                'enabled' => true,
            ]);

            // ray($member);

            $user = new User;
            $user->uuid = Str::uuid();
            $user->member_id = $member->id;
            $user->name = $first_name.' '.$last_name;
            $user->email = $email;
            $user->password = Hash::make('EETrial22!');
            $user->save();

            // ray($user);

            DB::commit();
        } catch (Throwable $e) {
            // ray($e);

            DB::rollback();
        }

        // endregion

        return $this->response();
    }

    private function response()
    {
        $data = trim('
            <soapenv:Envelope xmlns:soapenv="http://schemas.xmlsoap.org/soap/envelope/">
                <soapenv:Body>
                    <notificationsResponse xmlns="http://soap.sforce.com/2005/09/outbound">
                        <Ack>true</Ack>
                    </notificationsResponse>
                </soapenv:Body>
            </soapenv:Envelope>
        ');

        return response($data, 200, [
            'Content-Type' => 'application/xml',
        ]);
    }
}
