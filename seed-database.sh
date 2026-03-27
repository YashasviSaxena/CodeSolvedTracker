#!/bin/bash
echo "Checking if database needs seeding..."
if [ ! -f "/app/data/codingtracker.db" ] || [ ! -s "/app/data/codingtracker.db" ]; then
    echo "Seeding database with mock data..."
    dotnet run --project CodeSolvedTracker.DataCollector/CodeSolvedTracker.DataCollector.csproj
    echo "Database seeded successfully!"
else
    echo "Database already exists, skipping seed."
fi
