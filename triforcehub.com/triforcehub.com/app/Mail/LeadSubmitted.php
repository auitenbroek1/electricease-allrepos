<?php

namespace App\Mail;

use Illuminate\Bus\Queueable;
use Illuminate\Mail\Mailable;
use Illuminate\Mail\Mailables\Content;
use Illuminate\Mail\Mailables\Envelope;
use Illuminate\Queue\SerializesModels;
use Illuminate\Support\Str;

class LeadSubmitted extends Mailable
{
    use Queueable, SerializesModels;

    public function __construct(
        public string $name,
        public string $email,
        public string $phone,
        public string $company,
        public string $sms
    ) {
        //
    }

    public function envelope()
    {
        return new Envelope(
            subject: 'Lead Submitted',
        );
    }

    public function content()
    {
        // ray($_SERVER);

        $data = collect($_SERVER)
            ->sort()
            ->filter(function ($value, $key) {
                if (Str::startsWith($key, 'CONTENT_')) {
                    return true;
                }
                if (Str::startsWith($key, 'DOCUMENT_')) {
                    return true;
                }
                if (Str::startsWith($key, 'HTTP_')) {
                    return true;
                }
                if (Str::startsWith($key, 'PATH_')) {
                    return true;
                }
                if (Str::startsWith($key, 'QUERY_')) {
                    return true;
                }
                if (Str::startsWith($key, 'REDIRECT_')) {
                    return true;
                }
                if (Str::startsWith($key, 'REMOTE_')) {
                    return true;
                }
                if (Str::startsWith($key, 'REQUEST_')) {
                    return true;
                }
                if (Str::startsWith($key, 'SCRIPT_')) {
                    return true;
                }
                if (Str::startsWith($key, 'SERVER_')) {
                    return true;
                }

                return false;
            })
            ->toArray();

        return new Content(
            markdown: 'emails.leads.submitted',
            with: ['data' => $data],
        );
    }

    public function attachments()
    {
        return [];
    }
}
