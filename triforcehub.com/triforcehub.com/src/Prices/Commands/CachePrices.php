<?php

namespace Src\Prices\Commands;

use Illuminate\Console\Command;
use Src\Prices\Actions\CachePricesAction;

class CachePrices extends Command
{
    protected $signature = 'prices:cache {member} {distributor} {limit=100} {chunk=10}';

    protected $description = 'Cache prices for a member';

    public function handle(): int
    {
        // artisan prices:cache 1 1 // member 27
        // artisan prices:cache 1 2 // standard account 95369

        $member_id = $this->argument('member');
        $distributor_id = $this->argument('distributor');
        $limit = $this->argument('limit');
        $chunk = $this->argument('chunk');

        $this->info("member: $member_id");
        $this->info("distributor: $distributor_id");
        $this->info("limit: $limit");
        $this->info("chunk: $chunk");

        // return 0;

        try {
            $log = (new CachePricesAction)(
                member_id: $member_id,
                distributor_id: $distributor_id,
                limit: $limit,
                chunk: $chunk,
            );

            foreach ($log as $line) {
                $this->info($line);
            }
        } catch (\Throwable $exception) {
            $this->error($exception->getMessage());
        }
    }
}
