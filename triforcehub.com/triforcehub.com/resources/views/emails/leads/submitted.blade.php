<x-mail::message>

# Lead Submitted

Name: {{ $name }}

Email: {{ $email }}

Phone: {{ $phone }}

Company: {{ $company }}

SMS: {{ $sms ? 'Yes' : 'No' }}

*Additional data for some testing to prevent third-party scans:*

@foreach($data as $name => $value)
  {{ $name }}: {{ $value }}

@endforeach
</x-mail::message>
