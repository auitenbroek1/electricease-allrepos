<?php

namespace App\Console\Commands;

use App\Models\Assembly;
use Illuminate\Console\Command;
use Illuminate\Support\Str;

// artisan triforce:normalize-4

class Normalize4 extends Command
{
    protected $signature = 'triforce:normalize-4 {--sql}';

    protected $description = 'Verify material assigned to an assembly (disconnects)';

    public function handle(): int
    {
        Assembly::disableSearchSyncing();

        Assembly::default()->orderBy('name')->with(['categories', 'parts'])->chunk(100, function ($assemblies) use (&$categories_to_be_created) {
            foreach ($assemblies as $assembly) {
                $id = $assembly->id;
                $name = $assembly->name;
                $categories = $assembly->categories;
                $parts = $assembly->parts;

                //

                if ($categories->count() != 1) {
                    continue;
                }

                $category = $categories->first()->name;

                if (
                    Str::of($category)->startsWith('Disconnect')
                ) {
                    // $this->line($category);
                } else {
                    // $this->error($category);
                    continue;
                }

                //

                $parts = $parts->map(function ($part) {
                    return [
                        'name' => $part->name,
                        'quantity' => $part->pivot->quantity,
                    ];
                })->toArray();

                $this->check($name, $category, $parts);
            }
        });

        return 0;
    }

    private function check($name, $category, $parts)
    {
        // $this->line("$category / $name");

        $segments = [
            'safety_switch_phase' => '',
            'safety_switch_voltage' => '',
            'safety_switch_duty' => '',
            'safety_switch_enclosure' => '',
            'safety_switch_disconnect' => '',
            'safety_switch_amps' => '',
            'fuse_voltage' => '',
            'fuse_amps' => '',
        ];

        // region safety switch phase

        $phases = [
            '1 Phase',
            '3 Phase',
        ];

        foreach ($phases as $phase) {
            if (Str::of($category)->contains($phase)) {
                $segments['safety_switch_phase'] = $phase;
                break;
            }
        }

        if (! Str::of($segments['safety_switch_phase'])->length()) {
            $segments['safety_switch_phase'] = '<fg=red>missing safety_switch_phase</>';
        }

        // endregion

        // region safety switch voltage and duty

        $pattern = '/(([0-9]+v) (GD|HD))/';

        $match = Str::of($category)->match($pattern);

        if (Str::of($match)->length()) {
            [$voltage, $duty] = explode(' ', $match);

            $segments['safety_switch_voltage'] = $voltage;
            $segments['safety_switch_duty'] = $duty;
        }

        if (! Str::of($segments['safety_switch_voltage'])->length()) {
            $segments['safety_switch_voltage'] = '<fg=red>missing safety_switch_voltage</>';
        }

        if (! Str::of($segments['safety_switch_duty'])->length()) {
            $segments['safety_switch_duty'] = '<fg=red>missing safety_switch_duty</>';
        }

        // endregion

        // region safety switch enclosure

        $pattern = '/(NEMA ([0-9A-Z]+))/';

        $match = Str::of($category)->match($pattern);

        if (Str::of($match)->length()) {
            $segments['safety_switch_enclosure'] = $match;
        }

        if (! Str::of($segments['safety_switch_enclosure'])->length()) {
            $segments['safety_switch_enclosure'] = '<fg=red>missing safety_switch_enclosure</>';
        }

        // endregion

        // region safety switch disconnect

        $disconnects = [
            'Non-Fused',
            'Fusible',
        ];

        foreach ($disconnects as $disconnect) {
            if (Str::of($category)->contains($disconnect)) {
                $segments['safety_switch_disconnect'] = $disconnect;
                break;
            }
        }

        if (! Str::of($segments['safety_switch_disconnect'])->length()) {
            $segments['safety_switch_disconnect'] = '<fg=red>missing safety_switch_disconnect</>';
        }

        // endregion

        // region safety amps

        $pattern = '/([0-9A-Z]+a)/';

        $match = Str::of($name)->match($pattern);

        if (Str::of($match)->length()) {
            $segments['safety_switch_amps'] = $match;
        }

        if (! Str::of($segments['safety_switch_amps'])->length()) {
            $segments['safety_switch_amps'] = '<fg=red>missing safety_switch_amps</>';
        }

        // endregion

        // region fuse amps

        if (Str::of($segments['safety_switch_voltage'])->length()) {
            $segments['fuse_voltage'] = $segments['safety_switch_voltage'];

            if ($segments['fuse_voltage'] === '240v') {
                $segments['fuse_voltage'] = '250v';
            }
        }

        if (! Str::of($segments['fuse_voltage'])->length()) {
            $segments['fuse_voltage'] = '<fg=red>missing fuse_voltage</>';
        }

        // endregion

        // region fuse amps

        $pattern = '/([0-9A-Z]+a) Fuses/';

        $match = Str::of($name)->match($pattern);

        if (Str::of($match)->length()) {
            $segments['fuse_amps'] = $match;
        }

        if (! Str::of($segments['fuse_amps'])->length()) {
            $segments['fuse_amps'] = '<fg=red>missing fuse_amps</>';
        }

        // endregion

        // $this->line(implode(' : ', $segments));
        // $this->newLine();

        $this->check_segments($name, $category, $parts, $segments);
    }

