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

This quickstart provides the information need to set-up the backend development environment, as well as the tools and services used to support it.

### Installation

1. Open Git Bash terminal and run the following cmd

```
git clone https://github.com/OscarSantos98/CryptoProject.git
```

### Software dependencies

* ASP.NET Core 6

#### NuGet Packages
```
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="6.0.5" />
<PackageReference Include="Microsoft.AspNetCore.Mvc.NewtonsoftJson" Version="6.0.5" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="6.0.5" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="6.0.5">
    <PrivateAssets>all</PrivateAssets>
    <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
</PackageReference>
<PackageReference Include="Microsoft.VisualStudio.Web.CodeGeneration.Design" Version="6.0.5" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.3.0" />
```

### API references

Crypto API

[https://docs.bitfinex.com/reference#rest-public-candles](https://docs.bitfinex.com/reference#rest-public-candles)

## Tools

The following tools were used to design and develop the backend.

1. [Swagger Editor](https://editor.swagger.io/)
2. [dbdiagram](https://dbdiagram.io/)
3. [Microsoft SQL Server Management (SSMS)](https://aka.ms/ssmsfullsetup)
4. [Visual Studio](https://visualstudio.microsoft.com/fr/)
5. [POSTMAN](https://www.postman.com/)

## DB Connection string

Before building your code, you need to add the connection string of your database. Create an appsettings.Development.json file which by default is used by launchSettings.json when you do not specify other environment.
This file is ignored by the .gitignore so you can freely put your connection there without risk to share it on your repo.
When deploying with the Pipeline it will replace the connection on appsettings.json as appsettings.Development.json does not exist in the repo.

The file should look similar to the following
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "CryptoDB": "Server=<NAME_OF_YOUR_LOCAL_MSFT_SQL_Server>;Initial Catalog=CryptoDB;Trusted_Connection=True;"
  }
}
```

Replace <NAME_OF_YOUR_LOCAL_MSFT_SQL_Server> with the real connection.

## Build and Test

Either with .NET CLI or Visual Studio

```
cd CryptoProject
git checkout backend/dev
cd CryptoAPI
```

```
dotnet build --configuration Release
```

```
dotnet test --configuration Release --no-build
```

## Contributors

* Oscar Santos
