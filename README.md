# HexFlame 🔥

A mesmerizing hexagonal flame simulation created in Unity! This project was developed as part of my Bachelor's Computer Science Thesis, combining procedural animation, shader programming, and a custom hexagonal grid system to create a realistic, interactive flame effect.

![Flame Simulation Preview](https://github.com/zrsoo/HexFlame/raw/master/preview.gif)

## Overview

HexFlame is a passion project that explores the fascinating intersection of computer graphics, procedural generation, and real-time simulation. I've always been captivated by how games and simulations recreate natural phenomena, and fire is one of the most challenging and beautiful elements to simulate!

This project uses a hexagonal grid structure to create a more organic-looking flame compared to traditional particle systems. The flame's movement, height, and intensity are all procedurally generated and can be customized in real-time.

## Features

- **Hexagonal Grid System**: Custom-built system for creating and managing hexagonal elements
- **Procedural Animation**: Dynamic flame movement using Simplex noise for natural-looking randomness
- **Custom Shader Programming**: HLSL/ShaderLab implementation for realistic flame rendering
- **Interactive Controls**: Adjust flame parameters in real-time
- **Campfire Scene**: Complete environment with a 3D campfire model

## Technical Implementation

### Hexagonal Grid

The flame is built on a custom hexagonal grid system (see `DrawHexagon.cs` and `StackHexagons.cs`). Hexagons provide a more natural-looking base for the flame compared to square grids, allowing for smoother transitions and more organic movement patterns.

Each hexagon in the grid can be individually controlled, allowing for precise manipulation of the flame's shape and behavior.

### Flame Simulation

The core of the simulation is handled by two main controllers:

1. **FlameHeightController.cs**: Manages the vertical growth and shrinking of the flame
   - Uses coroutines to smoothly transition between different heights
   - Implements randomized growth factors for natural variation
   - Controls the overall flame intensity

2. **FlameMovementController.cs**: Handles the horizontal movement and "flickering" of the flame
   - Implements procedural movement patterns
   - Creates the characteristic wavering effect of real flames

### Procedural Noise

The `SimplexNoise.cs` implementation provides the random yet coherent variation that makes the flame look natural. Unlike pure random numbers, Simplex noise creates smooth transitions between values, which is perfect for simulating natural phenomena like fire.

### Shader Implementation

The visual appearance of the flame is achieved through custom shader programming (`FlameShader.shader`):

- Implements gradient-based coloring from yellow to orange to red
- Handles transparency for realistic flame edges
- Applies distortion effects to simulate heat haze
- Manages the glow and intensity of the flame

The shader works in tandem with the `FlameMaterial.mat` to create the final visual effect.

## Project Structure

```
Assets/
├── Flame/
│   ├── Controllers/       # UI and input controllers
│   ├── Resources/         # Textures and materials
│   ├── Scripts/           # Core simulation scripts
│   │   ├── Billboard.cs
│   │   ├── DrawHexagon.cs
│   │   ├── FlameHeightController.cs
│   │   ├── FlameMovementController.cs
│   │   ├── SimplexNoise.cs
│   │   └── StackHexagons.cs
│   ├── FlameMaterial.mat  # Material using the custom shader
│   ├── FlameShader.shader # Custom HLSL shader for flame rendering
│   ├── Hexagon.prefab     # Base hexagon prefab
│   └── HexagonStack.prefab # Assembled flame structure
├── Scenes/
│   └── SampleScene.unity  # Main demo scene with campfire
└── Table/                 # Environment assets
```

## How It Works

The simulation follows these steps:

1. A hexagonal grid is generated as the base structure
2. Each hexagon's properties (height, color, position) are controlled procedurally
3. The custom shader renders each hexagon with appropriate visual effects
4. The FlameHeightController and FlameMovementController continuously update the simulation
5. Simplex noise is applied to create natural variation in the flame's behavior

## Getting Started

### Prerequisites

- Unity 2020.3 or newer
- Basic understanding of Unity's interface

### Installation

1. Clone this repository
   ```
   git clone https://github.com/zrsoo/HexFlame.git
   ```
2. Open the project in Unity
3. Open the SampleScene in Assets/Scenes
4. Press Play to see the flame in action!

### Customization

You can easily customize the flame by adjusting parameters in the Inspector:

- **Base Growth Speed**: Controls how quickly the flame grows
- **Min/Max Flame Height**: Sets the boundaries for flame height
- **Transition Duration**: How quickly the flame transitions between states
- **Color Gradient**: Modify the flame's color scheme

## Technologies Used

- **Unity**: Game engine and development environment
- **C#**: Primary programming language
- **ShaderLab/HLSL**: Custom shader programming
- **Git**: Version control

## Acknowledgments

- My thesis advisor for guidance throughout the project
- The Unity community for invaluable resources and inspiration
- Various computer graphics research papers on fire simulation techniques

## License

This project is available for educational and personal use. Feel free to explore, learn from, and build upon it!

---

Created with ❤️ and lots of ☕ by [zrsoo](https://github.com/zrsoo)