    private function check_segments($name, $category, $parts, $segments)
    {
        $has_errors = false;

        $logs = [];

        $logs[] = ['white', ["# $category / $name"]];

        // region safety switch

        $candidate = null;

        foreach ($parts as $part) {
            if (Str::of($part['name'])->upper()->contains('SAFETY SWITCH')) {
                $candidate = $part;
                break;
            }
        }

        if ($candidate) {
            $haystack = $candidate['name'];

            $normalized_haystack = Str::of($haystack)
                ->upper()
                ->replace('NEMA 3 ', 'NEMA 3R ')
                ->replace('GENERAL DUTY', 'GD')
                ->replace('HEAVY DUTY', 'HD')
                ->replace(' VOLT', 'V')
                ->replace(' AMP', 'A');

            $normalized_needles = [];

            // region safety switch phase

            $needle = $segments['safety_switch_phase'];
            $normalized_needle = Str::of($needle)->upper();

            if (Str::of($normalized_needle)->is('1 PHASE')) {
                $normalized_needle = '2P';
            }

            if (Str::of($normalized_needle)->is('3 PHASE')) {
                $normalized_needle = '3P';
            }

            $normalized_needles[] = $normalized_needle;

            // endregion

            // region safety switch voltage

            $needle = $segments['safety_switch_voltage'];
            $normalized_needle = Str::of($needle)->upper();

            $normalized_needles[] = $normalized_needle;

            // endregion

            // region safety switch duty

            $needle = $segments['safety_switch_duty'];
            $normalized_needle = Str::of($needle)->upper();

            $normalized_needles[] = $normalized_needle;

            // endregion

            // region safety switch enclosure

            $needle = $segments['safety_switch_enclosure'];
            $normalized_needle = Str::of($needle)->upper();

            $normalized_needles[] = $normalized_needle;

            // endregion

            // region safety switch disconnect

            $needle = $segments['safety_switch_disconnect'];
            $normalized_needle = Str::of($needle)->upper();

            $normalized_needles[] = $normalized_needle;

            // endregion

            // region safety switch amps

            $needle = $segments['safety_switch_amps'];
            $normalized_needle = Str::of($needle)->upper();

            $normalized_needles[] = $normalized_needle;

            // endregion

            foreach ($normalized_needles as $normalized_needle) {
                if (Str::of($normalized_haystack)->contains($normalized_needle)) {
                    // $logs[] = ['green', [$haystack, 'ok', $normalized_needle]];
                } else {
                    $logs[] = ['red', [$haystack, 'error', 'should be', $normalized_needle]];
                    $has_errors = true;
                }
            }

            //

            $quantity_is = strval($candidate['quantity'] * 1);
            $quantity_should_be = strval(1);

            if ($quantity_is !== $quantity_should_be) {
                $logs[] = ['red', [$haystack, 'error', 'quantity is', $quantity_is, 'should be', $quantity_should_be]];
                $has_errors = true;
            }
        } else {
            $logs[] = ['red', ['error, missing safety switch']];
            $has_errors = true;
        }

        // endregion

        // region fuse

        $candidate = null;

        foreach ($parts as $part) {
            if (Str::of($part['name'])->upper()->contains('CARTRIDGE FUSE')) {
                $candidate = $part;
                break;
            }
        }

        if ($candidate) {
            $haystack = $candidate['name'];

            $normalized_haystack = Str::of($haystack)
                ->upper()
                ->replace(' VOLT', 'V')
                ->replace(' AMP', 'A');

            $normalized_needles = [];

            // region fuse amps

            $needle = $segments['fuse_voltage'];
            $normalized_needle = Str::of($needle)->upper();

            $normalized_needles[] = $normalized_needle;

            // endregion

            // region fuse amps

            $needle = $segments['fuse_amps'];
            $normalized_needle = Str::of($needle)->upper();

            $normalized_needles[] = $normalized_needle;

            // endregion

            foreach ($normalized_needles as $normalized_needle) {
                if (Str::of($normalized_haystack)->contains($normalized_needle)) {
                    // $logs[] = ['green', [$haystack, 'ok', $normalized_needle]];
                } else {
                    $logs[] = ['red', [$haystack, 'error', 'should be', $normalized_needle]];
                    $has_errors = true;
                }
            }

            //

            $quantity_is = strval($candidate['quantity'] * 1);
            $quantity_should_be = strval(1);

            if ($segments['safety_switch_phase'] === '1 Phase') {
                $quantity_should_be = strval(2);
            }

            if ($segments['safety_switch_phase'] === '3 Phase') {
                $quantity_should_be = strval(3);
            }

            if ($quantity_is !== $quantity_should_be) {
                $logs[] = ['red', [$haystack, 'error', 'quantity is', $quantity_is, 'should be', $quantity_should_be]];
                $has_errors = true;
            }
        } else {
            if ($segments['safety_switch_disconnect'] === 'Fusible') {
                $logs[] = ['red', ['error, missing fuse']];
                $has_errors = true;
            }
        }

        // endregion

        // region blank name plate

        $candidate = null;

        foreach ($parts as $part) {
            if (Str::of($part['name'])->upper()->contains('BLANK NAME PLATE')) {
                $candidate = $part;
                break;
            }
        }

        if ($candidate) {
            $quantity_is = strval($candidate['quantity'] * 1);
            $quantity_should_be = strval(1);

            if ($quantity_is !== $quantity_should_be) {
                $logs[] = ['red', [$haystack, 'error', 'quantity is', $quantity_is, 'should be', $quantity_should_be]];
                $has_errors = true;
            }
        } else {
            $logs[] = ['red', ['error, missing blank name plate']];
            $has_errors = true;
        }

        // endregion

        // region hex head screw

        $candidate = null;

        foreach ($parts as $part) {
            if (
                Str::of($part['name'])->upper()->contains('HEX HEAD SCREW')
                || Str::of($part['name'])->upper()->contains('HEX HEAD BOLT')
            ) {
                $candidate = $part;
                break;
            }
        }

        if ($candidate) {
            $quantity_is = strval($candidate['quantity'] * 1);
            $quantity_should_be = strval(4);

            if ($quantity_is !== $quantity_should_be) {
                $logs[] = ['red', [$haystack, 'error', 'quantity is', $quantity_is, 'should be', $quantity_should_be]];
                $has_errors = true;
            }
        } else {
            $logs[] = ['red', ['error, missing hex head screw']];
            $has_errors = true;
        }

        // endregion

        if ($has_errors) {
            foreach ($logs as $log) {
                [$color, $string] = $log;
                $this->line("<fg=$color>".implode(' / ', $string).'</>');
            }
            $this->newLine();
        }
    }
}
