<?php

namespace Src\Upstash\Engines;

use Laravel\Scout\Builder;
use Laravel\Scout\EngineManager;
use Laravel\Scout\Engines\Engine;

class HybridEngine extends Engine
{
    public function update($models)
    {
        app(EngineManager::class)->engine('algolia')->update($models);
        app(EngineManager::class)->engine('upstash')->update($models);
    }

    public function delete($models)
    {
        app(EngineManager::class)->engine('algolia')->delete($models);
        app(EngineManager::class)->engine('upstash')->delete($models);
    }

    public function search(Builder $builder)
    {
        return app(EngineManager::class)->engine('algolia')->search($builder);
    }

    public function paginate(Builder $builder, $perPage, $page)
    {
        return app(EngineManager::class)->engine('algolia')->paginate($builder, $perPage, $page);
    }

    public function mapIds($results)
    {
        return app(EngineManager::class)->engine('algolia')->mapIds($results);
    }

    public function map(Builder $builder, $results, $model)
    {
        return app(EngineManager::class)->engine('algolia')->map($builder, $results, $model);
    }

    public function lazyMap(Builder $builder, $results, $model)
    {
        return app(EngineManager::class)->engine('algolia')->lazyMap($builder, $results, $model);
    }

    public function getTotalCount($results)
    {
        return app(EngineManager::class)->engine('algolia')->getTotalCount($results);
    }

    public function flush($model)
    {
        app(EngineManager::class)->engine('algolia')->flush($model);
        app(EngineManager::class)->engine('upstash')->flush($model);
    }

    public function createIndex($name, array $options = [])
    {
        return app(EngineManager::class)->engine('algolia')->createIndex($name, $options);
    }

    public function deleteIndex($name)
    {
        return app(EngineManager::class)->engine('algolia')->deleteIndex($name);
    }
}
