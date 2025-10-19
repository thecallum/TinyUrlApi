#!/bin/bash
set -e

echo "==================================="
echo "🚀 TinyURL Backend Startup"
echo "==================================="

echo "⏳ Waiting for postgres..."
while ! nc -z postgres 5432; do
  echo "   Postgres not ready, waiting..."
  sleep 2
done
echo "✅ Postgres is ready!"

echo "🔄 Running database migrations..."
dotnet ef database update --no-build

if [ $? -eq 0 ]; then
    echo "✅ Migrations completed successfully"
else
    echo "❌ Migration failed!"
    exit 1
fi

echo "Starting application..."
exec dotnet TinyUrl.dll "$@"