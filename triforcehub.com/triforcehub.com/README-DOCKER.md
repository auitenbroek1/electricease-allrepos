# Electric Ease - Docker Development Setup

This guide will help you run the Electric Ease application locally using Docker Desktop.

## Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running
- Git (to clone the repository)

## Quick Start

### 1. Setup Environment File

Copy the Docker environment file to `.env`:

```bash
cp .env.docker .env
```

### 2. Generate Application Key

```bash
docker compose run --rm app php artisan key:generate
```

### 3. Start the Containers

```bash
docker compose up -d
```

This will start three services:
- **app** (PHP/Laravel) - http://localhost:8000
- **mysql** (MySQL 8.0) - localhost:3306
- **node** (Vite/React) - http://localhost:3000

### 4. Run Migrations and Seeders

Wait for the MySQL container to be healthy (about 30 seconds), then run:

```bash
# Run migrations
docker compose exec app php artisan migrate

# Seed the test user
docker compose exec app php artisan db:seed --class=TestUserSeeder
```

### 5. Access the Application

- **Main App**: http://localhost:8000
- **Dev Login** (auto-login as test user): http://localhost:8000/dev-login
- **Vite Dev Server**: http://localhost:3000

Test user credentials:
- Email: `test@test.com`
- Password: `password`

## AI Copilot Setup

To use the AI Copilot feature, add your API key to the `.env` file:

```bash
# For OpenAI
OPENAI_API_KEY=your-openai-api-key-here

# Or for Anthropic Claude
ANTHROPIC_API_KEY=your-anthropic-api-key-here
```

After updating the `.env` file, restart the containers:

```bash
docker compose restart app
```

## Common Commands

### View Container Logs

```bash
# All containers
docker compose logs -f

# Specific container
docker compose logs -f app
docker compose logs -f mysql
docker compose logs -f node
```

### Run Artisan Commands

```bash
docker compose exec app php artisan <command>

# Examples:
docker compose exec app php artisan migrate:fresh
docker compose exec app php artisan db:seed
docker compose exec app php artisan cache:clear
docker compose exec app php artisan config:clear
```

### Run Composer Commands

```bash
docker compose exec app composer <command>

# Example:
docker compose exec app composer install
docker compose exec app composer update
```

### Run NPM Commands

```bash
docker compose exec node npm <command>

# Example:
docker compose exec node npm install
docker compose exec node npm run build
```

### Access Container Shell

```bash
# PHP container
docker compose exec app bash

# Node container
docker compose exec node sh

# MySQL container
docker compose exec mysql bash
```

### Database Access

```bash
# MySQL CLI
docker compose exec mysql mysql -u electricease -psecret electricease

# Or use a GUI tool with:
# Host: localhost
# Port: 3306
# Database: electricease
# Username: electricease
# Password: secret
```

## Stopping the Environment

```bash
# Stop containers (preserves data)
docker compose down

# Stop and remove volumes (deletes database data)
docker compose down -v
```

## Rebuilding Containers

If you make changes to the Dockerfile or need a fresh build:

```bash
docker compose build --no-cache
docker compose up -d
```

## Troubleshooting

### MySQL Connection Issues

If the app can't connect to MySQL, wait a few seconds for the database to initialize:

```bash
docker compose logs mysql
```

Look for "ready for connections" in the output.

### Permission Issues

If you encounter permission issues with storage or cache:

```bash
docker compose exec app chmod -R 777 storage bootstrap/cache
```

### Port Conflicts

If ports 8000, 3000, or 3306 are already in use, modify the port mappings in `docker-compose.yml`:

```yaml
ports:
  - "8001:8000"  # Change 8000 to 8001
```

### Fresh Start

To completely reset your local environment:

```bash
docker compose down -v
docker compose build --no-cache
docker compose up -d
docker compose exec app php artisan migrate:fresh
docker compose exec app php artisan db:seed --class=TestUserSeeder
```

## Architecture Overview

```
+------------------+     +------------------+     +------------------+
|                  |     |                  |     |                  |
|   Node.js        |     |   PHP/Laravel    |     |   MySQL 8.0      |
|   (Vite + React) |     |   (Nginx + FPM)  |     |   Database       |
|                  |     |                  |     |                  |
|   Port: 3000     |---->|   Port: 8000     |---->|   Port: 3306     |
|                  |     |                  |     |                  |
+------------------+     +------------------+     +------------------+
```

- **Node container**: Runs Vite dev server with hot module replacement
- **App container**: Runs PHP-FPM + Nginx serving the Laravel application
- **MySQL container**: Persistent database storage

## Development Workflow

1. The Vite dev server (port 3000) proxies API requests to Laravel (port 8000)
2. Edit React/TypeScript files in `resources/src/` - changes hot-reload automatically
3. Edit PHP files - changes are reflected immediately (no rebuild needed)
4. Run `php artisan` commands via `docker compose exec app php artisan <command>`
