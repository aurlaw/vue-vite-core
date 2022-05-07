# VueViteCore

Demo .NET 6 MVC with Vue components powered by Vite.

Showcases using vite to build vue components and sass and integrate within a basic MVC Razor application

## Requirements
- Node 17+
- .NET 6 SDK
- Docker

This demo was built using JetBrains Rider and VSCode

## SQL Server

SQL Server is managed by Docker

Start container
```
 docker compose up -d
```

Stop container

```
 docker compose down
```


## Installation

```
npm --prefix ./VueViteCore/wwwroot/ClientComponents install
```

## Running

Start Vite
```
npm --prefix ./VueViteCore/wwwroot/ClientComponents run dev
```

Run Dotnet app via VisualStudio/Rider or CLI

```
dotnet run --project VueViteCore/VueViteCore.csproj
```

## EF Migrations

To use `dotnet-ef` for your migrations please add the following flags to your command (values assume you are executing from repository root)

* `--project VueViteCore.Business` (optional if in this folder)
* `--startup-project VueViteCore`
* `--output-dir Persistence/Migrations`

For example, to add a new migration from the root folder:

`dotnet ef migrations add "SampleMigration" --project VueViteCore.Business --startup-project VueViteCore --output-dir Persistence\Migrations`

dotnet ef migrations add "InitialCreation" --project VueViteCore.Business --startup-project VueViteCore --output-dir Persistence/Migrations

## Publishing

This will compile the .NET 6 app in release mode, execute npm install and the vite build (npm run build) for the Vue app and copy to the dist folder in wwwroot

Publish from Visual Studio/Rider or CLI

```
dotnet publish VueViteCore/VueViteCore.sln  -c Release
```

## Docker

```
cd VueViteCore
docker build -t aurlaw/vuevitecore:1.0 .
docker run -d -p 9090:80 aurlaw/vuevitecore:1.0

```
