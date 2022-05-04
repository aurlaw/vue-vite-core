# VueViteCore

Demo .NET 6 MVC with Vue components powered by Vite.

Showcases using vite to build vue components and sass and integrate within a basic MVC Razor application

## Requirements
- Node 17+
- .NET 6 SDK

This demo was built using JetBrains Rider and VSCode

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


## Publishing

This will compile the .NET 6 app in release mode, execute npm install and the vite build (npm run build) for the Vue app and copy to the dist folder in wwwroot

Publish from Visual Studio/Rider or CLI

```
dotnet publish --c Release
```

