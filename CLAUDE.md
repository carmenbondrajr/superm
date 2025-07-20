# 🛒 Supermarket Simulator Mods - Development Guide

This repository contains a comprehensive modding framework for creating BepInEx mods for Supermarket Simulator. Each mod outputs as a single DLL with individual build scripts for independent development.

## 📋 Repository Overview

**Goal**: Create a collection of Supermarket Simulator mods with:
- Individual mod DLL outputs
- Independent build scripts per mod
- Shared infrastructure and conventions
- Easy setup and development workflow

## 🏗️ Project Structure

```
superm/
├── mods/                          # Individual mod projects
│   └── MinimumWage/               # Example: Staff wage control mod
│       ├── MinimumWage.csproj     # Individual project file
│       ├── MinimumWagePlugin.cs   # Main plugin entry point
│       ├── ModBase.cs             # Base class for this mod
│       ├── EmployeeCostPatch.cs   # Harmony patches
│       ├── StaffWageConfig.cs     # Configuration management
│       └── Properties/AssemblyInfo.cs
├── libs/                          # Game & BepInEx dependencies
│   ├── BepInEx.dll               # From BepInEx/core/
│   ├── 0Harmony.dll              # From BepInEx/core/
│   ├── Assembly-CSharp.dll       # From game's Managed folder
│   ├── UnityEngine.CoreModule.dll
│   └── UnityEngine.dll
├── build-output/                  # Compiled mod DLLs
│   └── netstandard2.1/           # Individual mod DLLs output here
├── build.sh                      # Master build script
├── superm.sln                     # Visual Studio solution
└── CLAUDE.md                     # This file
```

## 🚀 Getting Started

### 1. Setup Dependencies

Copy these 5 DLL files from your Supermarket Simulator installation to `libs/`:

**From `BepInEx/core/`:**
- `BepInEx.dll`
- `0Harmony.dll`

**From `Supermarket Simulator_Data/Managed/`:**
- `Assembly-CSharp.dll`
- `UnityEngine.CoreModule.dll`
- `UnityEngine.dll`

**Game folder locations:**
- **Steam macOS**: `~/Library/Application Support/Steam/steamapps/common/Supermarket Simulator/Contents/Resources/Data/Managed/`
- **Steam Windows**: `C:\Program Files (x86)\Steam\steamapps\common\Supermarket Simulator\Supermarket Simulator_Data\Managed\`

### 2. Build All Mods

```bash
./build.sh
```

### 3. Build Individual Mod

```bash
dotnet build mods/MinimumWage/MinimumWage.csproj --configuration Release
```

### 4. Install Mods

Copy individual DLL files from `build-output/netstandard2.1/` to your game's `BepInEx/plugins/` folder.

## 🔨 Creating New Mods

### Step 1: Create Mod Directory Structure

```bash
mkdir mods/YourModName
mkdir mods/YourModName/Properties
```

### Step 2: Create Project File (`YourModName.csproj`)

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>netstandard2.1</TargetFramework>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    <LangVersion>latest</LangVersion>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <AssemblyName>YourModName</AssemblyName>
    <OutputPath>../../build-output</OutputPath>
  </PropertyGroup>

  <ItemGroup>
    <Reference Include="BepInEx">
      <HintPath>../../libs/BepInEx.dll</HintPath>
      <Private>False</Private>
    </Reference>
    <Reference Include="0Harmony">
      <HintPath>../../libs/0Harmony.dll</HintPath>
      <Private>False</Private>
    </Reference>
    <Reference Include="Assembly-CSharp">
      <HintPath>../../libs/Assembly-CSharp.dll</HintPath>
      <Private>False</Private>
    </Reference>
    <Reference Include="UnityEngine.CoreModule">
      <HintPath>../../libs/UnityEngine.CoreModule.dll</HintPath>
      <Private>False</Private>
    </Reference>
    <Reference Include="UnityEngine">
      <HintPath>../../libs/UnityEngine.dll</HintPath>
      <Private>False</Private>
    </Reference>
  </ItemGroup>

</Project>
```

### Step 3: Create Main Plugin Class

**File**: `mods/YourModName/YourModNamePlugin.cs`

```csharp
using BepInEx;
using BepInEx.Logging;

namespace SupermarketSimulatorMods.YourModName
{
    [BepInPlugin("com.supermarketsim.yourmodname", "Your Mod Name", "1.0.0")]
    [BepInProcess("Supermarket Simulator.exe")]
    public class YourModNamePlugin : ModBase
    {
        internal static new ManualLogSource Logger;
        
        protected override string GetModName() => "Your Mod Name";
        protected override string GetModVersion() => "1.0.0";
        
        protected override void Awake()
        {
            Logger = base.Logger;
            base.Awake();
        }
        
        protected override void InitializeConfig()
        {
            // Initialize your mod's configuration here
            // Example: YourModConfig.Initialize(Config);
        }
    }
}
```

