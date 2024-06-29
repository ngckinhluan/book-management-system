# Book Management System

Welcome to the Book Management System!

## Overview

This repository contains a desktop application designed for managing books. The application is built using WPF.NET and Entity Framework Core to connect to a database, demonstrating basic CRUD (Create, Read, Update, Delete) operations and a 3-tier architecture (GUI -> Service -> Repository).


## Features

- Desktop application built with WPF.NET for managing books
- Integration with Entity Framework Core for database connectivity
- Implementation of basic CRUD operations

## Screenshots

![Image](https://github.com/ngckinhluan/book-management-system/blob/main/image/Screenshot%202024-06-29%20110941.png)

## Getting Started

### Prerequisites

Ensure you have the following installed:

- [.NET Core SDK](https://dotnet.microsoft.com/download)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)

### Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/book-management-system.git
    ```
2. Navigate to the project directory:
    ```bash
    cd book-management-system
    ```
3. Restore dependencies:
    ```bash
    dotnet restore
    ```

### Configuration

1. Update the connection string in `appsettings.json`, especially the user ID and password:
    ```json
    "ConnectionStrings": {
        "DefaultConnection": "Server=your_server;Database=your_database;User Id=your_user_id;Password=your_password;"
    }
    ```
2. Update the database to reflect the current schema:
    ```bash
    dotnet ef database update
    ```

### Usage

1. Build the application:
    ```bash
    dotnet build
    ```
2. Run the application:
    ```bash
    dotnet run
    ```

## Project Structure

- **/Entities**: Contains entity classes for the database.
- **/Services**: Contains service classes for handling business logic.
- **/UI**: Contains WPF XAML files and code-behind for the user interface.

## Contributing

If you'd like to contribute, please fork the repository and use a feature branch. Pull requests are warmly welcome.

## Contact

Connect with me via email: [nkluantran2907@gmail.com](mailto:nkluantran2907@gmail.com)

## License

© 2024 Luan Tran

---

Feel free to make further adjustments based on your specific needs and preferences.
