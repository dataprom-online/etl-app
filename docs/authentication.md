# 📦 DataProm Private NuGet Packages

This project uses **private NuGet packages** hosted on [GitHub Packages](https://github.com/dataprom-online) under the `dataprom-online` organization.
To build, run, or restore this project, you must authenticate using a GitHub Personal Access Token (PAT).

---

## ⚠️ Authentication Required

When executing commands like:

```bash
dotnet restore
dotnet build
dotnet add package DataProm.Core
```

You may encounter the following error:

```
error NU1301: Failed to retrieve package ...
error NU1101: Unable to find package DataProm.Core.
401 Unauthorized
```

This happens because access to packages is restricted. You need a valid token.

---

## ✅ How to Authenticate with GitHub Packages

1. **Generate a Personal Access Token (PAT)**
   Visit [GitHub Tokens](https://github.com/settings/tokens) and create a token with:

   * `read:packages`
   * `repo` (if accessing private repos)

2. **Add the GitHub package source** using the token:

```bash
dotnet nuget add source \
  --username YOUR_GITHUB_USERNAME \
  --password YOUR_GITHUB_PAT \
  --store-password-in-clear-text \
  --name github \
  "https://nuget.pkg.github.com/dataprom-online/index.json"
```

> This stores credentials locally in your user-level NuGet config.
> No secrets are stored in source code or shared.

---

## 📄 License Acceptance Required

By installing or consuming these packages (e.g., `DataProm.Core`, `DataProm.Sqlite.Core`), you agree to the license defined in the [`LICENSE.txt`](../LICENSE.txt) file of this repository.

---

## 🚘 Support

For token access, license details, or troubleshooting authentication issues:

📧 Contact: **[maros.kolibas@dataprom.online](mailto:maros.kolibas@dataprom.online)**

---
