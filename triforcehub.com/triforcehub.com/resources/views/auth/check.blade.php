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
            <svg class="text-[#eeb808] mx-auto h-12 w-auto fill-current" xmlns="http://www.w3.org/2000/svg"
                viewBox="0 0 934 1080">
                <g fill-rule="evenodd">
                    <path
                        d="m467 0 466 270-778 450 312 180 466-269v179a150213 150213 0 0 1-466 270L1 810 0 270 467 0Zm0 180L155 360a8740 8740 0 0 0 1 181l468-271-157-90Z" />
                    <path d="m481 803-157-91a587368 587368 0 0 1 609-351v181L481 803Z" />
                </g>
            </svg>
            <h2 class="mt-6 text-center text-3xl font-extrabold">
                Sign in to your account
            </h2>
        </div>

        <div class="mt-8 sm:mx-auto sm:w-full sm:max-w-md">
            <div class="bg-white py-8 px-4 shadow sm:rounded-lg sm:px-10">
                <div class="space-y-6">
                    @if (session('error'))
                        <div class="bg-red-100 rounded-md p-3 flex">
                            <svg class="stroke-2 stroke-current text-red-600 h-5 w-8 mr-2 flex-shrink-0"
                                viewBox="0 0 24 24" fill="none" strokeLinecap="round" strokeLinejoin="round">
                                <path d="M0 0h24v24H0z" stroke="none" />
                                <circle cx="12" cy="12" r="9" />
                                <path d="M9 12l2 2 4-4" />
                            </svg>

                            <div class="text-red-700">
                                <div class="text-sm">{{ session('error') }}</div>
                            </div>
                        </div>
                    @endif
                    <div>
                        <a href="/auth/login"
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
                            Go Back
                        </a>
                    </div>
                </div>
            </div>
        </div>
        <div class="text-sm text-center text-gray-500 mt-4">
            Laravel v{{ Illuminate\Foundation\Application::VERSION }} (PHP v{{ PHP_VERSION }})
        </div>
    </div>
</body>

</html>
