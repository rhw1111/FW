#!/bin/bash
docker-compose -f netgateway.yml down --rmi 'all'
docker-compose -f netgateway.yml up -d