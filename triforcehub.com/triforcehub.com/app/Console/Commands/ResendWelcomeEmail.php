<?php

namespace App\Console\Commands;

use App\Mail\SubscriptionCreated;
use App\Models\User;
use Illuminate\Console\Command;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\Mail;
use Illuminate\Support\Str;

class ResendWelcomeEmail extends Command
{
    protected $signature = 'email:resend-welcome {user_id}';

    protected $description = 'Reset user password and resend welcome email';

    public function handle(): int
    {
        $user_id = $this->argument('user_id');
        $user = User::find($user_id);

        if (! $user) {
            $this->error('user not found');

            return Command::FAILURE;
        }

        $password = Str::random(18);
        $user->password = Hash::make($password);
        $user->save();

        Mail::to($user->email)
            ->bcc('bruno@electric-ease.com')
            ->send(new SubscriptionCreated($user->name, $user->email, $password));

        $this->info("email sent to {$user->email}");

        return Command::SUCCESS;
    }
}
