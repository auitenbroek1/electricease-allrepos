<?php

namespace App\Helpers;

use Throwable;

class Math
{
    public static function normalize($input, ?int $scale = 6)
    {
        $output = $input;

        try {
            $output = number_format($input, $scale, '.', '');
        } catch (Throwable $e) {
            $output = number_format(0, 6);
        }

        return $output;
    }

    public static function add(string $num1, string $num2, ?int $scale = 6)
    {
        $num1 = self::normalize($num1);
        $num2 = self::normalize($num2);

        return bcadd($num1, $num2, $scale);
    }

    public static function subtract(string $num1, string $num2, ?int $scale = 6)
    {
        $num1 = self::normalize($num1);
        $num2 = self::normalize($num2);

        return bcsub($num1, $num2, $scale);
    }

    public static function divide(string $num1, string $num2, ?int $scale = 6)
    {
        $num1 = self::normalize($num1);
        $num2 = self::normalize($num2);

        return bcdiv($num1, $num2, $scale);
    }

    public static function multiply(string $num1, string $num2, ?int $scale = 6)
    {
        $num1 = self::normalize($num1);
        $num2 = self::normalize($num2);

        return bcmul($num1, $num2, $scale);
    }

    public static function round(int|float $num, $precision = 2)
    {
        $multiplier = pow(10, $precision);

        $output = floor($num * $multiplier * 10);
        $output = ceil($output / 10) / $multiplier;

        return $output;
    }
}