### Step 4: Create ModBase Class

**File**: `mods/YourModName/ModBase.cs`

```csharp
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SupermarketSimulatorMods.YourModName
{
    public abstract class ModBase : BaseUnityPlugin
    {
        protected ManualLogSource ModLogger;
        protected Harmony HarmonyInstance;
        
        protected virtual void Awake()
        {
            ModLogger = base.Logger;
            ModLogger.LogInfo($"{GetModName()} v{GetModVersion()} loaded!");
            
            InitializeConfig();
            ApplyPatches();
            
            ModLogger.LogInfo($"{GetModName()} initialization complete!");
        }
        
        protected virtual void OnDestroy()
        {
            HarmonyInstance?.UnpatchSelf();
            ModLogger?.LogInfo($"{GetModName()} unloaded!");
        }
        
        protected abstract string GetModName();
        protected abstract string GetModVersion();
        protected virtual void InitializeConfig() { }
        
        protected virtual void ApplyPatches()
        {
            HarmonyInstance = Harmony.CreateAndPatchAll(GetType().Assembly);
        }
    }
}
```

### Step 5: Create Assembly Info

**File**: `mods/YourModName/Properties/AssemblyInfo.cs`

```csharp
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("YourModName")]
[assembly: AssemblyDescription("Description of your mod")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: Guid("your-unique-guid-here")]  // Generate with uuidgen or online tool
```

### Step 6: Update Build System

**Add to `build.sh`:**
```bash
echo "Building YourModName mod..."
dotnet build mods/YourModName/YourModName.csproj --configuration Release
```

**Add to `superm.sln`:**
```
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "YourModName", "mods\YourModName\YourModName.csproj", "{your-project-guid}"
EndProject
```

Add to the ProjectConfigurationPlatforms section:
```
{your-project-guid}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
{your-project-guid}.Debug|Any CPU.Build.0 = Debug|Any CPU
{your-project-guid}.Release|Any CPU.ActiveCfg = Release|Any CPU
{your-project-guid}.Release|Any CPU.Build.0 = Release|Any CPU
```

## 🎯 Development Best Practices

### Code Conventions

1. **Namespace**: `SupermarketSimulatorMods.YourModName`
2. **Plugin GUID**: `com.supermarketsim.yourmodname` (all lowercase)
3. **Class Names**: Use PascalCase (`YourModNamePlugin`, `SomeFeaturePatch`)
4. **File Organization**: One class per file, descriptive names

### Harmony Patching Patterns

**Prefix Patch** (modify input parameters):
```csharp
[HarmonyPatch(typeof(TargetClass), "MethodName")]
public class YourPatch
{
    static void Prefix(ref float parameterToModify)
    {
        // Modify the parameter before the original method runs
        parameterToModify *= YourConfig.SomeMultiplier;
    }
}
```

**Postfix Patch** (modify return value):
```csharp
[HarmonyPatch(typeof(TargetClass), "MethodName")]
public class YourPatch
{
    static void Postfix(ref ReturnType __result)
    {
        // Modify the result after the original method runs
        __result = CalculateNewValue(__result);
    }
}
```

### Configuration Management

Create a dedicated config class following this pattern:

```csharp
using BepInEx.Configuration;

public static class YourModConfig
{
    public static ConfigEntry<float> SomeValueConfig;
    public static ConfigEntry<bool> EnableFeatureConfig;
    
    public static float SomeValue => SomeValueConfig.Value;
    public static bool EnableFeature => EnableFeatureConfig.Value;
    
    public static void Initialize(ConfigFile config)
    {
        SomeValueConfig = config.Bind("Settings", 
            "SomeValue", 
            1.0f, 
            new ConfigDescription("Description of what this setting does",
                new AcceptableValueRange<float>(0f, 5f),
                new ConfigurationManagerAttributes { Order = 100 }));
                
        EnableFeatureConfig = config.Bind("Settings",
            "EnableFeature",
            true,
            new ConfigDescription("Enable or disable this feature"));
    }
}
```

### Reverse Engineering Game Code

1. **Use dnSpy or JetBrains Rider** to decompile `Assembly-CSharp.dll`
2. **Search for Manager classes** (e.g., `MoneyManager`, `EmployeeManager`)
3. **Look for method signatures** that handle the functionality you want to modify
4. **Identify parameters and return types** for your Harmony patches
5. **Test with simple patches first** before implementing complex logic

### Debugging Tips

