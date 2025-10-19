
## Deploy application

1. Populate `.env`
```
POSTGRES_PASSWORD=
POSTGRES_USER=postgres
POSTGRES_DB=tinyurl
```
2. Run EF migrations
`docker compose -f compose.prod.yaml run --rm migrations`
3. Cleanup
`docker compose -f compose.prod.yaml down`
4. Start Application
`docker compose -f compose.prod.yaml up -d`

## Run locally

1. Run application
`docker compose up`
2. Ensure db is migrated
`cd \TinyUrl && dotnet ef database update`