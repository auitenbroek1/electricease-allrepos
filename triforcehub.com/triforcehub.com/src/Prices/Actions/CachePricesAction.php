<?php

namespace Src\Prices\Actions;

use App\Models\Member;
use Exception;
use Illuminate\Support\Facades\DB;
use Src\Prices\Models\Price;
use Src\Prices\Support\EchoGroup;
use Src\Prices\Support\VanMeter;

class CachePricesAction
{
    /*

    When we look up 032886239038 for Custom Electric (97942) it returns 0.
    For Armor Electric (89953) the correct price is returned.

    */

    public function __invoke(
        int $member_id,
        int $distributor_id,
        int $limit,
        int $chunk
    ): array {
        // region

        $member = Member::find($member_id);

        if (! $member) {
            throw (new Exception('invalid member'));
        }

        $distributor = $member->distributors()->where('distributor_id', $distributor_id)->first();

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

        if ($distributor_id === 1) {
            $api = new EchoGroup(
                username: $username,
                password: $password,
                customerID: $customer,
            );
        } elseif ($distributor_id === 2) {
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

        $log = [];

        $candidates = DB::table('part_upcs')
            ->select('part_upcs.id', 'part_upcs.name')
            ->leftJoin('prices', function ($join) use ($member_id, $distributor_id) {
                $join
                    ->on('prices.member_id', DB::raw($member_id))
                    ->on('prices.distributor_id', DB::raw($distributor_id))
                    ->on('prices.part_upc_id', 'part_upcs.id');
            })
            ->where(function ($query) {
                return $query
                    ->where('prices.updated_at', null)
                    ->orWhere('prices.updated_at', '<', now()->subDays(1));
            })
            ->limit($limit);

        $log[] = 'found: '.$candidates->count();

        $batches = $candidates->get()->chunk($chunk);

        foreach ($batches as $batch) {
            // $log[] = 'batch start';
            foreach ($batch as $item) {
                $cost = $api->check($item->name, 1);
                // $log[] = $item->name.': '.$cost;
                Price::updateOrCreate([
                    'member_id' => $member_id,
                    'distributor_id' => $distributor_id,
                    'part_upc_id' => $item->id,
                ], [
                    'cost' => $cost,
                ]);
            }
            // $log[] = 'batch finish';
            sleep(1);
        }

        return $log;
    }
}
