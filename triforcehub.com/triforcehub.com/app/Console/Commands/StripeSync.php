<?php

namespace App\Console\Commands;

use App\Models\Member;
use Illuminate\Console\Command;
use Illuminate\Support\Str;

class StripeSync extends Command
{
    protected $signature = 'stripe:sync';

    protected $description = 'Sync Stripe Customers with Members';

    public function handle(): int
    {
        \Stripe\Stripe::setApiKey(env('STRIPE_SECRET'));

        $customers = $this->fetch();

        Command::info('');

        $this->sync($customers);

        return Command::SUCCESS;
    }

    private function fetch($limit = 50)
    {
        Command::info('fetch...');

        $emails = [];
        $customers = [];
        $starting_after = null;

        do {
            $params = [
                'limit' => $limit,
                'expand' => ['data.subscriptions'],
            ];

            if ($starting_after) {
                $params['starting_after'] = $starting_after;
            }

            Command::info('params: '.json_encode($params));

            $customer_collection = \Stripe\Customer::all($params);

            foreach ($customer_collection->data as $customer) {
                $subscriptions = collect($customer->subscriptions->data)->map(function ($subscription) {
                    return [
                        'id' => $subscription->id,
                        'status' => $subscription->status,
                        'created' => $subscription->created,
                    ];
                })->toArray();

                $email = Str::lower($customer->email);
                $emails[] = $email;

                if (! isset($customers[$email])) {
                    $customers[$email] = [
                        'id' => $customer->id,
                        'email' => $email,
                        'created' => $customer->created,
                        'subscriptions' => $subscriptions,
                    ];
                } else {
                    $customers[$email]['subscriptions'] = array_merge($customers[$email]['subscriptions'], $subscriptions);
                }
            }

            $starting_after = end($customers)['id'];
        } while ($customer_collection->has_more);

        $customers = collect($customers)->map(function ($customer) {
            $customer['enabled'] = collect($customer['subscriptions'])->filter(function ($subscription) {
                return in_array($subscription['status'], ['active', 'trialing']);
            })->count() > 0;

            return $customer;
        })->toArray();

        Command::info('found: '.count($emails));
        Command::info('unique: '.count($customers));
        Command::info('done...');

        return $customers;
    }

    private function sync($customers, $chunk_size = 500)
    {
        $this->info('sync...');

        $total_skipped = 0;
        $total_processed = 0;
        $total_enabled = 0;
        $total_disabled = 0;

        Member::chunk($chunk_size, function ($members) use ($customers, &$total_skipped, &$total_processed, &$total_enabled, &$total_disabled) {
            $this->info('found: '.count($members));

            foreach ($members as $member) {
                if ($this->should_skip($member)) {
                    $total_skipped++;

                    continue;
                }

                $total_processed++;

                $customer = collect($customers)->filter(function ($customer) use ($member) {
                    return Str::of($customer['email'])->lower() == Str::of($member->email)->lower();
                })->first();

                $current_status = $member->enabled ? true : false;

                if ($customer) {
                    $new_status = $customer['enabled'] ? true : false;

                    if ($current_status !== $new_status) {
                        if ($new_status) {
                            $total_enabled++;
                            $this->info("enable: {$member->email} ({$member->id}: {$member->name}) - active in stripe");
                        } else {
                            $member->enabled = false;
                            $member->save();

                            $total_disabled++;
                            $this->warn("disable: {$member->email} ({$member->id}: {$member->name}) - inactive in stripe");
                        }
                    }
                } else {
                    if ($current_status) {
                        // $member->enabled = false;
                        // $member->save();

                        $total_disabled++;
                        $this->warn("disable: {$member->email} ({$member->id}: {$member->name}) - not found in stripe");
                    }
                }
            }
        });

        $this->info("skipped: $total_skipped");
        $this->info("processed: $total_processed");
        $this->info("enabled: $total_enabled");
        $this->info("disabled: $total_disabled");
        $this->info('done');
    }

    private function should_skip($member)
    {
        $allow_list = [1, 2, 55, 345, 380];

        if (in_array($member->id, $allow_list)) {
            return true;
        }

        return false;
    }
}
