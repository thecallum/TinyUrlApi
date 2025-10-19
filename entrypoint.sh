#!/bin/bash
set -e

echo "==================================="
echo "🚀 TinyURL Backend Startup"
echo "==================================="

echo "⏳ Waiting for postgres..."
until dotnet ef database update --no-build 2>&1; do
  echo "   Postgres not ready or migrations failed, retrying..."
  sleep 3
done

echo "✅ Postgres is ready and migrations completed!"

echo "🎯 Starting application..."
echo "==================================="

exec dotnet TinyUrl.dll "$@"