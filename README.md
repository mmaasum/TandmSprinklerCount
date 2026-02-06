# Fire Designer Sprinkler Layout API

## Overview
This .NET API calculates the optimal layout of sprinklers in a rectangular room and connects each sprinkler to the nearest water pipe. The project is designed for fire safety planning and ensures sprinklers are evenly distributed according to specified spacing requirements.

---

## Features
- Calculate the number of sprinklers that can fit in a room.
- Determine exact (x, y, z) positions of each sprinkler on the ceiling.
- Connect each sprinkler to the nearest water pipe.
---

## Room and Pipe Data

C1: (97500.01, 34000.00, 2500.00)
C2: (85647.67, 43193.61, 2500.00)
C3: (91776.75, 51095.16, 2530.00)
C4: (103629.07, 41901.55, 2530.00)

### Room Ceiling Coordinates (x, y, z)


### Available Water Pipes

Pipe 1: (98242.11, 36588.29, 3000.00) to (87970.10, 44556.09, 3000.00)
Pipe 2: (99774.38, 38563.68, 3000.00) to (89502.37, 46531.47, 3000.00)
Pipe 3: (101306.65, 40539.07, 3000.00) to (91034.63, 48507.01, 3000.00)


### Sprinkler Spacing
- Minimum 2500mm away from walls and other sprinklers.

---

## Technology Stack
- **.NET Core 6.0+**
- **C#**
- **ASP.NET Core Web API**
- **Dependency Injection for services**
- **Structured logging with ILogger**

---

## Project Structure

Clone the repository:
git clone https://github.com/mmaasum/TandmSprinklerCount.git

Call the API endpoint via browser or Postman:
GET http://localhost:5207/FireDesign

Build and run the project in Visual Studio or using CLI:
dotnet build
dotnet run

## Notes:
  The API currently works with rectangular rooms only.
  Sprinkler positions are rounded to two decimal places.
  Pipe connections are calculated using closest point on the pipe segment.

## Author
Mohammad Masum Jahangir – Senior Software Engineer
