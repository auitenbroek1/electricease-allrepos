<!doctype html>
<html>
  <head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="https://cdn.tailwindcss.com"></script>
  </head>
  <body class="text-sm font-sans p-8 text-gray-700">
    <div class="grid grid-cols-2 gap-4">

      @isset($job['member'])
        @isset($job['member']['logo'])
          @isset($job['member']['logo']['url'])
            <div class="col-span-2">
              <img
                alt="{{$job['member']['name']}}"
                class="block max-h-10"
                src="{{$job['member']['logo']['url']}}"
              />
            </div>
          @endisset
        @endisset
      @endisset

      @isset($job['member'])
        <div>
          <div>
            {{$job['member']['name']}}
          </div>
          <div>
            {{$job['member']['address1']}}
          </div>
          <div>
            {{$job['member']['address2']}}
          </div>
          <div>
            {{$job['member']['city']}}
            @if($job['member']['city'] && $job['member']['state'] && $job['member']['state']['abbreviation'])  
            <span>,</span>
            @endif
            {{$job['member']['state']['abbreviation']}}
            {{$job['member']['zip']}}
          </div>
        </div>
      @endisset

      <div>
        <div>
          <span class="text-black font-bold">Job Number:</span>
          {{$job['number']}}
        </div>
        <div>
          <span class="text-black font-bold">Job Name:</span>
          {{$job['name']}}
        </div>
      </div>

      <div class="col-span-2">
        <hr class="border-gray-400" />
      </div>

      <div>
        <div class="space-y-4">
          <div>
            <span class="text-black font-bold">Customer Address:</span>
          </div>
          @isset($customer)
            <div>
              <div>
                {{$customer['name']}}
              </div>
              <div>
                {{$customer['address1']}}
              </div>
              <div>
                {{$customer['address2']}}
              </div>
              <div>
                {{$customer['city']}}
                @if($customer['city'] && $customer['state'] && $customer['state']['abbreviation'])  
                <span>,</span>
                @endif
                {{$customer['state']['abbreviation'] ?? ''}}
                {{$customer['zip']}}
              </div>
            </div>
          @endisset
        </div>
      </div>

      <div>
        <div class="space-y-4">
          <div>
            <span class="text-black font-bold">Job Site:</span>
          </div>
          @foreach ($locations as $location)
            <div>
              <div>
                {{$location['name']}}
              </div>
              <div>
                {{$location['address1']}}
              </div>
              <div>
                {{$location['address2']}}
              </div>
              <div>
                {{$location['city']}} 
                
                @if($location['city'] && $location['state'] && $location['state']['abbreviation'])  
                <span>,</span>
                @endif
                
                {{$location['state']['abbreviation'] ?? ''}}
                {{$location['zip']}}
              </div>
            </div>
          @endforeach
        </div>
      </div>

      <div class="col-span-2">
        <div class="space-y-4">
          <div>
            <span class="text-black font-bold">Notes:</span>
          </div>
          <div>
            @foreach ($job['blocks'] as $block)
              @if ($block['job_section_id'] == 1)
                <p class="whitespace-pre-line">{{$block['content']}}</p>
              @endif
            @endforeach
          </div>
        </div>
      </div>

      <div class="col-span-2">
        <div class="space-y-4">
          <div>
            <span class="text-black font-bold">Scope of Work:</span>
          </div>
          <div>
            @foreach ($job['blocks'] as $block)
              @if ($block['job_section_id'] == 2)
                <p class="whitespace-pre-line">{{$block['content']}}</p>
              @endif
            @endforeach
          </div>
        </div>
      </div>

    </div>
  </body>
</html>
