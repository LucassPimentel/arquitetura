param(
	[string]$ComposeFile = "docker-compose.yml"
)

Write-Host "Starting SQL Server container..."
docker compose -f $ComposeFile up -d

Write-Host "Waiting for SQL Server to accept connections..."
$max = 30
for ($i = 0; $i -lt $max; $i++) {
	try {
		$containerId = (docker ps -qf "name=db")
		if (-not $containerId) { Start-Sleep -Seconds 2; continue }
		docker exec $containerId /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "Your_password123" -Q "SELECT 1" > $null 2>&1
		if ($LASTEXITCODE -eq 0) {
			Write-Host "SQL Server is ready."
			exit 0
		}
	} catch {}
	Start-Sleep -Seconds 2
}
Write-Host "SQL Server did not become ready in time."
exit 1
