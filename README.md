## Tech used
-  .Net SDK 8.0
- Kafka

## Build

`docker compose build
`
## Run

`docker compose up
`
## Test

Open `index.html` or:

`curl http://localhost:8080/track`

## Env vars

- **Kafka__BootstrapServers**='localhost:9092'
- **Storage__Path**='/tmp/visits.log'

## Read the log file
`docker exec storage-service cat /tmp/visits.log`

## Status
- Implementation ✅
- Dockerfiles + docker compose ✅
- UnitTests ✅