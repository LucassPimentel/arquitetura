param(
	[int]$WaitSeconds = 10
)

Write-Host "Starting development stack with Docker Compose..."

docker compose up -d

Write-Host "Waiting $WaitSeconds seconds for services to initialize..."
Start-Sleep -Seconds $WaitSeconds

Write-Host "Containers status:"
docker ps --filter "name=ewallet-" --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}"

Write-Host "Connection info:"
Write-Host "  MongoDB: mongodb://localhost:27017 (database: EWalletRead)"
Write-Host "  SQL Server: Server=localhost,1433;User Id=sa;Password=Your_password123"
Write-Host "  RabbitMQ: amqp://guest:guest@localhost:5672 (management UI: http://localhost:15672)"

Write-Host "Done."
