# 🛒 Supermarket Simulator Mods

A comprehensive modding framework for creating BepInEx mods for Supermarket Simulator. Build multiple mods with shared infrastructure and streamlined development workflow.

## 📦 Current Mods

### 💰 MinimumWage
**Control staff wages in Supermarket Simulator**

- **Global wage multiplier**: Reduce all staff costs by any percentage
- **Individual wage control**: Set custom wages per employee type (Cashiers, Restockers, etc.)
- **Real-time configuration**: Modify settings without restarting the game
- **Detailed logging**: See exactly what wage changes are being applied

---

## 🚀 Quick Start

### 1️⃣ **Setup Dependencies**

You need to copy 5 DLL files from your Supermarket Simulator installation to the `libs/` folder:

#### **From `BepInEx/core/`:**
- `BepInEx.dll`
- `0Harmony.dll`

#### **From `Supermarket Simulator_Data/Managed/`:**
- `Assembly-CSharp.dll`
- `UnityEngine.CoreModule.dll`
- `UnityEngine.dll`

> **💡 Finding your game folder:**
> - **Steam**: `~/Library/Application Support/Steam/steamapps/common/Supermarket Simulator/Contents/Resources/Data/Managed`
> - **Standalone**: Look for `Supermarket Simulator_Data/Managed/` in your installation directory

### 2️⃣ **Build Mod**
```bash
./build.sh
```

### 3️⃣ **Install Mod**
Copy **just the single** `MinimumWage.dll` from `build-output/netstandard2.1/` to your game's `BepInEx/plugins/` folder:

```bash
# Example path (adjust for your installation)
cp build-output/netstandard2.1/MinimumWage.dll ~/Library/Application\ Support/Steam/steamapps/common/Supermarket\ Simulator/Contents/Resources/Data/../BepInEx/plugins/
```

> **✅ Single DLL** - No additional dependencies needed!

### 4️⃣ **Launch Game**
Start Supermarket Simulator. The mod will create a config file on first run.

