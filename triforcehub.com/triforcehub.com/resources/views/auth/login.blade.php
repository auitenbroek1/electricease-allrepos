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
                Sign in to your account
            </h2>
        </div>

        <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
            <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
                @if(session('status'))
                    <div class="bg-green-100 rounded-md p-3 flex">
                        <svg
                            class="stroke-2 stroke-current text-green-600 h-5 w-8 mr-2 flex-shrink-0"
                            viewBox="0 0 24 24"
                            fill="none"
                            strokeLinecap="round"
                            strokeLinejoin="round"
                        >
                            <path d="M0 0h24v24H0z" stroke="none" />
                            <circle cx="12" cy="12" r="9" />
                            <path d="M9 12l2 2 4-4" />
                        </svg>

                        <div class="text-green-700">
                            <div class="text-sm">{{ session('status') }}</div>
                        </div>
                    </div>
                    @endif
                <form action="" method="POST">
                    @csrf
                    @if ($errors->any())
                        @foreach ($errors->all() as $error)
                            {{-- <div>{{ $error }}</div> --}}
                        @endforeach
                    @endif
                    @if ($errors)
                        {{-- @dump($errors) --}}
                    @endif
                    <div class="space-y-6">
                        <div>
                            <label for="email" class="block text-sm font-medium text-gray-700">
                                Email address
                            </label>
                            <div class="mt-1">
                                <input id="email" name="email" type="email" autocomplete="email" required
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
                                <input id="password" name="password" type="password" autocomplete="current-password"
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

                        <div class="flex items-center justify-between">
                            <div class="flex items-center">
                                <input id="remember" name="remember" type="checkbox"
                                    class="h-4 w-4 text-blue-600 focus:ring-blue-500 border-gray-300 rounded">
                                <label for="remember" class="ml-2 block text-sm">
                                    Remember me
                                </label>
                            </div>
                            <div class="flex items-center">
                                <a href="{{ route('password.request') }}" class="ml-2 block text-sm text-blue-500 hover:text-blue-800">Forgot password?</a>
                            </div>
                        </div>

                        <div>
                            <button type="submit"
                                class="
                                    bg-blue-600
                                    border
                                    border-transparent
                                    flex
                                    focus:outline-none
                                    focus:ring-2
                                    focus:ring-blue-500
                                    focus:ring-offset-2
                                    font-medium
                                    hover:bg-blue-700
                                    items-center
                                    justify-center
                                    px-4
                                    py-2
                                    rounded-md
                                    shadow-sm
                                    text-sm
                                    text-white
                                    w-full
                                ">
                                Log In
                            </button>
                        </div>

                        @env('local')
                          <div class="relative">
                            <div class="absolute inset-0 flex items-center">
                              <div class="w-full border-t border-gray-300"></div>
                            </div>
                            <div class="relative flex justify-center">
                              <span class="px-2 bg-white text-sm text-gray-500">Or continue with</span>
                            </div>
                          </div>
                          <div class="text-center">
                            <a
                              class="
                                bg-white
                                border
                                border-gray-300
                                flex
                                focus:outline-none
                                focus:ring-2
                                focus:ring-blue-500
                                focus:ring-offset-2
                                font-medium
                                hover:bg-gray-50
                                items-center
                                justify-center
                                px-4
                                py-2
                                rounded-md
                                shadow-sm
                                text-gray-700
                                text-sm
                                w-full
                              "
                              href="/auth/login/now"
                            >
                              <svg class="mr-3 h-5 w-5" xmlns="http://www.w3.org/2000/svg" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                                <path stroke-linecap="round" stroke-linejoin="round" d="M10 20l4-16m4 4l4 4-4 4M6 16l-4-4 4-4" />
                              </svg>
                              Developer Account
                            </a>
                          </div>
                        @endenv
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
