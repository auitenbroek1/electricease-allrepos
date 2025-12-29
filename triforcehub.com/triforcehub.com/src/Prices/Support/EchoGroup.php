<?php

namespace Src\Prices\Support;

use Exception;
use Illuminate\Support\Facades\Cache;
use Illuminate\Support\Facades\Http;

class EchoGroup
{
    // region

    private string $base = 'http://eclp.echogroupinc.com:5000';

    private string $customerID;

    private string $password;

    private string $token;

    private string $username;

    // endregion

    public function __construct(
        string $username,
        string $password,
        string $customerID,
    ) {
        if (! $this->username = $username) {
            throw (new Exception('invalid username'));
        }

        if (! $this->password = $password) {
            throw (new Exception('invalid password'));
        }

        if (! $this->customerID = $customerID) {
            throw (new Exception('invalid customerID'));
        }
    }

    private function authenticate(

    ): void {
        $payload = [
            'username' => $this->username,
            'password' => $this->password,
            'logintype' => 'Customer',
        ];

        $key = md5(serialize($payload));
        $ttl = now()->addMinutes(5);

        if (Cache::has($key)) {
            // ray('token in cache');
        } else {
            // ray('need NEW token');
        }

        $this->token = Cache::remember($key, $ttl, function () use ($payload) {
            $url = $this->base.'/Sessions';
            $response = Http::post($url, $payload);

            if (! $response->ok()) {
                throw (new Exception('authenticate status '.$response->status()));
            }

            $json = $response->json();

            if (! $token = $json['sessionToken'] ?? null) {
                throw (new Exception('missing session token'));
            }

            return $token;
        });
    }

    public function check(
        string $upc,
        float $quantity,
    ): float {
        $this->authenticate();

        // ray('token', $this->token);
        // throw(new Exception('oops'));
        // return 0;

        $payload = [
            'CustomerId' => $this->customerID,
            'UPCCode' => $upc,
            'Quantity' => ceil($quantity),
        ];

        $response = Http::withToken($this->token, 'SessionToken')
            ->get($this->base.'/ProductPricingInquiry', $payload);

        // ray($response->json());

        $price = 0;

        if ($response->ok()) {
            $json = $response->json();
            // ray($json);
            $pricingPerQuantity = $json['pricingPerQuantity'];
            $productUnitPrice = $json['productUnitPrice']['value'];
            // ray($pricingPerQuantity, $productUnitPrice);
            $price = bcdiv($productUnitPrice, $pricingPerQuantity, 6);
            // ray($price);
        }

        return round($price, 6);
    }
}
