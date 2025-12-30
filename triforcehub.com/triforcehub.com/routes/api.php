<?php

use App\Http\Controllers\AssemblyCategoryController;
use App\Http\Controllers\AssemblyController;
use App\Http\Controllers\AssemblyFavoriteController;
use App\Http\Controllers\AssemblyTagController;
use App\Http\Controllers\ImpersonationController;
use App\Http\Controllers\JobAdjustmentController;
use App\Http\Controllers\JobAnnotationController;
use App\Http\Controllers\JobAssemblyController;
use App\Http\Controllers\JobBlockController;
use App\Http\Controllers\JobController;
use App\Http\Controllers\JobCrewController;
use App\Http\Controllers\JobCustomerController;
use App\Http\Controllers\JobExpenseController;
use App\Http\Controllers\JobLaborController;
use App\Http\Controllers\JobLocationController;
use App\Http\Controllers\JobMaterialPriceController;
use App\Http\Controllers\JobPartController;
use App\Http\Controllers\JobPhaseController;
use App\Http\Controllers\JobPlanController;
use App\Http\Controllers\JobQuoteController;
use App\Http\Controllers\JobReports;
use App\Http\Controllers\MemberController;
use App\Http\Controllers\PartCategoryController;
use App\Http\Controllers\PartController;
use App\Http\Controllers\PartFavoriteController;
use App\Http\Controllers\PartTagController;
use App\Http\Controllers\PartUPCController;
use App\Http\Controllers\PriceController;
use App\Http\Controllers\SymbolController;
use App\Http\Controllers\UpdateMemberFeatures;
use App\Http\Controllers\UploadController;
use App\Http\Controllers\UserController;
use Illuminate\Support\Facades\Route;
use Src\Bids\Controllers\SendProposalEmailController;
use Src\Bids\Controllers\SendWorkOrderEmailController;
use Src\Copilot\Controllers\CopilotController;
use Src\Dashboard\Controllers\DashboardController;

