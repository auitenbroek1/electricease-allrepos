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

      <div class="col-span-2">
        <div class="space-y-4">
          <div>
            <span class="text-black font-bold">Material Summary:</span>
          </div>
          @isset($report)
            <div>
              <div class="flex justify-between">
                <div>Name</div>
                <div>Quantity</div>
              </div>
              <div class="space-y-2 mt-2">
                @foreach($report as $item)
                  <div class="flex justify-between border-t border-gray-200 pt-2">
                    <div>
                      {{$item['name']}}
                    </div>
                    <div>
                      {{$item['quantity']}}
                    </div>
                  </div>
                @endforeach
              </div>
            </div>
          @endisset
        </div>
      </div>

    </div>
  </body>
</html>
