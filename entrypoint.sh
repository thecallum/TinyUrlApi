#!/bin/bash
set -e

echo "Waiting for postgres to be ready..."
while ! nc -z postgres 5432; do
  sleep 1
done

echo "✓ Postgres is ready"

echo "Running migrations..."
dotnet ef database update --no-build

echo "✓ Migrations completed"

echo "Starting application..."
exec dotnet YourApp.dll "$@"