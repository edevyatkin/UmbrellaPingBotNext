#!/usr/bin/env bash
docker compose pull
docker compose up --force-recreate --build -d
