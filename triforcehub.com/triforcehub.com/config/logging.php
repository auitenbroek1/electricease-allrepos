<?php

return [

    'channels' => [
        'flare' => [
            'driver' => 'flare',
        ],

        'vapor' => [
            'driver' => 'stack',
            'channels' => ['flare', 'stderr'],
            'ignore_exceptions' => false,
        ],
    ],

];
