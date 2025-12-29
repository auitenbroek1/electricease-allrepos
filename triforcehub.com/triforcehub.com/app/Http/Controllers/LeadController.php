<?php

namespace App\Http\Controllers;

use App\Mail\LeadSubmitted;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Log;
use Illuminate\Support\Facades\Mail;
use Illuminate\Support\Str;
use Throwable;

class LeadController extends Controller
{
    public function __invoke(Request $request)
    {
        // region

        $payload = $request->getContent();

        Log::info($payload);

        try {
            $data = json_decode($payload);

            // ray($data);

            $name = (string) $data->name;
            $email = (string) $data->email;
            $phone = (string) $data->phone;
            $company = (string) $data->company;
            $sms = (string) $data->sms;
        } catch (Throwable $e) {
            // ray($e);

            return $this->response();
        }

        // endregion

        // region

        if (! Str::length($name)) {
            // ray('missing name');

            return $this->response();
        }

        if (! Str::length($email)) {
            // ray('missing email');

            return $this->response();
        }

        if ($phone) {
            $phone = preg_replace('/[^0-9]/', '', $phone);
        }

        if (! Str::length($phone)) {
            // ray('missing phone');

            return $this->response();
        }

        if (! Str::length($company)) {
            // ray('missing company');

            return $this->response();
        }

        // ray($name);
        // ray($email);
        // ray($phone);
        // ray($company);
        // ray($sms);

        Mail::to('sales@electric-ease.com')
            ->cc('john@electric-ease.com')
            ->cc('kevin@electric-ease.com')
            ->bcc('bruno@electric-ease.com')
            ->send(new LeadSubmitted($name, $email, $phone, $company, $sms));

        // endregion

        return $this->response();
    }

    private function response()
    {
        $data = [
            'data' => 'done',
        ];

        return response($data, 200, [
            'Content-Type' => 'application/json',
        ]);
    }
}
