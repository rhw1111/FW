#!/bin/sh

echo "Server Starting..."

python "/usr/TestPlatform.TestHostService/server.py" > "/usr/TestPlatform.TestHostService/log.log" 2>&1 &

echo "Server Started."
