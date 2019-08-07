# Air Hockey VR
A two-person demonstration game of virtual air hockey using hand gestures input between a VR user and a display player !

This is a completed game that I created in order to demonstrate my hand tracking device prototype.

# Technologies Used/Requirements
- Unity 2017.3.0f3
- Leap Motion SDK (Orion software)
- Leap Motion Experimental Multidevice Unity Module [available here](https://github.com/leapmotion/UnityModules/tree/feat-multi-device/Multidevice%20Service)
- SteamVR (Software + Unity plugin)

## Hardware Requirements
- VR Headset and setup
- 2 Hand Tracking device prototypes (Leap Motion + customized gimbal)

# Architecture
The game runs on a single scene (either **Air Hockey VR** or **Air Hockey** depending on the second player's setup using VR or not).

## Scripts
- AIPad: Trivial AI attached to the Player 2 pad in the **Air Hockey** scene in order to test the game
- GoalScript: Handles scoring
- OptionMenuObjectVer: Option Menu allowing for parameter tweaking of the hand tracking device when the hand is attached to a virtual object (in this case the hockey pad)
- PlayerPad: Handles a hockey pad when controlled by a player via hand gestures
- PuckCollision: Handles the logic of the hockey puck
- TrueMotionObjectVer: Core logic and software pipeline for the hand tracking device. This script polls hand position and velocity from the Leap Motion SDK and computes virtual world coordinates to move the hockey pads while controlling the motorized tracking device (PID controller) in order to relocate the sensor in real-time

# Installation
All game files, assets, fonts, and libraries are located in the ZIP archive file, which can be compiled directly or converted into a jar to play.

# License
MIT
