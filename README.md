# 🚀 DataProm ETL DemoApp

**A minimal working demo of the DataProm platform — showcasing dynamic ETL pipelines, schema-driven metadata, and SQLite-based local analytics.**

Developed by **DataProm s.r.o. © 2025**

⚠️ This project is licensed under a proprietary license. See [LICENSE.txt](./DataProm.ETLConsoleApp/LICENSE.txt) for details.

---

[![🚀 Build & Release Executables](https://github.com/dataprom-online/etl-app/actions/workflows/build-and-release.yml/badge.svg)](https://github.com/dataprom-online/etl-app/actions/workflows/build-and-release.yml)

## 🧠 What Is This?

This demo shows the power and flexibility of the **DataProm Platform**, a data engineering and analytics framework focused on:

- ⚡ Schema-driven ETL pipelines  
- 🧩 Metadata-first configuration  
- 🔄 Normalization-aware dynamic records  
- 🧪 Local SQLite processing  
- 🧱 Modular app layout with logging and diagnostics  

---

## 🗂️ Project Structure

```text
DemoApp [AppName]
├── app.config.json                  # Global app configuration
├── SqliteETL [Project]              # Sample project with schema + sync setup
│   ├── fetch.xml                    # ETL fetch definitions
│   ├── test.db                      # Local storage for the data
│   └── Configuration
│       ├── metadata.xml             # Schema definition
│       └── sync.config.xml          # Project-specific sync configuration
├── logs                             # Application logs
│   ├── appLog.log                   # Root-level log
│   └── SqliteETL
│       └── appLog.log               # Project-specific ETL log
```

---

### ⚙️ Application Startup Workflow

When the application starts, it performs the following initialization steps:

**App Initialization**  
Loads directory layout, global config (`app.config.json`), and logging.  
> Application root is:  
```csharp
Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
    "dataprom"
)
```

### 🧭 Platform-specific paths:

| Platform    | Base Folder (`Environment.SpecialFolder.LocalApplicationData`) | Full Application Path                                    |
| ----------- | -------------------------------------------------------------- | -------------------------------------------------------- |
| **Windows** | `C:\Users\<username>\AppData\Local`                            | `C:\Users\<username>\AppData\Local\dataprom`             |
| **macOS**   | `/Users/<username>/Library/Application Support`                | `/Users/<username>/Library/Application Support/dataprom` |
| **Linux**   | `/home/<username>/.local/share`                                | `/home/<username>/.local/share/dataprom`                 |


1. **Global Configuration Initialization**
   - Loads the global app configuration from:
     ```
     app.config.json
     ```
   - Sets the active application name (e.g. `"QA-Test"`).
   - Applies culture and language settings from the configuration.

2. **Logging Setup**
   - Initializes the **FileLogger** with an app-level folder under:
     ```
     <ApplicationRoot>/logs/
     ```
   - Begins capturing startup diagnostics and timing.

3. **Project Enumeration**
   - Reads all projects from the global configuration.
   - Iterates through enabled projects using:
     ```csharp
     App.GetEnabledProjects()
     ```

4. **Per-Project Context Initialization**
   For each enabled project:
   - Loads **project configuration** and directories.
   - Initializes **metadata** from `metadata.xml`.
   - Loads **Fetch XML definitions** from `fetch.xml`.
   - Sets up **project-level logging** in:
     ```
     <ApplicationRoot>/logs/<ProjectName>/
     ```

5. **Project Context Access**
   - Exposes key runtime objects:
     - `App.SyncConfig` → synchronization settings
     - `App.AppConfig` → global app settings
     - `App.CurrentProject` → active project context

6. **Error Handling**
   - Any errors are captured and written to the log:
     ```csharp
     FileLogger.LogException(ex);
     ```

7. **Run Completion**
   - When all initialization completes successfully:
     ```csharp
     DataProm.Core.GlobalApp.StartupDiagnosticsManager.MarkRunCompleted(App.AppConfig);
     ```
   - Marks the run as successful and stores diagnostic information.

---

### 🧠 Example Console App Initialization

```csharp
// ✅ Initialize App data - Directory structure, Metadata, configs
App.InitializeAppConfig(appName);
ConsolePrint.WriteLine($"App Data {App.AppConfig.AppName} Initialized...");
ConsolePrint.WriteLine($"App Configuration {App.AppConfig.AppName} Initialized...");
ConsolePrint.WriteLine($"App Culture {App.AppConfig.AppName} Initialized...");

foreach (ProjectInfo projectInfo in App.GetEnabledProjects())
{
    App.LoadProjectContext(projectInfo);

    ConsolePrint.WriteLine($"Project {projectInfo.Name} Initialized...");
    ConsolePrint.WriteLine("Metadata Initialized...");
    ConsolePrint.WriteLine("FetchXml Repository Initialized...");

    AppDatabase.Initialize(App.SyncConfig);
    ConsolePrint.WriteLine("AppDatabase Initialized...");        

    // ✅ ETL Pipeline
    ConsolePrint.WriteLine("ETL Pipeline Running...");
    ETLPipeline.Run();
    ConsolePrint.WriteLine("ETL Pipeline Finished...");
}

DateTime end = DateTime.Now;
ConsolePrint.WriteLine($"Elapsed {end.Subtract(start).TotalMilliseconds} ms", ConsolePrint.Category.Complete);

StartupDiagnosticsManager.MarkRunCompleted(App.AppConfig);
```

## 🖥️ Application and Configuration View

![DataProm ETL Screen](https://raw.githubusercontent.com/dataprom-online/.github/main/profile/dpm-etl-screen.png)


## 💻 Two Runtime Packages

### 1️⃣ Console App

**Purpose:** Clean runtime, accepts `--app [AppName]` argument.  
- Reads configuration, metadata, and projects from `ApplicationRoot`  
- Intended to run the ETL backend for the **DataProm ETL Configurator Web App**  
- Does **not generate dummy data** — relies on actual configured projects

Example:

```bash
DataProm.ConsoleApp --app DemoApp
```

---

### 2️⃣ Self-Configured App

**Purpose:** Fully self-contained simulation of ETL pipelines.  
- Creates a local SQLite database  
- Generates dummy data, performs extraction, transformation, and loads back into DB  
- Great for testing, demos, or CI/CD pipelines without connecting to a real backend  

**ETL Steps**
- ✅ Generate Dummy Data (simulates source) — **SelfConfiguredApp only**  
- 📥 Extract records dynamically  
- 🧪 Transform (schema-aware manipulation)  
- 📤 Load into normalized SQLite DB  

Example:

```bash
DataProm.SelfConfiguredApp --app DemoApp
```

---

## 🖥️ Platform Support & Permissions

> ⚠️ macOS & Linux may block execution for unsigned apps. To run:

1. Open **System Preferences → Security & Privacy**  
2. Allow the app to run after trying to launch  
3. Make Linux/macOS binary executable if needed:
```bash
chmod +x DataProm.ConsoleApp
chmod +x DataProm.SelfConfiguredApp
```

Platforms: **Windows, Linux, macOS**

---

## 🧪 Tech Highlights

- ✅ `DynamicRecordStruct` for schema-bound, high-perf data handling  
- 🧠 Normalization-aware string handling via global dictionary mapping  
- 📦 Configurable via XML and JSON  
- 🧩 High-performance `IDataReader` mapping via field readers  
- 🧰 Log structure and diagnostics ready  
- 🧪 CSV export, validation, LINQ-style querying (planned)  

---

## 🔽 Download & Run

Download platform-specific release assets from [v1.0.0 Release page](https://github.com/dataprom-online/etl-app/releases/tag/v1.0.0):

| Package | Description |
|---------|-------------|
| `DataProm.ConsoleApp_[platform]_v1.0.0.zip` | Clean runtime for web app backend, accepts `--app` |
| `DataProm.SelfConfiguredApp_[platform]_v1.0.0.zip` | Self-contained ETL simulation with dummy data |

**Linux / macOS Example:**
```bash
unzip DataProm.SelfConfiguredApp_linux-x64_v1.0.0.zip
chmod +x DataProm.SelfConfiguredApp
./DataProm.SelfConfiguredApp --app DemoApp
```

**Windows Example:**
- Extract ZIP  
- Run `DataProm.ConsoleApp.exe` or `DataProm.SelfConfiguredApp.exe`  

---

## ▶️ Run the Demo using CLI

```bash
dotnet run --project DemoApp
```

> Check the `logs/` directory for diagnostic output.

---

## 🐳 Docker Support (Planned)

Containerized ETL execution with SQLite persistence is planned:

- Bind-mounted `projects/` and `logs/`  
- Volume-based SQLite persistence  
- Optional cron-based scheduling  

---

## 📊 For BI Analysts & Data Engineers

- ✅ Ready-to-query normalized SQLite data  
- 📥 Import/export support for CSV pipelines  
- 🔍 Track every ETL step with log-based lineage  
- 💡 Extendable with your own SQL/BI dashboards  

**Use it to:**
- Prototype data pipelines  
- Standardize metadata  
- Validate transformations  
- Power embedded reporting  

---

## 📄 Example Data Preview

After running SelfConfiguredApp, `SqliteETL/test.db` is created with normalized and transformed data.

### 🧾 Tables Overview

```text
DummyData                      -- Main data table with multiple data types
DummyData_prop15_Attributes    -- Normalized multi-valued attributes
```

### ✅ Sample Row – `DummyData`

```text
Id          = 1C6h9
Date        = 2025-06-30
IntMax      = 2147483647
Double      = 3.14159265358979
Bool        = 1
Guid        = fe7c124f-3a7d-4b8b-adf3-7847c47b9b2e
...
```

### ✅ Sample Row – `DummyData_prop15_Attributes`

```text
Id = 1
Value = MvgkJwFiGkCXhDU10xGa
```

---

## 📣 Credits

Built by the **DataProm Platform Team** – your partner in **structured, scalable, and schema-first data pipelines**.