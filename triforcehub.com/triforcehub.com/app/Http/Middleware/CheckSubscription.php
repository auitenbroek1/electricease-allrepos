<?php

namespace App\Http\Middleware;

use Closure;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Auth;
use Symfony\Component\HttpFoundation\Response;

class CheckSubscription
{
    public function handle(Request $request, Closure $next): Response
    {
        $user = $request->user();

        if (! $user->member->enabled) {
            try {
                $customer = $user->member->customer;

                if (is_null($customer)) {
                    throw new \Exception('No subscription found.');
                }

                $stripe = new \Stripe\StripeClient(env('STRIPE_SECRET'));

                $session = $stripe->billingPortal->sessions->create([
                    'customer' => $customer,
                ]);

                Auth::logout();

                return redirect($session->url);
            } catch (\Exception $e) {

            }

            Auth::logout();

            session()->flash('error', 'No subscription found.');

            return redirect('/auth/check');
        }

        return $next($request);
    }
}
