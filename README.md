# Blazor WebAssembly and MAUI Clean Architecture Example

*This is an INCOMPLETE work-in-progress experiment.*

This example application is intented to allow a theoretical customer to create a 3D model, a "Thing", that they might wish to order as a 3D printed item.

### Introduction:

- Loose goals for this project
    - Have some fun with Blazor and MAUI!
    - Explore isolating domain components for use in WebAssembly Blazor, specifically the `Thing.Domain.SolidModeling` project. 
    - Minimize the API glue necessary to make backend calls using common contract interfaces and GRPC-Web
    - Experiment with verification/snapshot testing.
    - Experiment with implementing a simple DDD paradigm along with an application façade layer.

- Based on the clean architecture template from Steve Smith
    - https://github.com/ardalis/CleanArchitecture
- Uses Verify and BUnit for rapid implementation of tests.
    - https://github.com/VerifyTests/Verify.Blazor

### Quick Start

- WebAssembly Client and Backend API
  - Right click on the solution and "Set Startup Projects..."
  - Set both `Thing.Designer.App.Api` and `Thing.Designer.App.WebClient` to start.
  - Debug > Start Debugging

- MAUI targeting Android or WinUI 3
  - To build and run the MAUI project `Thing.Designer.App.Mobile` you will need the latest preview version of Visual Studio 2022, until MAUI is formally released.
  - Set `Thing.Designer.App.Mobile` as the startup project.
  - Under the debug button you can select the "Framework" and target either Windows or Android as the target executable.
  - iOS is untested, but can be enabled, see the comments in the csproj file.

### Notes

- The backend uses EFCore backed by SQLite, a database is automatically generated when launching the API project.
- Once launched you will need to register an account and sign in to see the "Manage Designs" page.
- GRPC from the MAUI App is not implemented, though I noticed a video from Carl Franklin might tackle this. https://www.youtube.com/watch?v=-JnvLP31Z64
