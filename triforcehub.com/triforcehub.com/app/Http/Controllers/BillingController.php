<?php

namespace App\Http\Controllers;

use App\Models\User;
use Illuminate\Http\RedirectResponse;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;

class BillingController extends Controller
{
    public function __invoke(Request $request): RedirectResponse
    {
        $user = Auth::user();
        if ($target_user_id = session('target_user_id')) {
            $user = User::find($target_user_id);
        }

        $customer = $user->member->customer;

        throw_if(is_null($customer), \Exception::class, "No subscription found for {$user->email}.");

        //

        $stripe = new \Stripe\StripeClient(env('STRIPE_SECRET'));

        $session = $stripe->billingPortal->sessions->create([
            'customer' => $customer,
            // 'configuration' => 'bpc_1PqjsXCuJm3ct3GoqaBVX7Uy',
        ]);

        return redirect($session->url);
    }
}
