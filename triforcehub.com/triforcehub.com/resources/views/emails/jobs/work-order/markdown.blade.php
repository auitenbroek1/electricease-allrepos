@component('mail::layout')
  @slot('header')
    @component('mail::header', ['url' => config('app.url')])
      {{ $job['member']['name'] }}
    @endcomponent
  @endslot

  # Work Order

  Number: {{ $job['number'] }}

  Name: {{ $job['name'] }}

  @slot('footer')
    @component('mail::footer')
      Copyright © {{ date('Y') }} {{ $job['member']['name'] }}.
      @lang('All rights reserved.')\
      Powered by Electric Ease.
    @endcomponent
  @endslot
@endcomponent
