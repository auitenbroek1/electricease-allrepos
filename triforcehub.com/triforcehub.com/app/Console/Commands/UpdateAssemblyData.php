<?php

namespace App\Console\Commands;

use Illuminate\Console\Command;

class UpdateAssemblyData extends Command
{
    protected $signature = 'assemblies:update';

    protected $description = 'Update description and keywords';

    public function handle(): int
    {
        return Command::SUCCESS;
    }
}
