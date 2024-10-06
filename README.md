# Space Dash

Space Dash is an exciting web-based game where players navigate through space, completing challenges against the clock. Built with ASP.NET Core MVC, this game offers a thrilling experience with a space-themed interface.

## Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)

## Features

- Space-themed user interface with animated starry background
- Timed challenges to test player reflexes
- Score tracking system
- Game over screen with final score and time taken
- Responsive design for various device sizes

## Prerequisites

Before you begin, ensure you have met the following requirements:

- .NET 6.0 SDK or later
- Visual Studio 2022 or Visual Studio Code with C# extension
- Basic knowledge of ASP.NET Core MVC

## Installation

To install Space Dash, follow these steps:

1. Clone the repository:
   ```
   git clone https://github.com/cyprianndemo/SpaceDash.git
   ```
2. Navigate to the project directory:
   ```
   cd space-dash
   ```
3. Restore the NuGet packages:
   ```
   dotnet restore
   ```
4. Build the project:
   ```
   dotnet build
   ```

## Usage

To run Space Dash locally:

1. Navigate to the project directory if you haven't already.
2. Run the following command:
   ```
   dotnet run
   ```
3. Open a web browser and go to `https://localhost:5001` or `http://localhost:5000`.

## Project Structure

- `Controllers/`: Contains the MVC controllers
  - `GameController.cs`: Handles game logic and actions
  - `HomeController.cs`: Manages the home page and navigation
- `Models/`: Contains the data models
  - `GameSession.cs`: Represents a game session with score and time
- `Views/`: Contains the Razor views
  - `Game/`: Game-related views
  - `Home/`: Home page and shared views
- `wwwroot/`: Static files (CSS, JavaScript, images)

## Contributing

Contributions to Space Dash are welcome! Here's how you can contribute:

1. Fork the repository.
2. Create a new branch: `git checkout -b feature-branch-name`.
3. Make your changes and commit them: `git commit -m 'Add some feature'`.
4. Push to the original branch: `git push origin space-dash/feature-branch-name`.
5. Create the pull request.

Alternatively, see the GitHub documentation on [creating a pull request](https://help.github.com/articles/creating-a-pull-request/).

## License

This project uses the following license: [MIT License](https://opensource.org/licenses/MIT).
