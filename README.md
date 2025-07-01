# 🚀 DataProm DemoApp

**A minimal working demo of the DataProm platform — showcasing dynamic ETL pipelines, schema-driven metadata, and SQLite-based local analytics.**

Developed by **DataProm s.r.o. © 2025**

---

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

## ⚙️ How It Works

### 🧵 Pipeline Flow (from `main.cs`)

**App Initialization**  
Loads directory layout, global config (`app.config.json`), and logging.

**Project Loading**  
Loads schema (`metadata.xml`) and sync config for each enabled project.

**ETL Steps**
- ✅ Generate Dummy Data (simulates source)
- 📥 Extract records dynamically
- 🧪 Transform (schema-aware manipulation)
- 📤 Load into normalized SQLite DB

**Logging & Diagnostics**  
All major operations are timed, logged, and tracked.

---

## 💻 Platform Support

The project uses raw Mono.SQLite. For **Linux or macOS**
For **Windows**, the default System.Data.SQLite provider.

---

## 🧪 Tech Highlights

- ✅ `DynamicRecordStruct` for schema-bound, high-perf data handling  
- 🧠 Normalization-aware string handling via global dictionary mapping  
- 📦 Configurable via XML and JSON  
- 🧩 High-performance `IDataReader` mapping via field readers  
- 🧰 Log structure and diagnostics ready  
- 🧪 CSV export, validation, LINQ-style querying (planned)  

---

## ▶️ Run the Demo

```bash
dotnet run --project DemoApp
```

> Check the `logs/` directory for diagnostic output.

---

## 🐳 Docker Support (Planned)

A Dockerfile for containerized ETL execution and SQLite persistence is under development.

**Planned features:**
- Bind-mounted `projects/` and `logs/` folders  
- Volume-based SQLite persistence  
- Optional cron-based scheduling for pipelines  

---

## 📊 For BI Analysts & Data Engineers

This demo is designed as a **starter ETL + schema-driven analytics tool**:

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

After running the pipeline, the local `test.db` SQLite file is created under `SqliteETL/test.db`. It contains the transformed and normalized output of your ETL process.

### 🧾 Tables Overview

```text
DummyData                      -- Main data table with multiple data types
DummyData_prop15_Attributes    -- Normalized attribute values for a multivalue property
```

---

### 📋 Sample Row – `DummyData`

```text
Id                  = 1C6h9
Date                = 2025-06-30
Time                = 00:00:00
DateTime            = 2025-06-30 10:31:30.066673
IntMax              = 2147483647
Double              = 3.14159265358979
Bool                = 1
Float               = 1.40129846432482e-45
LongMin             = -9223372036854775808
Byte                = 1
Guid                = fe7c124f-3a7d-4b8b-adf3-7847c47b9b2e
Long                = 64
StringDateOnly      = 1990-06-13
StringDateTime      = 2025-06-30 10:31:30
StringTime          = 23:23:59
Flag                = 1
```

This row showcases:
- Full type range: integers, floats, booleans, GUIDs, dates, times
- Support for boundary values (e.g., `int.Max`, `long.Min`)
- Typed columns controlled by metadata

---

### 🧩 Sample Row – `DummyData_prop15_Attributes`

```text
Id = 1
Value   = MvgkJwFiGkCXhDU10xGa
```

Represents a **multi-valued attribute**, modeled using a separate normalized table and foreign key (`DummyId`).

---

### ✅ Why It Matters

This design reflects real-world data:
- Mixed data types
- Nullable support
- Multi-valued (1:N) modeling
- Precision-safe exports to CSV or JSON
- Ready-to-query structure for Power BI, Excel, or SQL dashboards

## 📣 Credits

Built by the **DataProm Platform Team** – your partner in **structured, scalable, and schema-first data pipelines**.
---
