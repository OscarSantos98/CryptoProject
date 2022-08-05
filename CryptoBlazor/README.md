# CryptoProject

## Table of Contents

* [Getting Started](#getting-started)
    1. [Installation](#installation)
    2. [Software dependencies](#software-dependencies)
    3. [Latest releases](#latest-releases)
    4. [API references](#api-references)
* [Tools](#tools)
* [Build and Test](#build-and-test)
* [Contributors](#contributors)

## Getting Started

This quickstart provides the information need to set-up the frontend development environment, as well as the tools and services used to support it. 

This is a simple component for basic TradingView chart in Blazor supporting OHLC candle, volume and markers.

### Installation

1. Open Git Bash terminal and run the following cmd

```
git clone https://github.com/OscarSantos98/CryptoProject.git
```

### Software dependencies

* ASP.NET Core 6

#### NuGet Packages
```
<PackageReference Include="Blazored.LocalStorage" Version="4.2.0" />
<PackageReference Include="Blazored.Menu" Version="2.2.0" />
<PackageReference Include="Blazored.SessionStorage" Version="2.2.0" />
<PackageReference Include="CsvHelper" Version="27.2.1" />
<PackageReference Include="LightweightCharts.Blazor" Version="3.8.0" />
<PackageReference Include="Microsoft.AspNetCore.Components.Authorization" Version="6.0.5" />
<PackageReference Include="Microsoft.IdentityModel.Tokens" Version="6.19.0" />
<PackageReference Include="Newtonsoft.Json" Version="13.0.1" />
<PackageReference Include="System.Security.Claims" Version="4.3.0" />
```

### API references

Crypto API

[https://docs.bitfinex.com/reference#rest-public-candles](https://docs.bitfinex.com/reference#rest-public-candles)

## Tools

The following tools were used to design and develop the frontend.

1. [Microsoft SQL Server Management (SSMS)](https://aka.ms/ssmsfullsetup)
2. [Visual Studio](https://visualstudio.microsoft.com/fr/)
3. [POSTMAN](https://www.postman.com/)

## Build and Test

Either with .NET CLI or Visual Studio

```
cd CryptoProject
git checkout frontend/dev
cd CryptoBlazor
```

```
dotnet build --configuration Release
```

```
dotnet test --configuration Release --no-build
```

## Contributors

* Oscar Santos
