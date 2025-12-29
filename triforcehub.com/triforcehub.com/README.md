# triforcehub.com

## scout

php artisan scout:flush "App\Models\Assembly"
php artisan scout:import "App\Models\Assembly"

php artisan scout:index parts
php artisan scout:flush "App\Models\Part"
php artisan scout:import "App\Models\Part" --chunk=100
php artisan scout:delete-index parts

php artisan upstash:search "electrical outlet" --limit=1

## jumpstart

php artisan triforce:duplicate-job 4214 345
php artisan triforce:duplicate-job 4215 345
php artisan triforce:duplicate-job 4216 345
php artisan triforce:duplicate-job 4217 345
php artisan triforce:duplicate-job 4218 345

## migration

php artisan triforce:migrate-armor-electric-jobs
php artisan triforce:migrate-custom-electric-jobs
php artisan triforce:migrate-electrical-concepts-jobs
php artisan triforce:migrate-live-wire-electric
php artisan triforce:migrate-trinity-power-jobs

## docker

sudo rm -rf ~/Library/Containers/com.docker.*

cd /usr/local/var/log

sudo du -h -d2

brew services list
brew services stop --all

sudo brew services list
sudo brew services stop --all

## ost

* count
- count quantity items, e.g. light fixtures, electrical outlets, etc.

* linear
- measure distance, e.g. cable, conduit pipe

* linear with drop
- measure conduit and include drop distance at user defined points

* count by distance
- divides linear instance into quantity for j-hooks, anchors, straps, etc.

* area / volume
- measure area in square units, or space in cubic ft

* auto-count
