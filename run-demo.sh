#!/bin/bash

echo "================================================"
echo "Customer Management REST API - .NET Core 8 Demo"
echo "================================================"
echo

# Function to cleanup background processes
cleanup() {
    echo
    echo "Stopping services..."
    if [ ! -z "$API_PID" ]; then
        kill $API_PID 2>/dev/null
    fi
    exit 0
}

# Set trap to cleanup on exit
trap cleanup SIGINT SIGTERM

# Build the projects
echo "Building REST API..."
cd CustomerAPI
dotnet build --verbosity quiet
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to build CustomerAPI"
    exit 1
fi

echo "Building client..."
cd ../CustomerAPI.Client
dotnet build --verbosity quiet
if [ $? -ne 0 ]; then
    echo "ERROR: Failed to build CustomerAPI.Client"
    exit 1
fi

# Start the API in background
echo "Starting REST API server..."
cd ../CustomerAPI
dotnet run --verbosity quiet &
API_PID=$!

# Wait for API to start
echo "Waiting for API to start..."
sleep 5

# Test if API is responding
echo "Testing API availability..."
curl -s http://localhost:5202/api/customers/count > /dev/null
if [ $? -ne 0 ]; then
    echo "ERROR: API is not responding. Make sure port 5202 is available."
    cleanup
fi

echo "API is running at: http://localhost:5202"
echo "Swagger UI available at: http://localhost:5202/swagger"
echo

# Run the client demo
echo "Running client demo..."
echo "======================================"
cd ../CustomerAPI.Client
dotnet run

# Cleanup
cleanup