Route::middleware(['auth:sanctum'])->group(function () {
    Route::get('/dashboard', DashboardController::class);

    Route::get('/assemblies', [AssemblyController::class, 'index']);
    Route::post('/assemblies', [AssemblyController::class, 'store']);
    Route::get('/assemblies/{assembly}', [AssemblyController::class, 'show'])->whereNumber('assembly');
    Route::patch('/assemblies/{assembly}', [AssemblyController::class, 'update'])->whereNumber('assembly');
    Route::delete('/assemblies/{assembly}', [AssemblyController::class, 'destroy'])->whereNumber('assembly');
    Route::post('/assemblies/{assembly}/duplicate', [AssemblyController::class, 'duplicate'])->whereNumber('assembly');
    Route::post('/assemblies/{assembly}/favorites', [AssemblyFavoriteController::class, 'store'])->whereNumber('assembly');
    Route::delete('/assemblies/{assembly}/favorites', [AssemblyFavoriteController::class, 'destroy'])->whereNumber('assembly');

    Route::get('/assemblies/categories', [AssemblyCategoryController::class, 'index']);
    Route::post('/assemblies/categories', [AssemblyCategoryController::class, 'store']);
    Route::get('/assemblies/categories/{category}', [AssemblyCategoryController::class, 'show'])->whereNumber('category');
    Route::patch('/assemblies/categories/{category}', [AssemblyCategoryController::class, 'update'])->whereNumber('category');
    Route::delete('/assemblies/categories/{category}', [AssemblyCategoryController::class, 'destroy'])->whereNumber('category');

    Route::get('/assemblies/tags', [AssemblyTagController::class, 'index']);
    Route::post('/assemblies/tags', [AssemblyTagController::class, 'store']);
    Route::get('/assemblies/tags/{tag}', [AssemblyTagController::class, 'show'])->whereNumber('tag');
    Route::patch('/assemblies/tags/{tag}', [AssemblyTagController::class, 'update'])->whereNumber('tag');
    Route::delete('/assemblies/tags/{tag}', [AssemblyTagController::class, 'destroy'])->whereNumber('tag');

    //

    Route::post('/distributors/0/prices', [PriceController::class, 'national']);
    Route::post('/distributors/{distributor}/prices', [PriceController::class, 'distributor']);
    Route::post('/jobs/{job}/prices', JobMaterialPriceController::class);

    //

    Route::get('/jobs', [JobController::class, 'index'])->middleware('gzip');
    Route::post('/jobs', [JobController::class, 'store']);
    Route::get('/jobs/{job}', [JobController::class, 'show'])->whereNumber('job')->middleware('gzip');
    Route::patch('/jobs/{job}', [JobController::class, 'update'])->whereNumber('job')->middleware('gzip');
    Route::delete('/jobs/{job}', [JobController::class, 'destroy'])->whereNumber('job');
    Route::get('/jobs/{job}/summary', [JobController::class, 'summary'])->whereNumber('job');
    Route::patch('/jobs/{job}/settings', [JobController::class, 'updateSettings'])->whereNumber('job');
    Route::post('/jobs/{job}/duplicate', [JobController::class, 'duplicate'])->whereNumber('job');

    Route::post('/jobs/{job}/{customer}/proposal', SendProposalEmailController::class);
    Route::post('/jobs/{job}/{customer}/work-order', SendWorkOrderEmailController::class);
    Route::get('/jobs/{job}/reports', JobReports::class);

    Route::controller(JobAdjustmentController::class)->group(function () {
        Route::post('/jobs/adjustments', 'store');
        Route::patch('/jobs/adjustments/{adjustment}', 'update')->whereNumber('adjustment');
        Route::delete('/jobs/adjustments/{adjustment}', 'destroy')->whereNumber('adjustment');
    });

    Route::controller(JobAnnotationController::class)->group(function () {
        Route::post('/jobs/annotations', 'store');
        Route::patch('/jobs/annotations/{annotation:uuid}', 'update')->whereUuid('annotation');
        Route::delete('/jobs/annotations/{annotation:uuid}', 'destroy')->whereUuid('annotation');
        Route::post('/jobs/annotations/bulk/destroy', 'bulk_destroy');
    });

    Route::post('/jobs/assemblies', [JobAssemblyController::class, 'store']);
    Route::patch('/jobs/assemblies/{assembly}', [JobAssemblyController::class, 'update'])->whereNumber('assembly');
    Route::patch('/jobs/assemblies/{assembly}/partial', [JobAssemblyController::class, 'update_partial'])->whereNumber('assembly');
    Route::delete('/jobs/assemblies/{assembly}', [JobAssemblyController::class, 'destroy'])->whereNumber('assembly');
    Route::post('/jobs/assemblies/enabled', [JobAssemblyController::class, 'enabled']);

    Route::controller(JobBlockController::class)->group(function () {
        Route::post('/jobs/blocks', 'store');
        Route::patch('/jobs/blocks/{block}', 'update')->whereNumber('block');
        Route::delete('/jobs/blocks/{block}', 'destroy')->whereNumber('block');

        Route::post('/jobs/blocks/sort', 'sort');
    });

    Route::controller(JobCrewController::class)->group(function () {
        Route::post('/jobs/crews', 'store');
        Route::patch('/jobs/crews/{crew}', 'update')->whereNumber('crew');
        Route::delete('/jobs/crews/{crew}', 'destroy')->whereNumber('crew');
    });

    Route::controller(JobCustomerController::class)->group(function () {
        Route::post('/jobs/customers', 'store');
        Route::patch('/jobs/customers/{customer}', 'update')->whereNumber('customer');
        Route::delete('/jobs/customers/{customer}', 'destroy')->whereNumber('customer');
    });

    Route::post('/jobs/expenses', [JobExpenseController::class, 'store']);
    Route::patch('/jobs/expenses/{expense}', [JobExpenseController::class, 'update'])->whereNumber('expense');
    Route::delete('/jobs/expenses/{expense}', [JobExpenseController::class, 'destroy'])->whereNumber('expense');

    Route::post('/jobs/labor', [JobLaborController::class, 'store']);
    Route::patch('/jobs/labor/{labor}', [JobLaborController::class, 'update'])->whereNumber('labor');
    Route::delete('/jobs/labor/{labor}', [JobLaborController::class, 'destroy'])->whereNumber('labor');

    Route::controller(JobLocationController::class)->group(function () {
        Route::post('/jobs/locations', 'store');
        Route::patch('/jobs/locations/{location}', 'update')->whereNumber('location');
        Route::delete('/jobs/locations/{location}', 'destroy')->whereNumber('location');
    });

    Route::post('/jobs/parts', [JobPartController::class, 'store']);
    Route::patch('/jobs/parts/{part}', [JobPartController::class, 'update'])->whereNumber('part');
    Route::patch('/jobs/parts/{part}/partial', [JobPartController::class, 'update_partial'])->whereNumber('part');
    Route::delete('/jobs/parts/{part}', [JobPartController::class, 'destroy'])->whereNumber('part');
    Route::post('/jobs/parts/enabled', [JobPartController::class, 'enabled']);

    Route::post('/jobs/phases', [JobPhaseController::class, 'store']);
    Route::patch('/jobs/phases/{phase}', [JobPhaseController::class, 'update'])->whereNumber('phase');
    Route::delete('/jobs/phases/{phase}', [JobPhaseController::class, 'destroy'])->whereNumber('phase');
    Route::post('/jobs/phases/{phase}/duplicate', [JobPhaseController::class, 'duplicate'])->whereNumber('phase');

    Route::controller(JobPlanController::class)->group(function () {
        Route::get('/jobs/{job}/plans', 'index')->whereNumber(['job']);
        Route::post('/jobs/{job}/plans', 'store')->whereNumber(['job']);
        Route::get('/jobs/{job}/plans/{plan}', 'show')->whereNumber(['job', 'plan'])->middleware('gzip');
        Route::patch('/jobs/{job}/plans/{plan}', 'update')->whereNumber(['job', 'plan']);
        Route::delete('/jobs/{job}/plans/{plan}', 'destroy')->whereNumber(['job', 'plan']);

        Route::get('/jobs/{job}/plans/{plan}/annotations', 'annotations')->whereNumber(['job', 'plan'])->middleware('gzip');
        Route::post('/jobs/{job}/plans/{plan}/update_quantities', 'update_quantities')->whereNumber(['job', 'plan']);
    });

    Route::post('/jobs/quotes', [JobQuoteController::class, 'store']);
    Route::patch('/jobs/quotes/{quote}', [JobQuoteController::class, 'update'])->whereNumber('quote');
    Route::delete('/jobs/quotes/{quote}', [JobQuoteController::class, 'destroy'])->whereNumber('quote');

    //

    Route::get('/members', [MemberController::class, 'index']);
    Route::post('/members', [MemberController::class, 'store']);
    Route::get('/members/{member}', [MemberController::class, 'show'])->whereNumber('member');
    Route::patch('/members/{member}', [MemberController::class, 'update'])->whereNumber('member');
    Route::delete('/members/{member}', [MemberController::class, 'destroy'])->whereNumber('member');

    Route::get('/members/{member}/jumpstart', [MemberController::class, 'jumpstart_index'])->whereNumber('member');
    Route::post('/members/{member}/jumpstart', [MemberController::class, 'jumpstart_store'])->whereNumber('member');
    Route::delete('/members/{member}/jumpstart', [MemberController::class, 'jumpstart_destroy'])->whereNumber('member');

    Route::patch('/members/{member}/features', UpdateMemberFeatures::class)->whereNumber('member');

    //

    Route::get('/parts', [PartController::class, 'index']);
    Route::post('/parts', [PartController::class, 'store']);
    Route::get('/parts/{part}', [PartController::class, 'show'])->whereNumber('part');
    Route::patch('/parts/{part}', [PartController::class, 'update'])->whereNumber('part');
    Route::delete('/parts/{part}', [PartController::class, 'destroy'])->whereNumber('part');
    Route::post('/parts/{part}/duplicate', [PartController::class, 'duplicate'])->whereNumber('part');
    Route::post('/parts/{part}/favorites', [PartFavoriteController::class, 'store'])->whereNumber('part');
    Route::delete('/parts/{part}/favorites', [PartFavoriteController::class, 'destroy'])->whereNumber('part');

    Route::get('/parts/categories', [PartCategoryController::class, 'index']);
    Route::post('/parts/categories', [PartCategoryController::class, 'store']);
    Route::get('/parts/categories/{category}', [PartCategoryController::class, 'show'])->whereNumber('category');
    Route::patch('/parts/categories/{category}', [PartCategoryController::class, 'update'])->whereNumber('category');
    Route::delete('/parts/categories/{category}', [PartCategoryController::class, 'destroy'])->whereNumber('category');

    Route::get('/parts/tags', [PartTagController::class, 'index']);
    Route::post('/parts/tags', [PartTagController::class, 'store']);
    Route::get('/parts/tags/{tag}', [PartTagController::class, 'show'])->whereNumber('tag');
    Route::patch('/parts/tags/{tag}', [PartTagController::class, 'update'])->whereNumber('tag');
    Route::delete('/parts/tags/{tag}', [PartTagController::class, 'destroy'])->whereNumber('tag');

    Route::get('/parts/upcs', [PartUPCController::class, 'index']);
    Route::post('/parts/upcs', [PartUPCController::class, 'store']);
    Route::get('/parts/upcs/{upc}', [PartUPCController::class, 'show'])->whereNumber('upc');
    Route::patch('/parts/upcs/{upc}', [PartUPCController::class, 'update'])->whereNumber('upc');
    Route::delete('/parts/upcs/{upc}', [PartUPCController::class, 'destroy'])->whereNumber('upc');

    //

    Route::get('/profile', [UserController::class, 'profile']);
    Route::patch('/profile', [UserController::class, 'profileUpdate']);

    //

    Route::get('/symbols', [SymbolController::class, 'index']);

    //

    // Route::get('/uploads', [UploadController::class, 'index']);
    Route::post('/uploads', [UploadController::class, 'store']);
    // Route::get('/uploads/{upload}', [UploadController::class, 'show'])->whereNumber('upload');
    // Route::patch('/uploads/{upload}', [UploadController::class, 'update'])->whereNumber('upload');
    Route::delete('/uploads/{upload}', [UploadController::class, 'destroy'])->whereNumber('upload');

    //

    Route::get('/users', [UserController::class, 'index']);
    Route::post('/users', [UserController::class, 'store']);
    Route::get('/users/{user}', [UserController::class, 'show'])->whereNumber('user');
    Route::patch('/users/{user}', [UserController::class, 'update'])->whereNumber('user');
    Route::delete('/users/{user}', [UserController::class, 'destroy'])->whereNumber('user');

    Route::post('/impersonation', [ImpersonationController::class, 'store']);
    Route::delete('/impersonation', [ImpersonationController::class, 'destroy']);
});

// Copilot AI Assistant (no auth required for demo)
Route::post('/copilot/chat', [CopilotController::class, 'chat']);
