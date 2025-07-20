#!/bin/bash

# Build script for Supermarket Simulator mods

echo "Building Supermarket Simulator Mods..."

# Clean previous builds
echo "Cleaning previous builds..."
rm -rf build-output/*

# Build all mods
echo "Building MinimumWage mod..."
dotnet build mods/MinimumWage/MinimumWage.csproj --configuration Release

echo "Building BackToWork mod..."
dotnet build mods/BackToWork/BackToWork.csproj --configuration Release

# List built mods
echo ""
echo "Built mods:"
ls -la build-output/netstandard2.1/*.dll 2>/dev/null || echo "No DLLs found in build-output"

echo ""
echo "Build complete! Copy mod DLLs to your BepInEx/plugins/ folder."