<?php

namespace App\Console\Commands;

use App\Models\Part;
use Illuminate\Console\Command;
use Illuminate\Support\Env;
use Illuminate\Support\Facades\DB;
use OpenAI;

class UpdatePartData extends Command
{
    protected $signature = 'parts:update {--limit=10}';

    protected $description = 'Update description and keywords';

    public function handle(): int
    {
        $limit = $this->option('limit');

        $parts = DB::select("
            SELECT DISTINCT
                p.id
                , p.name
            FROM
                assembly_part AS ap
                JOIN parts AS p ON p.id = ap.part_id
            WHERE
                (p.member_id = 1 OR p.member_id IS NULL)
                AND (p.keywords IS NULL)
            ORDER BY
                p.id
            LIMIT
                $limit
            ;
        ");

        $this->info('Found '.count($parts).' parts to update.');

        foreach ($parts as $part) {
            $this->process($part);
            $this->newLine();
        }

        return Command::SUCCESS;
    }

    private function process($part)
    {
        $this->line('Processing: '.$part->id.' - '.$part->name);

        $functions = [
            [
                'name' => 'generate_part_data',
                'description' => 'Generate a description and keywords for an electrical contractor part',
                'parameters' => [
                    'type' => 'object',
                    'properties' => [
                        'name' => [
                            'type' => 'string',
                        ],
                        'description' => [
                            'type' => 'string',
                        ],
                        'keywords' => [
                            'type' => 'string',
                        ],
                    ],
                    'required' => ['description', 'keywords'],
                ],
            ],
        ];

        $prompt = "I'm creating a database of electrical parts, and want to get a list of synonyms for a given product name to improve the search, and add a detailed product description. The person doing the search is an electrical contractor. The product name is: {$part->name}";

        //

        $openai = OpenAI::client(Env::get('OPENAI_API_KEY'));

        $result = $openai->chat()->create([
            'model' => 'gpt-4o',
            'messages' => [
                [
                    'role' => 'user',
                    'content' => $prompt,
                ],
            ],
            'functions' => $functions,
            'function_call' => 'auto',
        ]);

        if (isset($result->choices[0]->message->functionCall)) {
            $response = json_decode($result->choices[0]->message->functionCall->arguments, true);

            if (json_last_error() !== JSON_ERROR_NONE) {
                $this->error('Invalid JSON response: '.json_last_error_msg());

                return;
            }

            $description = $response['description'] ?? '';
            $keywords = $response['keywords'] ?? '';

            $this->line('Description: '.$description);
            $this->line('Keywords: '.$keywords);

            $model = Part::find($part->id);
            $model->description = $description;
            $model->keywords = $keywords;
            $model->save();
        } else {
            $this->error('No function call detected in the response.');
        }
    }
}
