<?php

return [

    'connections' => [
        'aspnet_production' => [
            'driver' => 'sqlsrv',
            'url' => env('DB_URL'),
            'host' => 'electric-ease.cngllfmtj39c.us-east-2.rds.amazonaws.com',
            'port' => '1433',
            'database' => 'production',
            'username' => 'admin',
            'password' => 'BU7U64V4BXgADdX6Qakq9.EB',
            'charset' => 'utf8',
            'prefix' => '',
            'prefix_indexes' => true,
            'trust_server_certificate' => true,
        ],
    ],

    'migrations' => [
        'table' => 'migrations',
        'update_date_on_publish' => false, // disable to preserve original behavior for existing applications
    ],

];
