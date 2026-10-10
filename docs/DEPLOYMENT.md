# Deploying the Snow Buddies API

The API is a standard Docker container that needs a PostgreSQL database and a few environment variables. It applies database migrations itself on startup and exposes `/health` for the platform's health checks.

## Option 1: Render (recommended, one-click)

`render.yaml` in the repo root is a Render Blueprint that creates the API service and a Postgres database and wires them together.

1. Sign in at [dashboard.render.com](https://dashboard.render.com) with GitHub.
2. Choose **New > Blueprint**, pick the `Snow-Buddies-App/snow-buddies` repository and the `development` branch.
3. When prompted, fill in `Authentication__Google__ClientIds` (comma-separated Google OAuth client IDs). Leave it blank if you're not using Google Sign-In yet.
4. Click **Apply**. Render creates the database, generates a random JWT signing key, builds the Dockerfile and deploys.
5. When the deploy is live, open `https://<service-name>.onrender.com/swagger` to try the API, and check `https://<service-name>.onrender.com/health` returns `Healthy`.

Every push to `development` redeploys automatically.

### Plans and cost

The blueprint uses Render's free plans so you can try it at no cost. They have limits that matter for real users:

| Resource | Free plan limit | Recommended for production |
|---|---|---|
| Web service | Sleeps after 15 minutes idle; the next request takes about a minute | `starter` |
| Postgres | Deleted 30 days after creation unless upgraded | `basic-256mb` or larger, with backups |

Change `plan:` in `render.yaml` (or in the dashboard) before launch. Check [render.com/pricing](https://render.com/pricing) for current prices.

### Custom domain

In the service's **Settings > Custom Domains**, add e.g. `api.snowbuddies.app`, then create the CNAME record Render shows at your DNS provider. Render issues the TLS certificate automatically.

## Option 2: Any Docker host

The same image runs on Railway, Fly.io, Azure Container Apps, Google Cloud Run, AWS App Runner or a VM:

```bash
docker build -t snow-buddies-api .
docker run -p 8080:8080 \
  -e DATABASE_URL="postgres://user:password@host:5432/snowbuddies" \
  -e AppSettings__Token="<random secret, 32+ characters>" \
  -e Authentication__Google__ClientIds="<client ids>" \
  snow-buddies-api
```

The container listens on port 8080, or on `$PORT` if the platform sets it. Point the platform's health check at `/health`. TLS should be terminated by the platform's load balancer; the app trusts `X-Forwarded-For` / `X-Forwarded-Proto` headers.

## Google Sign-In setup

1. In [Google Cloud Console](https://console.cloud.google.com/apis/credentials), create OAuth client IDs for each app (iOS, Android, and web if needed).
2. Put all of them in `Authentication__Google__ClientIds`, separated by commas.
3. The mobile app signs in with Google, then sends the resulting **ID token** to `POST /api/Auth/Google-sign-in` as `{ "idToken": "..." }` and receives the API's JWT.

## Production checklist

- [ ] Upgrade the database and web service off free plans.
- [ ] Turn on database backups.
- [ ] Set `Swagger__Enabled=false` if you don't want the API docs public.
- [ ] Keep `AppSettings__Token` secret. Changing it signs everyone out.
- [ ] Add a custom domain and point the mobile app at it.
