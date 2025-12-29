<?php

namespace App\Http\Controllers;

use App\Jobs\SetupJumpStart;
use App\Mail\SubscriptionCreated;
use App\Models\Member;
use App\Models\State;
use App\Models\User;
use Illuminate\Http\JsonResponse;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\Mail;
use Illuminate\Support\Str;
use Throwable;

class StripeController extends Controller
{
    public function __invoke(Request $request): JsonResponse
    {
        try {
            \Stripe\Stripe::setApiKey(env('STRIPE_SECRET'));

            $payload = @file_get_contents('php://input');
            $sig_header = $_SERVER['HTTP_STRIPE_SIGNATURE'] ?? null;
            $secret = 'whsec_WVkbXz19Wb8XTcvnllVVU4viFasJtKRz';

            $event = null;

            if ($sig_header) {
                $event = \Stripe\Webhook::constructEvent(
                    $payload,
                    $sig_header,
                    $secret
                );
            } else {
                $event = \Stripe\Event::constructFrom(
                    json_decode($payload, true)
                );
            }

            switch ($event->type) {
                case 'checkout.session.completed':
                    $this->process($event);

                    break;
                case 'customer.subscription.created':
                    $this->customer_subscription_created($event);

                    break;
                case 'invoice.paid':
                    $this->enable_member($event);

                    break;
                case 'customer.subscription.deleted':
                case 'invoice.payment_failed':
                    $this->disable_member($event);

                    break;
                default:
                    break;
            }

            return response()->json([
                'type' => $event->type,
            ]);
        } catch (Throwable $e) {
            report($e);

            return response()->json(['error' => $e->getMessage()], 500);
        }
    }

    private function process($event)
    {
        $object = $event->data->object;

        // region subscription

        $subscription_id = $object->subscription;
        // $subscription_id = 'sub_1OrSXnCuJm3ct3GofNcWcDZF'; // Hire Electrical Co Inc
        $subscription = \Stripe\Subscription::retrieve($subscription_id);

        // ray($subscription->toJSON());

        // endregion

        // region product

        $product_id = $subscription->plan->product;
        // $product_id = 'prod_PzXC4QxsMCQVJL'; // Apprentice Yearly Plan
        // $product_id = 'prod_PzXBKLNs0lMWp5'; // Apprentice Monthly Plan
        // $product_id = 'prod_PtvLyNJIGgVdal'; // Journeyman Annual Plan
        // $product_id = 'prod_PtvL0W469VzUFB'; // Journeyman Monthly Plan
        $product = \Stripe\Product::retrieve($product_id);

        // ray($product->toJSON());

        // endregion

        // region features

        $feature_data = [
            'feature_digital_takeoff_enabled' => true,
            'feature_auto_count_enabled' => true,
            'feature_jumpstart' => true,
        ];

        // ray($feature_data);

        // endregion

        // region customer

        $customer_id = $object->customer;
        // $customer_id = 'cus_PgqEA7qnAIEDDP';
        $customer = \Stripe\Customer::retrieve($customer_id);

        // ray($customer->toJSON());

        // endregion

        // region custom fields

        $custom_fields = collect($object->custom_fields)->map(function ($item) {
            $key = $item->key;
            $value = $item->text->value;

            return [
                $key => $value,
            ];
        })->keyBy(function ($item) {
            return key($item);
        })->map(function ($item) {
            return current($item);
        })->toArray();

        // ray($custom_fields);

        // endregion

        // region member

        $member_data = [
            'customer' => $customer_id,
            'name' => $custom_fields['companyname'] ?? $customer->name,
            'email' => $customer->email,
            'address1' => $customer->address->line1,
            'address2' => $customer->address->line2,
            'city' => $customer->address->city,
            'state_id' => State::where('abbreviation', $customer->address->state)->first()->id,
            'zip' => $customer->address->postal_code,
            'office' => Str::substr($customer->phone, -10),
            ...$feature_data,
        ];

        // ray($member_data);

        // endregion

        // region user

        $password = Str::random(18);

        $user_data = [
            'member_id' => null,
            'name' => $customer->name,
            'email' => $customer->email,
            'password' => Hash::make($password),
        ];

        // ray($user_data);

        // endregion

        // region save

        DB::beginTransaction();

        try {
            $member = Member::create($member_data);

            // ray($member);

            $user_data['member_id'] = $member->id;

            $user = User::create($user_data);

            // ray($user);

            DB::commit();

            if ($feature_data['feature_jumpstart']) {
                SetupJumpStart::dispatch($member->id);
            }

            Mail::to($user->email)
                ->bcc('bruno@electric-ease.com')
                ->send(new SubscriptionCreated($user->name, $user->email, $password));
        } catch (Throwable $e) {
            DB::rollback();

            throw $e;
        }

        // endregion
    }

