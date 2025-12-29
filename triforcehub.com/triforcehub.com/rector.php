<?php

declare(strict_types=1);

use Rector\Config\RectorConfig;
use Rector\Php56\Rector\FuncCall\PowToExpRector;
use Rector\Php70\Rector\FuncCall\RandomFunctionRector;
use Rector\Php71\Rector\FuncCall\RemoveExtraParametersRector;
use Rector\Php74\Rector\Assign\NullCoalescingOperatorRector;
use Rector\Php74\Rector\Closure\ClosureToArrowFunctionRector;
use Rector\Php74\Rector\Property\RestoreDefaultNullToNullableTypePropertyRector;
use Rector\Php80\Rector\Catch_\RemoveUnusedVariableInCatchRector;
use Rector\Php80\Rector\Class_\ClassPropertyAssignToConstructorPromotionRector;
use Rector\Php81\Rector\FuncCall\NullToStrictStringFuncCallArgRector;
use Rector\Php83\Rector\ClassMethod\AddOverrideAttributeToOverriddenMethodsRector;
use Rector\Php84\Rector\FuncCall\AddEscapeArgumentRector;

return RectorConfig::configure()
    ->withPaths([
        __DIR__,
    ])
    ->withSkip([
        __DIR__.'/node_modules',
        __DIR__.'/storage',
        __DIR__.'/vendor',
        AddEscapeArgumentRector::class,
        AddOverrideAttributeToOverriddenMethodsRector::class,
        ClassPropertyAssignToConstructorPromotionRector::class,
        ClosureToArrowFunctionRector::class,
        NullCoalescingOperatorRector::class,
        NullToStrictStringFuncCallArgRector::class,
        PowToExpRector::class,
        RandomFunctionRector::class,
        RemoveExtraParametersRector::class,
        RemoveUnusedVariableInCatchRector::class,
        RestoreDefaultNullToNullableTypePropertyRector::class,
    ])
    ->withPhpSets();