### 5️⃣ **Optional: Install Configuration Manager (Recommended)**
For an in-game configuration GUI, install [BepInEx Configuration Manager](https://github.com/BepInEx/BepInEx.ConfigurationManager):
1. Download the latest release for BepInEx 5.x
2. Extract to your game directory (DLL goes in `BepInEx/plugins/`)
3. Press **F1 in-game** to open the configuration GUI
4. Adjust settings with sliders and see descriptions on hover

> **💡 Your mod is fully compatible** - Configuration Manager will automatically detect and display all settings with sliders for numeric values!

---

## ⚙️ Configuration

After first launch, edit your configuration at:
`BepInEx/config/com.supermarketsim.minimumwage.cfg`

### **Basic Setup (Recommended)**
```ini
[General]
WageMultiplier = 0.5              # 50% staff wage reduction
EnableIndividualWageControl = false
```

### **Advanced Setup (Per-Employee Control)**
```ini
[General]
WageMultiplier = 1.0              # Ignored when individual control is enabled
EnableIndividualWageControl = true

[Individual Wages]
RestockerWage = 25.0              # Daily wage for restockers
CashierWage = 30.0                # Daily wage for cashiers
CustomerHelperWage = 20.0         # Daily wage for customer helpers
SecurityGuardWage = 35.0          # Daily wage for security guards
JanitorWage = 20.0                # Daily wage for janitors
```

### **Common Configurations**
- **Free staff**: `WageMultiplier = 0.0`
- **Normal costs**: `WageMultiplier = 1.0`
- **25% reduction**: `WageMultiplier = 0.75`
- **Double costs**: `WageMultiplier = 2.0`

### **Configuration Manager Features (If Installed)**
When using Configuration Manager (F1 in-game), you'll get:
- **Sliders** for all numeric values with appropriate ranges
- **Hover descriptions** explaining each setting
- **Real-time changes** - no need to restart the game
- **Organized sections** - "General" and "Individual Wages"
- **Proper ordering** - most important settings at the top

---

## 🏗️ Project Structure

```
├── 📁 mods/                            # Individual mod projects
│   └── 📁 MinimumWage/                 # Staff wage control mod
│       ├── MinimumWagePlugin.cs        # Main plugin entry point
│       ├── EmployeeCostPatch.cs        # Harmony patches for wage control
│       ├── StaffWageConfig.cs          # Configuration management
│       ├── ModBase.cs                  # Base class for this mod
│       ├── ConfigurationManagerAttributes.cs # Config Manager integration
│       └── MinimumWage.csproj          # Project file
├── 📁 libs/                            # Game & BepInEx dependencies
├── 📁 build-output/                    # Compiled mod DLLs
├── 🔧 build.sh                         # Build script
├── 📄 superm.sln                       # Visual Studio solution
└── 📖 README.md                        # This file
```

---

## 🛠️ Creating New Mods

### **1. Create Mod Directory**
```bash
mkdir mods/YourModName
mkdir mods/YourModName/Properties
```

### **2. Create Project File**
Create `mods/YourModName/YourModName.csproj`:
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>netstandard2.1</TargetFramework>
    <AssemblyName>YourModName</AssemblyName>
    <OutputPath>../../build-output</OutputPath>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    <LangVersion>latest</LangVersion>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="../../shared/SupermarketSimulatorShared.csproj" />
  </ItemGroup>

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

### **3. Create Plugin Class**
Create `mods/YourModName/YourModNamePlugin.cs`:
```csharp
using BepInEx;
using BepInEx.Logging;
using SupermarketSimulatorMods.Shared;

namespace SupermarketSimulatorMods.YourModName
{
    [BepInPlugin("com.supermarketsim.yourmodname", "Your Mod Name", "1.0.0")]
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
        }
    }
}
```

### **4. Create Assembly Info**
Create `mods/YourModName/Properties/AssemblyInfo.cs`:
```csharp
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("YourModName")]
[assembly: AssemblyDescription("Description of your mod")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: Guid("your-unique-guid-here")]
```

### **5. Update Build System**
Add your mod to `build.sh`:
```bash
echo "Building YourModName mod..."
dotnet build mods/YourModName/YourModName.csproj --configuration Release
```

Add to `superm.sln`:
```
Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "YourModName", "mods\YourModName\YourModName.csproj", "{your-project-guid}"
EndProject
```

---

## 🎯 Development Tips

### **Reverse Engineering Game Code**
1. **Use dnSpy or JetBrains Rider** to decompile `Assembly-CSharp.dll`
2. **Search for keywords** related to your target functionality
3. **Look for Manager classes** (e.g., `EmployeeManager`, `MoneyManager`)
4. **Identify method signatures** for Harmony patches

### **Harmony Patching**
```csharp
[HarmonyPatch(typeof(TargetClass), "MethodName")]
public class YourPatch
{
    static void Postfix(ref ReturnType __result)
    {
        // Modify the result after the original method runs
        YourModNamePlugin.Logger?.LogInfo($"Original: {__result}");
        __result = ModifiedValue;
    }
}
```

### **Debugging**
- **Use Unity Explorer** (F7 in-game) for runtime inspection
- **Check BepInEx console** for logs and errors
- **Add detailed logging** to track mod behavior
- **Test incrementally** - start with simple patches

### **Best Practices**
- **Follow naming conventions** from existing mods
- **Use the shared ModBase class** for consistency
- **Make unique plugin GUIDs** to avoid conflicts
- **Document your configuration options**
- **Test with different game scenarios**

---

## 📋 Requirements

- **.NET SDK** (for building mods)
- **BepInEx 5.x** installed in Supermarket Simulator
- **Supermarket Simulator** (Steam or standalone)
- **Basic C# knowledge** for mod development

---

## 🔧 Troubleshooting

### **Build Issues**
| Problem | Solution |
|---------|----------|
| `Assembly not found` errors | Verify all 5 DLL files are in `libs/` folder |
| `Permission denied` | Run with appropriate permissions or check file locks |
| `Project not found` | Ensure you're in the correct directory |

### **Runtime Issues**
| Problem | Solution |
|---------|----------|
| Mod not loading | Check BepInEx console for plugin GUID conflicts |
| Patches not working | Verify target class/method names match current game version |
| Config not saving | Ensure BepInEx has write permissions to config directory |
| Game crashes | Remove mod and check BepInEx logs for errors |

### **Common File Paths**
```bash
# macOS Steam
~/Library/Application Support/Steam/steamapps/common/Supermarket Simulator/

# Windows Steam  
C:\Program Files (x86)\Steam\steamapps\common\Supermarket Simulator\

# Game Data
Contents/Resources/Data/Managed/        # macOS
Supermarket Simulator_Data/Managed/     # Windows

# BepInEx
Contents/Resources/Data/../BepInEx/      # macOS
BepInEx/                                # Windows
```

---

## 📄 License

This modding framework is provided as-is for educational and modding purposes. Please respect the game's terms of service and only use for personal, non-commercial purposes.

---

## 🤝 Contributing

1. **Fork the repository**
2. **Create a feature branch** for your new mod
3. **Follow the established patterns** and naming conventions
4. **Test thoroughly** before submitting
5. **Update documentation** for new features

Happy modding! 🎮