    private function customer_subscription_created($event)
    {
        $object = $event->data->object;

        // region subscription

        $subscription = $object;

        // ray($subscription);

        // endregion

        // region product

        $product_id = $subscription->plan->product;
        $product = \Stripe\Product::retrieve($product_id);

        // ray($product);

        // endregion

        // region features

        $feature_data = [
            'feature_digital_takeoff_enabled' => true,
            'feature_auto_count_enabled' => true,
            'feature_jumpstart' => true,
        ];

        // ray($feature_data);

        // endregion

        // region customer

        $customer_id = $object->customer;
        $customer = \Stripe\Customer::retrieve($customer_id);

        // ray($customer);

        // endregion

        // region custom fields

        $custom_fields = collect($object->custom_fields)->map(function ($item) {
            $key = $item->key;
            $value = $item->text->value;

            return [
                $key => $value,
            ];
        })->keyBy(function ($item) {
            return key($item);
        })->map(function ($item) {
            return current($item);
        })->toArray();

        // ray($custom_fields);

        // endregion

        // region member

        $member_data = [
            'customer' => $customer_id,
            'name' => $custom_fields['companyname'] ?? $customer->name,
            'email' => $customer->email,
            'address1' => $customer->address->line1 ?? '',
            'address2' => $customer->address->line2 ?? '',
            'city' => $customer->address->city ?? '',
            'state_id' => State::where('abbreviation', $customer->address->state ?? 'AL')->first()->id,
            'zip' => $customer->address->postal_code ?? '',
            'office' => Str::substr($customer->phone, -10) ?? '',
            ...$feature_data,
        ];

        // ray($member_data);

        // endregion

        // region user

        $password = Str::random(18);

        $user_data = [
            'member_id' => null,
            'name' => $customer->name,
            'email' => $customer->email,
            'password' => Hash::make($password),
        ];

        // ray($user_data);

        // endregion

        // region save

        DB::beginTransaction();

        try {
            $member = Member::create($member_data);

            ray($member);

            $user_data['member_id'] = $member->id;

            $user = User::create($user_data);

            ray($user);

            DB::commit();

            if ($feature_data['feature_jumpstart']) {
                SetupJumpStart::dispatch($member->id);
            }

            Mail::to($user->email)
                ->bcc('bruno@electric-ease.com')
                ->send(new SubscriptionCreated($user->name, $user->email, $password));
        } catch (Throwable $e) {
            DB::rollback();

            throw $e;
        }

        // endregion
    }

    private function enable_member($event)
    {
        $object = $event->data->object;
        $customer_id = $object->customer;

        $member = Member::where('customer', $customer_id)->first();

        // ray($member);

        if ($member && ! $member->enabled) {
            $member->enabled = true;
            $member->save();
        }
    }

    private function disable_member($event)
    {
        $object = $event->data->object;
        $customer_id = $object->customer;

        $member = Member::where('customer', $customer_id)->first();

        // ray($member);

        if ($member) {
            $member->enabled = false;
            $member->save();
        }
    }
}
