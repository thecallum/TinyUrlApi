#!/bin/bash
set -e

cd /app/TinyUrl

echo "Waiting for postgres..."
until dotnet ef database update; do
  echo "   Postgres not ready or migrations failed, retrying..."
  sleep 3
done

echo "Migrations completed!"

echo "Starting application..."
echo "==================================="

cd /app/build
exec dotnet TinyUrl.dll "$@"