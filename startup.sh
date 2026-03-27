#!/bin/bash
echo "=== Checking database ==="
cd /app/datacollector

# Check if database has data
if [ ! -f "/app/data/codingtracker.db" ] || [ $(sqlite3 /app/data/codingtracker.db "SELECT COUNT(*) FROM Submissions;" 2>/dev/null || echo "0") -eq "0" ]; then
    echo "Database is empty. Seeding..."
    dotnet CodeSolvedTracker.DataCollector.dll
    echo "Database seeded!"
else
    echo "Database already has data. Skipping seed."
fi

# Start API
cd /app/api
dotnet CodeSolvedTracker.API.dll
