<?php

namespace Src\Upstash\Engines;

use Laravel\Scout\Builder;
use Laravel\Scout\Engines\Engine;
use Src\OpenAI\Services\OpenAIService;
use Upstash\Vector\Contracts\IndexInterface;
use Upstash\Vector\VectorMatch;
use Upstash\Vector\VectorQuery;
use Upstash\Vector\VectorRange;
use Upstash\Vector\VectorUpsert;

class UpstashVectorEngine extends Engine
{
    protected $index;

    protected $openai;

    public function __construct(IndexInterface $index, OpenAIService $openai)
    {
        $this->index = $index;
        $this->openai = $openai;
    }

    public function update($models): void
    {
        if ($models->isEmpty()) {
            return;
        }

        $namespace = $models->first()->searchableAs();

        $objects = $models->map(function ($model) {
            $id = $model->getKey();

            $data = collect([
                '<id>'.$model->id.'</id>',
                '<name>'.$model->name.'</name>',
                '<description>'.$model->description.'</description>',
                '<keywords>'.$model->keywords.'</keywords>',
            ])->filter()->implode("\n");

            $vector = $this->openai->createEmbedding($data);

            $metadata = $model->toSearchableArray();
            // TODO: remove
            $metadata['member'] = collect([1, $model->member_id ?? 1])->unique()->filter()->values()->toArray();
            $metadata['categories'] = $model->categories->pluck('id')->toArray();
            $metadata['tags'] = $model->tags->pluck('id')->toArray();

            return new VectorUpsert(
                id: $id,
                vector: $vector,
                metadata: $metadata,
            );
        })->filter()->values()->all();

        if (empty($objects)) {
            return;
        }

        $this->index->namespace($namespace)->upsertMany($objects);
    }

    public function delete($models): void
    {
        if ($models->isEmpty()) {
            return;
        }

        $namespace = $models->first()->searchableAs();

        $ids = $models->map(function ($model) {
            return $model->getKey();
        })->toArray();

        if (empty($ids)) {
            return;
        }

        $this->index->namespace($namespace)->delete($ids);
    }

    public function search(Builder $builder): mixed
    {
        return $this->performSearch($builder);
    }

    public function paginate(Builder $builder, $perPage, $page): mixed
    {
        return $this->performSearch($builder, $perPage, $page);
    }

    public function mapIds($results): \Illuminate\Support\Collection
    {
        if (isset($results['results'])) {
            $results = $results['results'];
        }

        if (empty($results)) {
            return collect();
        }

        return $results->pluck('id')->values();
    }

    public function map(Builder $builder, $results, $model): \Illuminate\Support\Collection
    {
        if (isset($results['results'])) {
            $results = $results['results'];
        }

        $ids = $this->mapIds($results)->all();

        if (empty($ids)) {
            return $model->newCollection();
        }

        $positions = array_flip($ids);

        return $model->getScoutModelsByIds(
            $builder, $ids
        )->filter(function ($model) use ($ids) {
            return in_array($model->id, $ids);
        })->map(function ($model) use ($results, $positions) {
            $result = $results[$positions[$model->id]] ?? [];

            foreach ($result as $key => $value) {
                $model->withScoutMetadata($key, $value);
            }

            return $model;
        })->sortBy(function ($model) use ($positions) {
            return $positions[$model->id];
        })->values();
    }

    public function lazyMap(Builder $builder, $results, $model): \Illuminate\Support\Collection
    {
        if (isset($results['results'])) {
            $results = $results['results'];
        }

        return $this->map($builder, $results, $model);
    }

    public function getTotalCount($results): int
    {
        return $results['total_count'];
    }

    public function flush($model)
    {
        $namespace = $model->searchableAs();

        $this->index->namespace($namespace)->reset();
    }

    public function createIndex($name, array $options = [])
    {
        return true;
    }

    public function deleteIndex($name)
    {
        $this->index->namespace($name)->deleteNamespace();

        return true;
    }

    //

    protected function filter(Builder $builder): string
    {
        $filters = collect($builder->wheres)->map(function ($value, $key) {
            // TODO: remove
            $key = str_replace('_id', '.id', $key);

            if (is_bool($value)) {
                return sprintf('%s = %s', $key, $value ? 'true' : 'false');
            }

            return is_numeric($value)
                ? sprintf('%s = %s', $key, $value)
                : sprintf('%s = "%s"', $key, $value);
        });

        $whereInOperators = [
            'whereIns' => 'CONTAINS',
            'whereNotIns' => 'NOT CONTAINS',
        ];

        foreach ($whereInOperators as $property => $operator) {
            if (property_exists($builder, $property)) {
                foreach ($builder->{$property} as $key => $values) {
                    // TODO: remove
                    $key = str_replace('.id', '', $key);

                    $or_conditions = collect();
                    collect($values)->each(function ($value) use ($key, $operator, $or_conditions) {
                        if (is_bool($value)) {
                            return sprintf('%s', $value ? 'true' : 'false');
                        }

                        $formatted = filter_var($value, FILTER_VALIDATE_INT) !== false
                            ? sprintf('%s', $value)
                            : sprintf('"%s"', $value);

                        $or_conditions->push(sprintf('%s %s %s', $key, $operator, $formatted));
                    });
                    $filters->push(sprintf('(%s)', $or_conditions->join(' OR ')));
                }
            }
        }

        return $filters->values()->join(' AND ');
    }

    protected function performSearch(Builder $builder, ?int $perPage = null, ?int $page = null): mixed
    {
        $query = $builder->query;
        $filter = $this->filter($builder);

        if (empty($query) && ! empty($filter)) {
            $query = '*';
        }

        $namespace = $builder->model->searchableAs();

        if (empty($query)) {
            $response = $this->index->namespace($namespace)->range(new VectorRange(
                limit: 1000
            ));
        } else {
            $vector = $this->openai->createEmbedding($query);

            $response = $this->index->namespace($namespace)->query(new VectorQuery(
                vector: $vector,
                topK: 1000,
                filter: $filter
            ));
        }

        $results = $response->getResults();

        $paginated = collect($results)->map(function (VectorMatch $result) {
            return [
                'id' => (int) $result->id,
                'score' => $result->score,
            ];
        })->forPage($page, $perPage);

        return [
            'total_count' => count($results),
            'results' => $paginated,
        ];
    }
}
