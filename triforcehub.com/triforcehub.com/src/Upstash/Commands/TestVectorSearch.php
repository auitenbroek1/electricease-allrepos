<?php

namespace Src\Upstash\Commands;

use App\Models\Part;
use Illuminate\Console\Command;

class TestVectorSearch extends Command
{
    protected $signature = 'upstash:search
                          {query : The search query to test}
                          {--member= : Optional member ID to filter by}
                          {--limit=10 : Number of results to return}
                          {--category= : Optional category ID to filter by}
                          {--tag= : Optional tag ID to filter by}';

    protected $description = 'Test vector search functionality';

    public function handle()
    {
        // dump(config('openai'));
        // dump(config('upstash'));

        $query = $this->argument('query');
        $memberId = $this->option('member');
        $limit = $this->option('limit');
        $categoryId = $this->option('category');
        $tagId = $this->option('tag');

        // $part = Part::find(1)->searchable();
        // $part = Part::find(1)->unsearchable();
        // $part = Part::find(19034)->searchable();

        $this->info("Query: $query");
        $this->info("Member ID: $memberId");
        $this->info("Limit: $limit");
        $this->info("Category ID: $categoryId");
        $this->info("Tag ID: $tagId");

        $results = Part::search($query)
            ->when($memberId, function ($query) use ($memberId) {
                return $query->whereIn('member.id', [1, $memberId]);
            })
            ->when($categoryId, function ($query) use ($categoryId) {
                return $query->whereIn('categories.id', [$categoryId]);
            })
            ->when($tagId, function ($query) use ($tagId) {
                return $query->whereIn('tags.id', [$tagId]);
            })
            ->paginate($limit);

        $this->info("\nSearch Results:");
        $this->info('Total Results: '.$results->total());
        $this->info('Per Page: '.$results->perPage());
        $this->info('Current Page: '.$results->currentPage());
        $this->info('Last Page: '.$results->lastPage());
        $this->info("\nShowing ".count($results)." results on this page:\n");

        collect($results->items())->take(3)->each(function ($item) {
            $this->info("{$item->id}: {$item->name}");
            $this->line(json_encode($item->scoutMetaData()));
            $this->newLine();
        });

        return;

        foreach ($results as $part) {
            $this->line("ID: {$part->id}");
            $this->line("Name: {$part->name}");
            if ($part->description) {
                $this->line("Description: {$part->description}");
            }
            if ($part->keywords) {
                $this->line("Keywords: {$part->keywords}");
            }
            $this->line('Categories: '.$part->categories->pluck('name')->join(', '));
            $this->line('Tags: '.$part->tags->pluck('name')->join(', '));
            $this->line('Member ID: '.($part->member_id ?? 'null'));
            $this->line('metadata: '.serialize($part->scoutMetaData()));
            $this->line(str_repeat('-', 50)."\n");
        }
    }
}
