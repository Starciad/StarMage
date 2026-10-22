# Star Mage

## Table of Contents

- [Star Mage](#star-mage)
  - [Table of Contents](#table-of-contents)
  - [Overview](#overview)
  - [Gameplay Summary](#gameplay-summary)
  - [Key Features](#key-features)
  - [Technical Overview](#technical-overview)
  - [Procedural Level Generation](#procedural-level-generation)
  - [Project Structure](#project-structure)
  - [Requirements](#requirements)
  - [Setup](#setup)
  - [Build and Run](#build-and-run)
  - [Publishing](#publishing)
  - [Platform Notes](#platform-notes)
  - [Controls](#controls)
  - [Contributing](#contributing)
  - [License](#license)
  - [Author](#author)
  - [Links](#links)

## Overview

Star Mage is a 2D side-scrolling platform shooter inspired by 1990s NES-era games and designed around short, arcade-style sessions. The project was developed entirely in C# with .NET 8 and MonoGame 3.8.4.

The game supports Windows and Linux through separate platform projects and includes the original project assets in the repository.

## Gameplay Summary

Gameplay is structured across 16 levels, organized as 4 worlds with 4 levels each. You progress by traversing procedurally assembled stages, fighting enemies, collecting stars, and clearing boss encounters at world-ending stages.

During play, you can discover spell variations that change your attack behavior:

- `Bolt`
- `Bounce`
- `ZigZag`
- `Wave`

The core loop favors direct controls and high readability, with lightweight stage runs that still require movement precision and resource awareness.

## Key Features

- 2D side-scrolling action with platforming and shoot 'em up combat
- 16 procedurally generated levels (4 worlds x 4 levels)
- Boss-focused pacing on every 4th level
- Multiple enemy types with spawn-location-aware allocation
- Spell pickups that rotate combat style (`Bolt`, `Bounce`, `ZigZag`, `Wave`)
- Shader-based indexed palette colorization for dynamic visual variation
- Original sprites, sound effects, and music assets included in source
- Windows and Linux platform targets

## Technical Overview

Star Mage is split into one shared core project and two platform executables:

- `src/StarMage.Core`: gameplay logic, actors, level generation, rendering systems, input, audio wrappers, data tables
- `src/StarMage.Windows`: Windows entry point and packaging target (`net8.0-windows7.0`, `win-x64`)
- `src/StarMage.Linux`: Linux entry point and packaging target (`net8.0`, `linux-x64`)

The project uses:

- .NET SDK-style projects
- MonoGame 3.8.4 (`WindowsDX` on Windows target, `DesktopGL` on Linux target)
- MonoGame content pipeline (`MonoGame.Content.Builder.Task`)
- Local .NET tools for MGCB and MGCB editors (`src/.config/dotnet-tools.json`)

## Procedural Level Generation

The generator is one-dimensional in structure: levels are assembled horizontally from predefined chunk templates.

Implementation highlights from `src/StarMage.Core/Levels/LevelGenerator.cs` and related files:

- Chunk size is fixed at 16x11 tiles.
- A level is built as:
  - 1 beginning chunk
  - N normal chunks
  - 1 ending chunk, or 1 arena chunk every 4th level
- Horizontal chunk count scales with level index:
  - Starts at 7 chunks
  - Increases gradually with progression
  - Clamped to a maximum of 32
- Spawn markers embedded in chunk tile data drive placement for:
  - Starting/finishing stars
  - crystals and treasure chests
  - enemy spawn pools (ground, walls, ceiling, aerial)
  - boss spawn position
- Enemy allocation is weighted and capacity-aware:
  - Filters enemies by level progression rules and compatible spawn locations
  - Computes target enemy count from available positions and a level-scaled spawn rate
  - Distributes enemies by weighted ratios and available pool capacity
  - Uses evenly spaced index selection to avoid clustered placement

This approach keeps generation understandable and deterministic in structure while still producing variation through chunk selection and weighted spawning.

## Project Structure

```text
src/
 StarMage.sln
 StarMage.Core/
  Actors/
  Audio/
  Chunks/
  Content/
  Databases/
  Handlers/
  Levels/
  Tiles/
 StarMage.Windows/
 StarMage.Linux/
 Scripts/
  publish_all.ps1
 Targets/
```

## Requirements

| Component | Version / Notes |
| --- | --- |
| .NET SDK | 8.0 |
| MonoGame packages | 3.8.4 (resolved via NuGet) |
| Supported OS targets | Windows x64, Linux x64 |

Notes:

- The repository includes a local .NET tool manifest for MonoGame CLI tools in `src/.config/dotnet-tools.json`.
- During build, the imported `Restore.targets` runs `dotnet tool restore` automatically before package reference collection.

## Setup

From the repository root:

```bash
dotnet --version
dotnet tool restore --tool-manifest src/.config/dotnet-tools.json
dotnet restore src/StarMage.sln
```

If you use Visual Studio, open `src/StarMage.sln`.

## Build and Run

Build the full solution:

```bash
dotnet build src/StarMage.sln -c Release
```

Run on Windows:

```bash
dotnet run --project src/StarMage.Windows/StarMage.Windows.csproj -c Release
```

Run on Linux:

```bash
dotnet run --project src/StarMage.Linux/StarMage.Linux.csproj -c Release
```

Build a specific target explicitly:

```bash
dotnet build src/StarMage.Windows/StarMage.Windows.csproj -c Release
dotnet build src/StarMage.Linux/StarMage.Linux.csproj -c Release
```

Manual MonoGame content pipeline commands are optional for this project because content is already integrated into the build through `MonoGame.Content.Builder.Task`. If you need to run MGCB directly, restore tools first and run the `mgcb` command against `src/StarMage.Core/Content/StarMage.mgcb`.

## Publishing

You can publish each platform directly:

```bash
dotnet publish src/StarMage.Windows/StarMage.Windows.csproj -c Release -o publish/StarMage.Windows
dotnet publish src/StarMage.Linux/StarMage.Linux.csproj -c Release -o publish/StarMage.Linux
```

Or use the repository script (`src/Scripts/publish_all.ps1`), which publishes both targets into `src/Publish`:

```powershell
Set-Location src/Scripts
powershell -ExecutionPolicy Bypass -File .\publish_all.ps1
```

## Platform Notes

- Windows target project:
  - `TargetFramework`: `net8.0-windows7.0`
  - `RuntimeIdentifier`: `win-x64`
  - MonoGame backend: `MonoGame.Framework.WindowsDX`

- Linux target project:
  - `TargetFramework`: `net8.0`
  - `RuntimeIdentifier`: `linux-x64`
  - MonoGame backend: `MonoGame.Framework.DesktopGL`

- Linux runtime note:
  - The DesktopGL backend may require native multimedia libraries available to the system runtime on some distributions.

- Both platform projects are configured as self-contained publish targets and include the same core content pipeline reference (`StarMage.mgcb`).

## Controls

Keyboard and gamepad mappings are defined in `src/StarMage.Core/InputSystem/Controller.cs`.

| Action            | Keyboard   | Gamepad            |
| ----------------- | ---------- | ------------------ |
| Move              | Arrow keys | D-pad / Left stick |
| Jump              | `Z`        | `A`                |
| Shoot / Run       | `X`        | `X`                |
| Start / Pause     | `Enter`    | `Start`            |
| Toggle Fullscreen | `F11`      | -                  |

## Contributing

Contributions are welcome in principle, especially for technical discussion, code readability, or platform/build improvements.

Please note the project status:

- Star Mage is considered complete for its original goals.
- New features, bug fixes, or maintenance updates are not guaranteed.
- Pull requests and suggestions may be reviewed, but acceptance and any continued development remain at the author's discretion.

If you plan to contribute:

1. Fork the repository.
2. Create a topic branch.
3. Keep changes focused and well documented.
4. Open a pull request describing motivation, approach, and testing.

## License

This repository uses separate licenses for code and assets.

| Scope                                  | License                                                                                   | File                |
| -------------------------------------- | ----------------------------------------------------------------------------------------- | ------------------- |
| [Source Code](./LICENSE)               | GNU General Public License, Version 3                                                     | `LICENSE`           |
| [Artistic Assets](./LICENSE-ASSETS.md) | Creative Commons Attribution-NonCommercial-ShareAlike 4.0 International (CC BY-NC-SA 4.0) | `LICENSE-ASSETS.md` |

No additional scope qualifiers are declared in those files beyond the split represented by their names and repository organization. Review both license files directly before redistribution or reuse.

## Author

Developed entirely by Davi "Starciad" Fernandes.

## Links

- Official Itch.io page (learn more, download releases, and optionally support development with donations): <https://starciad.itch.io/star-mage>
