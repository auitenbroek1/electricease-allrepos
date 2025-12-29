<?php

namespace App\Http\Controllers;

use App\Models\Distributor;
use App\Models\Member;
use Exception;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\DB;
use Src\Prices\Support\EchoGroup;
use Src\Prices\Support\VanMeter;

class PriceController extends Controller
{
    public function national(Request $request)
    {
        $payload = $request->all();
        $prices = [];

        // return [];

        //

        foreach ($payload as $item) {
            if (count($item['upcs']) === 0) {
                $prices[] = [
                    'key' => $item['key'],
                    'price' => round(0, 2),
                ];

                continue;
            }

            // TODO: account for multiple!
            $upc = $item['upcs'][0];
            $quantity = $item['quantity'];

            $cost = DB::table('prices')
                ->join('part_upcs', 'part_upcs.id', 'prices.part_upc_id')
                ->where('part_upcs.name', $upc)
                ->where('prices.cost', '>', 0)
                ->whereNot('prices.cost', null)
                ->max('cost');

            $prices[] = [
                'key' => $item['key'],
                'price' => round($cost, 2),
            ];
        }

        // ray($prices);

        return $prices;
    }

    public function distributor(Request $request, Distributor $distributor)
    {
        // region

        $distributor = auth()->user()->member->distributors()->where('distributor_id', $distributor->id)->first();

        if (! $distributor) {
            throw (new Exception('invalid distributor'));
        }

        // endregion

        // region

        $username = $distributor->pivot['username'];
        $password = $distributor->pivot['password'];
        $customer = $distributor->pivot['customer'];
        $enabled = $distributor->pivot['enabled'];

        if (! $enabled) {
            throw (new Exception('invalid status'));
        }

        // endregion

        // region

        $api = null;

        if ($distributor->id === 1) {
            $api = new EchoGroup(
                username: $username,
                password: $password,
                customerID: $customer,
            );
        } elseif ($distributor->id === 2) {
            $api = new VanMeter(
                username: $username,
                password: $password,
                customerID: $customer,
            );
        } elseif ($distributor->id === 3) {
            $member = Member::find(1);

            $username = $member->distributors()->where('distributor_id', 2)->first()->pivot['username'];
            $password = $member->distributors()->where('distributor_id', 2)->first()->pivot['password'];
            $customer = $member->distributors()->where('distributor_id', 2)->first()->pivot['customer'];

            $api = new VanMeter(
                username: $username,
                password: $password,
                customerID: $customer,
            );
        }

        if (! $api) {
            throw (new Exception('invalid api'));
        }

        // endregion

        $payload = $request->all();
        $prices = [];

        // return [];

        //

        foreach ($payload as $item) {
            if (count($item['upcs']) === 0) {
                $prices[] = [
                    'key' => $item['key'],
                    'price' => round(0, 2),
                ];

                continue;
            }

            // TODO: account for multiple!
            $upc = $item['upcs'][0];
            $quantity = $item['quantity'];

            $cost = $api->check($upc, $quantity);

            if ($distributor->id === 1) {
                // EchoGroup
            } elseif ($distributor->id === 2) {
                // VanMeter
            } elseif ($distributor->id === 3) {
                // 3E
                $cost = $cost * 1.05; // 5% markup
            }

            $prices[] = [
                'key' => $item['key'],
                'price' => round($cost, 2),
            ];
        }

        // ray($prices);

        return $prices;
    }
}
