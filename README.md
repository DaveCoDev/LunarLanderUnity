# Lunar Lander Unity

Lunar Lander Unity is designed to be an environment to bridge the gap between reinforcement learning research and the implementation of various reinforcement learning algorithms in practice. Our primary objective is for the game environment to be easily demoed and destributed to a broad audience.
By developing in Unity, we hope to leverage its flexibility and ubiquity in game development and combine that with state of the art techniques in AI research for human-AI collaboration. The project is currently in development and has no reinforcement learning algorithms implemented yet. It is currently only a reimplementation from the ground-up of the Lunar Lander game, specifically inspired by [Open AI's implementation](https://www.gymlibrary.ml/environments/box2d/lunar_lander/).

## Game Installation 
### Windows
Download the release zip from [GitHub Releases](https://github.com/DaveCoDev/LunarLanderUnity/releases). Extract the zip file and run the `LunarLanderUnity.exe` executable.

### Other
Other official releases may come in the future. Other platforms can have releases created by downloading the project (following the installation instructions) and using Unity's build tools. Note that we have only tested the Windows build.

## Development
1. Download the latest [Unity Hub](https://unity3d.com/get-unity/download).
2. In Unity Hub, `Open` --> `Add project from disk` --> Select the extracted source code folder.


## Known Issues
* The `E` key cannot be used as a possible keybinding due to a [Unity bug](https://github.com/Unity-Technologies/InputSystem/pull/1485) whose fix has not been released yet.

