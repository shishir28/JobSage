#!/bin/bash

# JobSage Backend Environment Setup Script

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

echo -e "${GREEN}JobSage Backend Environment Setup${NC}"
echo "======================================"

# Check if .env exists
if [ ! -f ".env" ]; then
    echo -e "${YELLOW}Creating .env file from .env.example...${NC}"
    if [ -f "../.env.example" ]; then
        cp ../.env.example .env
        echo -e "${GREEN}✓ .env file created${NC}"
    else
        echo -e "${RED}✗ .env.example not found${NC}"
        exit 1
    fi
else
    echo -e "${GREEN}✓ .env file already exists${NC}"
fi

# Check if .env.docker exists
if [ ! -f ".env.docker" ]; then
    echo -e "${YELLOW}✗ .env.docker file not found${NC}"
    echo "Please create .env.docker for Docker configuration"
else
    echo -e "${GREEN}✓ .env.docker file exists${NC}"
fi

# Check if Pipenv is installed
if ! command -v pipenv &> /dev/null; then
    echo -e "${RED}✗ Pipenv not found. Installing pipenv...${NC}"
    pip install pipenv
fi

# Install dependencies
echo -e "${YELLOW}Installing Python dependencies...${NC}"
pipenv install --dev

echo -e "${GREEN}✓ Setup complete!${NC}"
echo ""
echo "To start the development server:"
echo "  pipenv run start"
echo ""
echo "To run tests:"
echo "  pipenv run test"
echo ""
echo "To format code:"
echo "  pipenv run format"
