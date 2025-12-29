<!doctype html>
<html class="h-full bg-gray-50">

<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <script src="https://cdn.tailwindcss.com"></script>
    <script>
        tailwind.config = {
            theme: {
                extend: {
                    colors: {
                        clifford: '#da373d',
                    }
                }
            }
        }
    </script>
</head>

<body class="h-full">
    <div class="min-h-full flex flex-col justify-center py-12 px-8">
        <div class="sm:mx-auto sm:w-full sm:max-w-md">
            <svg
              class="text-[#eeb808] mx-auto h-12 w-auto fill-current"
              xmlns="http://www.w3.org/2000/svg"
              viewBox="0 0 934 1080"
            >
              <g fill-rule="evenodd">
                <path d="m467 0 466 270-778 450 312 180 466-269v179a150213 150213 0 0 1-466 270L1 810 0 270 467 0Zm0 180L155 360a8740 8740 0 0 0 1 181l468-271-157-90Z" />
                <path d="m481 803-157-91a587368 587368 0 0 1 609-351v181L481 803Z" />
              </g>
            </svg>
            <h2 class="mt-6 text-center text-3xl font-extrabold">
                Reset password
            </h2>
        </div>

        <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
            <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
                <form action="{{ route('password.update') }}" method="POST">

                    @csrf
                    @if ($errors->any())
                        @foreach ($errors->all() as $error)
                            {{-- <div>{{ $error }}</div> --}}
                        @endforeach
                    @endif
                    @if ($errors)
                        {{-- @dump($errors) --}}
                    @endif
                    <input type="hidden" name="token" value="{{ $request->route('token')}}">
                    <div class="space-y-6">
                        <div>
                            <label for="email" class="block text-sm font-medium text-gray-700">
                                Email address
                            </label>
                            <div class="mt-1">
                                <input id="email" name="email" type="email" autocomplete="email" value="{{ $request->email }}" required
                                    class="
                                        appearance-none
                                        block
                                        border
                                        border-gray-300
                                        focus:border-blue-500
                                        focus:outline-none
                                        focus:ring-blue-500
                                        placeholder-gray-400
                                        px-3
                                        py-2
                                        rounded-md
                                        shadow-sm
                                        sm:text-sm
                                        w-full
                                        @error('email') border-red-600 @enderror
                                " />
                            </div>
                            @error('email')
                                <div class="text-red-600">
                                    {{ $message }}
                                </div>
                            @enderror
                        </div>

                        <div>
                            <label for="password" class="block text-sm font-medium text-gray-700">
                                Password
                            </label>
                            <div class="mt-1">
                                <input id="password" name="password" type="password"
                                    required
                                    class="
                                        appearance-none
                                        block
                                        border
                                        border-gray-300
                                        focus:border-blue-500
                                        focus:outline-none
                                        focus:ring-blue-500
                                        placeholder-gray-400
                                        px-3
                                        py-2
                                        rounded-md
                                        shadow-sm
                                        sm:text-sm
                                        w-full
                                        @error('password') border-red-600 @enderror
                                ">
                            </div>
                            @error('password')
                                <div class="text-red-600">
                                    {{ $message }}
                                </div>
                            @enderror
                        </div>

                        <div>
                            <label for="password_confirmation" class="block text-sm font-medium text-gray-700">
                                Confirm Password
                            </label>
                            <div class="mt-1">
                                <input id="password_confirmation" name="password_confirmation" type="password"
                                    required
                                    class="
                                        appearance-none
                                        block
                                        border
                                        border-gray-300
                                        focus:border-blue-500
                                        focus:outline-none
                                        focus:ring-blue-500
                                        placeholder-gray-400
                                        px-3
                                        py-2
                                        rounded-md
                                        shadow-sm
                                        sm:text-sm
                                        w-full
                                        @error('password') border-red-600 @enderror
                                ">
                            </div>
                            @error('password')
                                <div class="text-red-600">
                                    {{ $message }}
                                </div>
                            @enderror
                        </div>

                        <div>
                            <button type="submit"
                                class="
                                    w-full
                                    flex
                                    justify-center
                                    py-2
                                    px-4
                                    border
                                    border-transparent
                                    rounded-md
                                    shadow-sm
                                    text-sm
                                    font-medium
                                    text-white
                                    bg-blue-600
                                    hover:bg-blue-700
                                    focus:outline-none
                                    focus:ring-2
                                    focus:ring-offset-2
                                    focus:ring-blue-500
                                ">
                                Reset password
                            </button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
        <div class="text-sm text-center text-gray-500 mt-4">
            Laravel v{{ Illuminate\Foundation\Application::VERSION }} (PHP v{{ PHP_VERSION }})
        </div>
    </div>

</body>

</html>
