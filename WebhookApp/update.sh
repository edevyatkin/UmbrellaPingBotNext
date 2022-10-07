#!/usr/bin/env bash
dotnet ef database update
docker compose pull
docker compose up --force-recreate --build -d
