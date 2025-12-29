<?php

namespace App\Http\Controllers;

use App\Http\Requests\StoreUploadRequest;
use App\Http\Requests\UpdateUploadRequest;
use App\Models\Upload;
use Illuminate\Support\Facades\Storage;
use Intervention\Image\Drivers\Gd\Driver;
use Intervention\Image\ImageManager;

class UploadController extends Controller
{
    public function index()
    {
        //
    }

    public function store(StoreUploadRequest $request)
    {
        $from = $request->input('key');
        $to = str_replace('tmp/', 'uploads/', $from);

        Storage::copy($from, $to);
        $url = Storage::url($to);

        //

        $max_width = $request->input('max_width', 0);

        if ($max_width) {
            $manager = new ImageManager(Driver::class);
            $image = $manager->read(file_get_contents($url));
            $img = $image->scale($max_width, null, function ($constraint) {
                $constraint->aspectRatio();
            });

            Storage::put($to, $img->toJpeg(), 'public');
        }

        //

        $upload = new Upload;
        $upload->uuid = $request->input('uuid');
        $upload->name = $request->input('name');
        $upload->type = $request->input('type');
        $upload->size = $request->input('size');
        $upload->url = $url;
        $upload->save();

        return $upload;
    }

    public function show(Upload $upload)
    {
        //
    }

    public function update(UpdateUploadRequest $request, Upload $upload)
    {
        //
    }

    public function destroy(Upload $upload)
    {
        //
    }
}
