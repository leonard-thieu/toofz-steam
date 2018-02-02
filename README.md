# toofz Steam

[![Build status](https://ci.appveyor.com/api/projects/status/fhfu870220jgfm3l/branch/master?svg=true)](https://ci.appveyor.com/project/leonard-thieu/toofz-necrodancer-leaderboards/branch/master)
[![codecov](https://codecov.io/gh/leonard-thieu/toofz-steam/branch/master/graph/badge.svg)](https://codecov.io/gh/leonard-thieu/toofz-steam)
[![MyGet](https://img.shields.io/myget/toofz/v/toofz.Steam.svg)](https://www.myget.org/feed/toofz/package/nuget/toofz.Steam)

## Overview

**toofz Steam** is a .NET library designed for retrieving leaderboards, players, and user-generated content from [Steam](http://store.steampowered.com/about/). 
It includes clients for Steam Client API, [Steam Community Data](https://partner.steamgames.com/documentation/community_data), 
[Steam Web API](https://partner.steamgames.com/doc/webapi_overview), and Steam Workshop.

---

**toofz Steam** is a component of **toofz**. 
Information about other projects that support **toofz** can be found in the [meta-repository](https://github.com/leonard-thieu/toofz-necrodancer).

## Description

toofz Steam implements clients for various Steam APIs. Only the APIs used by toofz projects are implemented. More APIs may be implemented if there is a 
demand for it. All clients have support for transient fault handling, telemetry, and cancellation.

## Installing via NuGet

Add a NuGet.Config to your solution directory with the following content:

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="toofz" value="https://www.myget.org/F/toofz/api/v3/index.json" />
  </packageSources>
</configuration>
```

```powershell
Install-Package 'toofz.Steam'
```

### Dependencies

* [toofz Build](https://github.com/leonard-thieu/toofz-build)

### Dependents

* [toofz Leaderboards Service](https://github.com/leonard-thieu/leaderboards-service)
* [toofz Daily Leaderboards Service](https://github.com/leonard-thieu/daily-leaderboards-service)
* [toofz Players Service](https://github.com/leonard-thieu/players-service)
* [toofz Replays Service](https://github.com/leonard-thieu/replays-service)

## Requirements

* [.NET Standard 2.0](https://github.com/dotnet/standard/blob/master/docs/versions.md)-compatible platform
  * .NET Core 2.0
  * .NET Framework 4.6.1
  * Mono 5.4

## Contributing

Contributions are welcome for toofz Steam.

* Want to report a bug or request a feature? [File a new issue](https://github.com/leonard-thieu/toofz-steam/issues).
* Join in design conversations.
* Fix an issue or add a new feature.
  * Aside from trivial issues, please raise a discussion before submitting a pull request.

### Development

#### Requirements

* Visual Studio 2017

#### Getting started

Open the solution file and build. Use Test Explorer to run tests.

#### Repository layout

* [`ClientApi`](src/toofz.Steam/ClientApi) - Steam Client API client (wrapper around [SteamKit](https://github.com/SteamRE/SteamKit))
* [`CommunityData`](src/toofz.Steam/CommunityData) - [Steam Community Data](https://partner.steamgames.com/documentation/community_data) client
* [`WebApi`](src/toofz.Steam/WebApi) - [Steam Web API](https://partner.steamgames.com/doc/webapi_overview) client
* [`Workshop`](src/toofz.Steam/Workshop) - Steam Workshop client

## License

**toofz Steam** is released under the [MIT License](LICENSE).
