<?php

namespace App\Console\Commands;

use App\Models\Symbol;
use Illuminate\Console\Command;
use Illuminate\Support\Str;

class UpdateSymbols extends Command
{
    protected $signature = 'triforce:update-symbols';

    protected $description = 'Update symbols';

    public function handle(): int
    {
        $items = [
            [
                'name' => 'Power Symbols',
                'items' => [
                    [
                        'name' => 'Duplex Outlet',
                        'slug' => 'power-duplex',
                    ], [
                        'name' => 'Weatherproof Duplex Outlet',
                        'slug' => 'power-duplex-wp',
                    ], [
                        'name' => 'Ground Fault Circuit Interrupt Duplex Outlet',
                        'slug' => 'power-duplex-gfci',
                    ], [
                        'name' => 'Duplex Outlet - One Receptacle Controlled by Switch',
                        'slug' => 'power-duplex-switched',
                    ], [
                        'name' => 'Duplex Outlet on Emergency Branch',
                        'slug' => 'power-duplex-emerg',
                    ], [
                        'name' => 'Double Duplex Outlet (aka Quad Outlet)',
                        'slug' => 'power-quad',
                    ], [
                        'name' => 'Switch',
                        'slug' => 'power-switch',
                    ], [
                        'name' => '3-Way Switch',
                        'slug' => 'power-switch-3way',
                    ], [
                        'name' => 'Switch with Built-In Dimmer',
                        'slug' => 'power-switch-dimmer',
                    ], [
                        'name' => 'Power Panel',
                        'slug' => 'power-panel-power',
                    ], [
                        'name' => 'Lighting Panel',
                        'slug' => 'power-panel-lighting',
                    ], [
                        'name' => 'Junction Box',
                        'slug' => 'power-box-junction',
                    ], [
                        'name' => 'Recessed Floor Box',
                        'slug' => 'power-box-floor',
                    ], [
                        'name' => 'Through-Wall Sleeve',
                        'slug' => 'power-sleeve',
                    ],
                ],
            ], [
                'name' => 'Lighting Symbols',
                'items' => [
                    [
                        'name' => '2x2 Recessed Light',
                        'slug' => 'light-2x2-1',
                    ], [
                        'name' => '2x2 Recessed Light on Emergency Branch',
                        'slug' => 'light-2x2-emerg',
                    ], [
                        'name' => '2x4 Recessed Light',
                        'slug' => 'light-2x4-1',
                    ], [
                        'name' => '2x4 Recessed Light on Emergency Branch',
                        'slug' => 'light-2x4-emerg',
                    ], [
                        'name' => 'Recessed Linear Light',
                        'slug' => 'light-linear',
                    ], [
                        'name' => 'Recessed Linear Light on Emergency Branch',
                        'slug' => 'light-linear-emerg',
                    ], [
                        'name' => 'Surface Mounted Utility Light',
                        'slug' => 'light-utility',
                    ], [
                        'name' => 'Track Lighting',
                        'slug' => 'light-track',
                    ], [
                        'name' => 'Recessed Can Light',
                        'slug' => 'light-can',
                    ], [
                        'name' => 'Wall Mounted Light',
                        'slug' => 'light-wall',
                    ], [
                        'name' => 'Recessed Wall Wash Light',
                        'slug' => 'light-wall-wash',
                    ],
                ],
            ], [
                'name' => 'Fire Alarm Symbols',
                'items' => [
                    [
                        'name' => 'Fire Alarm Pull Box',
                        'slug' => 'fa-pull-box',
                    ], [
                        'name' => 'Fire Alarm Strobe and Horn Combination',
                        'slug' => 'fa-strobe',
                    ], [
                        'name' => 'Smoke Detector',
                        'slug' => 'fa-smoke-detector',
                    ], [
                        'name' => 'Fire Department Key Box',
                        'slug' => 'fa-keybox',
                    ], [
                        'name' => 'Ceiling Mounted Exit Sign',
                        'slug' => 'fa-exit',
                    ], [
                        'name' => 'Wall Mounted Exit Sign',
                        'slug' => 'fa-exit-wall',
                    ], [
                        'name' => 'Battery Powered Emergency Light',
                        'slug' => 'fa-light-emerg',
                    ],
                ],
            ], [
                'name' => 'Security Symbols',
                'items' => [
                    [
                        'name' => 'Panic Button or Distress Button',
                        'slug' => 'security-button-panic',
                    ], [
                        'name' => 'Card Reader',
                        'slug' => 'security-card-reader',
                    ], [
                        'name' => 'Magnetic Door Lock',
                        'slug' => 'security-lock-mag',
                    ], [
                        'name' => 'Electric Door Latch',
                        'slug' => 'security-latch-elect',
                    ], [
                        'name' => 'Electric Door Strike',
                        'slug' => 'security-strike-elect',
                    ], [
                        'name' => 'Security Camera',
                        'slug' => 'security-camera',
                    ],
                ],
            ], [
                'name' => 'Communications Symbols',
                'items' => [
                    [
                        'name' => 'Telephone Jack',
                        'slug' => 'comms-phone',
                    ], [
                        'name' => 'Data Jack',
                        'slug' => 'comms-data',
                    ], [
                        'name' => 'Data Jack for Wall Mounted Item',
                        'slug' => 'comms-data-wall',
                    ], [
                        'name' => 'Combination Telephone and Data Jack',
                        'slug' => 'comms-tele-data',
                    ], [
                        'name' => 'Ceiling Mounted Speaker',
                        'slug' => 'comms-speaker',
                    ],
                ],
            ],
        ];

        foreach ($items as $category) {
            foreach ($category['items'] as $symbol) {
                $name = $symbol['name'];
                $slug = $symbol['slug'];
                $path = resource_path('assets/symbols/'.$symbol['slug'].'.svg');
                $data = file_get_contents($path);

                $model = Symbol::where([
                    'slug' => $slug,
                ])->first();

                if ($model && $model->id) {
                    $this->line('update: '.$slug);

                    $model->name = $name;
                    $model->slug = $slug;
                    $model->data = $data;
                    $model->save();
                } else {
                    $this->line('create: '.$slug);

                    $model = new Symbol;
                    $model->uuid = Str::uuid();
                    $model->name = $name;
                    $model->slug = $slug;
                    $model->data = $data;
                    $model->position = 1;
                    $model->save();
                }
            }
        }

        return 0;
    }
}
