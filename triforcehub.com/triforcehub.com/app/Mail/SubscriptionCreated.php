<?php

namespace App\Mail;

use Illuminate\Bus\Queueable;
use Illuminate\Mail\Mailable;
use Illuminate\Mail\Mailables\Content;
use Illuminate\Mail\Mailables\Envelope;
use Illuminate\Queue\SerializesModels;

class SubscriptionCreated extends Mailable
{
    use Queueable, SerializesModels;

    public function __construct(
        public string $name,
        public string $email,
        public string $password,
    ) {
        //
    }

    public function envelope()
    {
        return new Envelope(
            subject: 'Subscription Created',
        );
    }

    public function content()
    {
        return new Content(
            markdown: 'emails.subscriptions.created',
        );
    }

    public function attachments()
    {
        return [];
    }
}
