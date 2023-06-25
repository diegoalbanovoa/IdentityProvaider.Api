# IdentityProvaider

IdentityProvaider is a microservice project based on the Domain-Driven Design (DDD) architecture, providing functionalities related to identity management and authentication in an application. The project consists of several layers and components that enable secure and efficient management of users, roles, sessions, and passwords.

## Project Structure

The project follows an organized structure based on the principles of Domain-Driven Design:

- **IdentityProvaider.Api**: This folder contains the implementation of the project's API. It includes controllers, application services, and routing for the different functionalities exposed by the project.

- **IdentityProvaider.Domain**: This folder contains the domain logic of the project. It defines entities, aggregates, repositories, and interfaces related to identity management and authentication.

- **IdentityProvaider.Infraestructure**: This folder contains the implementation of the project's infrastructure layer. It includes database contexts, repository implementations, and other components related to data access and persistence.

- **IdentityProvaider.ApplicationServices**: This folder contains the application services that encapsulate the project's business logic. It implements operations and business rules related to identity management and authentication.

- **IdentityProvaider.Commands**: This folder contains the commands of the project. Commands represent actions that trigger changes or updates in the system.

- **IdentityProvaider.Queries**: This folder contains the queries of the project. Queries represent actions that retrieve information from the system without making changes.

- **IdentityProvaider.Controllers**: This folder contains the API controllers. Controllers are responsible for handling HTTP requests and responses.

- **IdentityProvaider.ValueObjects**: This folder contains the value objects used in the project. Value objects encapsulate related data that do not have their own identity, such as coordinates, creation dates, identifications, and more.

## Configuration

The project uses configuration files to adapt to different environments. The configuration files are located in the "Properties" folder and include:

- **appsettings.json**: This file contains the main configuration for the project, which applies to all environments.

- **appsettings.Development.json**: This file contains environment-specific configuration for the development environment.

## Prerequisites

Before running the project, make sure you have the following dependencies and tools installed:

- [.NET 6.0 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Running the Project

To run the project, follow these steps:

1. Clone this repository to your local machine.

2. Open a terminal and navigate to the "IdentityProvaider.Api" folder.

3. Run the following command to build and run the project:

```bash
dotnet run