1. **Enable detailed logging** in your mod for development
2. **Use BepInEx console** to monitor logs and errors
3. **Install Unity Explorer** (F7 in-game) for runtime inspection
4. **Test incrementally** - add one patch at a time
5. **Check mod load order** if multiple mods conflict

### Configuration Manager Integration

For better user experience, add Configuration Manager attributes:

```csharp
using ConfigurationManagerAttributes = BepInEx.Configuration.ConfigurationManagerAttributes;

// In your config binding:
new ConfigurationManagerAttributes 
{ 
    Order = 100,                    // Higher numbers appear first
    ShowRangeAsPercent = false,     // Show as decimal, not percentage
    IsAdvanced = true               // Hide in advanced section
}
```

## 🔧 Build System

### Individual Mod Building

Each mod can be built independently:

```bash
# Build specific mod
dotnet build mods/ModName/ModName.csproj --configuration Release

# Clean and rebuild
dotnet clean mods/ModName/ModName.csproj
dotnet build mods/ModName/ModName.csproj --configuration Release
```

### Master Build Script

The `build.sh` script:
1. Cleans previous builds
2. Builds each mod individually
3. Lists generated DLLs
4. Provides installation instructions

### Output Structure

```
build-output/
└── netstandard2.1/
    ├── ModName1.dll
    ├── ModName1.pdb
    ├── ModName2.dll
    └── ModName2.pdb
```

Only the `.dll` files need to be installed to the game.

## 🧪 Testing

### Basic Testing Checklist

- [ ] Mod loads without errors in BepInEx console
- [ ] Configuration file is created properly
- [ ] Harmony patches apply successfully
- [ ] Core functionality works as expected
- [ ] No conflicts with base game mechanics
- [ ] Configuration changes take effect

### Advanced Testing

- [ ] Test with Configuration Manager (F1 in-game)
- [ ] Test with multiple saves/scenarios
- [ ] Verify mod unloads cleanly
- [ ] Check performance impact
- [ ] Test edge cases and error conditions

## 🚨 Troubleshooting

### Common Build Issues

| Problem | Solution |
|---------|----------|
| `Assembly not found` errors | Verify all 5 DLL files are in `libs/` |
| `Permission denied` | Check file permissions and directory access |
| `Project not found` | Ensure you're in the correct directory |
| `Duplicate assembly` | Check for conflicting references |

### Common Runtime Issues

| Problem | Solution |
|---------|----------|
| Mod not loading | Check BepInEx console for errors |
| Patches not working | Verify target class/method names |
| Config not saving | Check BepInEx write permissions |
| Game crashes | Remove mod and check logs |

### Debugging Steps

1. **Check BepInEx console** for loading errors
2. **Verify plugin GUID uniqueness** (no conflicts)
3. **Test with minimal patches** first
4. **Use detailed logging** to trace execution
5. **Check game version compatibility**

## 📁 File Paths Reference

### macOS Steam
```
Game Directory: ~/Library/Application Support/Steam/steamapps/common/Supermarket Simulator/
Game Data: Contents/Resources/Data/Managed/
BepInEx: Contents/Resources/Data/../BepInEx/
Plugins: Contents/Resources/Data/../BepInEx/plugins/
Config: Contents/Resources/Data/../BepInEx/config/
```

### Windows Steam
```
Game Directory: C:\Program Files (x86)\Steam\steamapps\common\Supermarket Simulator\
Game Data: Supermarket Simulator_Data\Managed\
BepInEx: BepInEx\
Plugins: BepInEx\plugins\
Config: BepInEx\config\
```

## 🎮 Development Workflow

### Starting a New Mod

1. **Plan your mod functionality**
2. **Research game code** with dnSpy/Rider
3. **Create mod directory structure**
4. **Set up basic plugin class**
5. **Implement core patches incrementally**
6. **Add configuration system**
7. **Test thoroughly**
8. **Update build scripts**

### Example: MinimumWage Mod Analysis

The existing MinimumWage mod demonstrates:

- **Single responsibility**: Controls staff wage costs
- **Clean architecture**: Separate classes for plugin, config, and patches
- **Harmony integration**: Patches `MoneyManager.MoneyTransition`
- **Configuration**: Uses BepInEx config system with Configuration Manager support
- **Logging**: Detailed logging for debugging and transparency
- **Error handling**: Proper checks for transaction types

This is the template to follow for new mods.

## 💡 Mod Ideas for Future Development

- **Customer Behavior**: Modify shopping patterns, patience levels
- **Store Management**: Automatic restocking, advanced scheduling
- **Economic**: Dynamic pricing, supply chain simulation
- **Quality of Life**: UI improvements, keyboard shortcuts
- **Analytics**: Sales tracking, performance metrics
- **Expansion**: New product types, store layouts

Each mod should follow the established patterns and be built as an independent DLL.