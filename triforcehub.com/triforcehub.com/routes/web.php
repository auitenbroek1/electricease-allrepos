<?php

use App\Http\Controllers\BillingController;
use App\Http\Controllers\CheckSubscriptionController;
use App\Http\Controllers\FreeTrialController;
use App\Http\Controllers\JobReportDownloadController;
use App\Http\Controllers\LeadController;
use App\Http\Controllers\StripeController;
use App\Http\Middleware\CheckSubscription;
use App\Mail\SubscriptionCreated;
use App\Models\Job;
use App\Models\JobCustomer;
use Illuminate\Support\Facades\Http;
use Illuminate\Support\Facades\Redirect;
use Illuminate\Support\Facades\Route;
use Src\Bids\Actions\GenerateMaterialSummaryAttachmentHtmlAction;
use Src\Bids\Actions\GenerateProposalAttachmentHtmlAction;
use Src\Bids\Actions\GenerateWorkOrderAttachmentHtmlAction;
use Src\Bids\Controllers\DownloadMaterialSummaryReportController;
use Src\Bids\Mail\SendProposal;
use Src\Bids\Mail\SendWorkOrder;
use Src\External\Controllers\ExternalController;

Route::get('/', function () {
    return Redirect::to('/app');
});

Route::get('/auth/check', CheckSubscriptionController::class)->name('auth.check');

Route::middleware(['auth', CheckSubscription::class])->group(function () {
    Route::prefix('app')->group(function () {
        Route::get('/', function () {
            return view('app');
        });

        Route::get('/billing', BillingController::class);

        Route::fallback(function () {
            return view('app');
        });
    });

    Route::prefix('downloads')->group(function () {
        Route::get('/jobs/{job}/reports', JobReportDownloadController::class);
        Route::get('/jobs/{job}/reports/material-summary/{extension}', DownloadMaterialSummaryReportController::class);
    });

    Route::prefix('emails')->group(function () {
        Route::get('welcome', function () {
            return new SubscriptionCreated('Company Name', 'member@test.com', 'test');
        });

        // proposal

        Route::get('/jobs/{job}/{customer}/proposal', function (Job $job, JobCustomer $customer) {
            return new SendProposal(
                job: $job,
                customer: $customer,
                subject: '',
                attachments: [],
            );
        });

        Route::get('/jobs/{job}/{customer}/proposal/attachments/proposal', function (Job $job, JobCustomer $customer) {
            return (new GenerateProposalAttachmentHtmlAction)($job, $customer);
        });

        Route::get('/jobs/{job}/{customer}/proposal/attachments/material-summary', function (Job $job) {
            return (new GenerateMaterialSummaryAttachmentHtmlAction)($job);
        });

        // work order

        Route::get('/jobs/{job}/{customer}/work-order', function (Job $job, JobCustomer $customer) {
            return new SendWorkOrder(
                job: $job,
                customer: $customer,
                subject: '',
                attachments: [],
            );
        });

        Route::get('/jobs/{job}/{customer}/work-order/attachments/work-order', function (Job $job, JobCustomer $customer) {
            return (new GenerateWorkOrderAttachmentHtmlAction)($job, $customer);
        });

        Route::get('/jobs/{job}/{customer}/work-order/attachments/material-summary', function (Job $job) {
            return (new GenerateMaterialSummaryAttachmentHtmlAction)($job);
        });
    });

    Route::prefix('test')->group(function () {
        Route::get('ip', function () {
            $response = Http::get('https://api.ipify.org/?format=json');

            return $response->json();
        });

        Route::get('/upc/{upc}', function ($upc) {
            // auth
            $response = Http::post('http://eclp.echogroupinc.com:5000/Sessions', [
                'username' => '50478',
                'password' => 'ECHO108508',
                'logintype' => 'Customer',
            ]);
            // dump($response);
            $data = $response->json();
            // dump($data);
            $token = $data['sessionToken'];

            // lookup single

            print_r('ProductPricingInquiry');
            $response = Http::withToken($token, 'SessionToken')
                ->get('http://eclp.echogroupinc.com:5000/ProductPricingInquiry', [
                    'CustomerId' => '50478',
                    'UPCCode' => $upc,
                ]);
            $json = $response->json();
            dump($json);

            print_r('ProductInventoryPricingInquiry');
            $response = Http::withToken($token, 'SessionToken')
                ->get('http://eclp.echogroupinc.com:5000/ProductInventoryPricingInquiry', [
                    'CustomerId' => '50478',
                    'UPCCode' => $upc,
                ]);
            $json = $response->json();
            dump($json);

            // lookup entity

            print_r('BasicInformation');
            $response = Http::withToken($token, 'SessionToken')
                ->get('http://eclp.echogroupinc.com:5000/Products/BasicInformation', [
                    'keyword' => $upc,
                    // 'keyword' => 'screw'
                ]);
            $json = $response->json();
            dump($json);
            // return;

            print_r('EntitySearch');
            $response = Http::withToken($token, 'SessionToken')
                ->get('http://eclp.echogroupinc.com:5000/EntitySearch', [
                    'keyword' => $upc,
                    // 'keyword' => 'screw'
                ]);
            $json = $response->json();
            dump($json);

            return;

            // mass lookup

            $upcs = [
                $upc,
                // '074983574742',
                // '7498357474',
                // '07498357474',

                // '3288685139',
                // '03288685139',
                // '032886851391',

                // '78101101675',
            ];

            $upcs_qs = collect($upcs)->map(function ($item) {
                return 'UPCCode='.$item;
            })->join('&');
            $response = Http::withToken($token, 'SessionToken')
                ->get('http://eclp.echogroupinc.com:5000/ProductPricingMassInquiry?CustomerId=50478&'.$upcs_qs);

            // dump($response);
            // dump($response->body());
            $json2 = $response->json();
            dump($json2['results']);
        });
    });
});

Route::prefix('external')->group(function () {
    Route::get('bids', [ExternalController::class, 'bids']);
    Route::get('bids/{bid_id}/customers', [ExternalController::class, 'customers'])->whereNumber('bid_id');
    Route::post('bids/{bid_id}/customers', [ExternalController::class, 'customers_create'])->whereNumber('bid_id');
    Route::delete('bids/{bid_id}/customers/{customer_id}', [ExternalController::class, 'customers_delete'])->whereNumber(['bid_id', 'customer_id']);
});

Route::prefix('webhooks')->group(function () {
    Route::post('free-trial', FreeTrialController::class);
    Route::post('leads', LeadController::class);
    Route::post('stripe', StripeController::class);
});

if (app()->environment('local')) {
    require __DIR__.'/local.php';
}
