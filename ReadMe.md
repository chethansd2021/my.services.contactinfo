# Contact Management Application

This is a contact management application built with Angular for the frontend and .NET Core 8 for the backend. The application allows users to create, read, update, and delete contacts, while ensuring data integrity through validation.

## Table of Contents
- [Setup Instructions](#setup-instructions)
- [How to Run the Application](#how-to-run-the-application)
- [Design Decisions and Application Structure](#design-decisions-and-application-structure)

## Setup Instructions

### Prerequisites

Make sure you have the following installed:
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/en/) (version 14 or later)
- [Angular CLI](https://angular.io/cli) (install globally via npm)

### Backend Setup

1. **Clone the Repository**:
    ```bash
    git clone https://github.com/yourusername/contact-management-app.git
    cd contact-management-app/backend
    ```

2. **Install Dependencies**:
    ```bash
    dotnet restore
    ```

3. **Configure JSON File Path**:
    Ensure the `contacts.json` file exists in the backend directory. If not, create an empty JSON file with the following content:
    ```json
    []
    ```

### Frontend Setup

1. **Navigate to the Frontend Directory**:
    ```bash
    cd ../frontend
    ```

2. **Install Angular Dependencies**:
    ```bash
    npm install
    ```

## How to Run the Application

### Running the Backend

1. Navigate to the backend directory (if not already there).
2. Start the .NET Core application:
    ```bash
    dotnet run
    ```
3. The API will run on `http://localhost:5000` by default.

### Running the Frontend

1. Navigate to the frontend directory (if not already there).
2. Start the Angular application:
    ```bash
    ng serve
    ```
3. The frontend will run on `http://localhost:4200` by default.

### Accessing the Application

Open your web browser and go to `http://localhost:4200` to access the contact management application.

## Design Decisions and Application Structure

### Architecture

- **Frontend**: Built using Angular with reactive forms for handling user input. The application uses Bootstrap for styling to provide a responsive design. State management is handled by RxJS.
- **Backend**: Built using .NET Core 8, utilizing a JSON file as a mock database for storing contacts. The application implements a repository pattern for data access, allowing for easy modifications to the data layer in the future.

### Key Features

- **CRUD Operations**: Users can create, read, update, and delete contacts.
- **Validation**: Each contact's fields (First Name, Last Name, Email) are validated using FluentValidation to ensure data integrity.
- **Error Handling**: Global error handling is implemented in the backend to provide appropriate error responses.

### Application Structure

- **Backend**: Contains models, repositories, and controllers.
  - `Models/Contact.cs`: Represents the contact data structure.
  - `Repositories/IContactRepository.cs`: Defines the contract for contact data access.
  - `Repositories/ContactRepository.cs`: Implements the `IContactRepository` using `JsonFileDataAccess`.
  - `Services/JsonFileDataAccess.cs`: Handles the reading and writing of the JSON file.

- **Frontend**: Contains components, services, and routing.
  - `src/app/components`: Contains components for managing contacts.
  - `src/app/services/contact.service.ts`: Manages API interactions.
  - `src/app/models/contact.model.ts`: Defines the contact model for the frontend.

## Conclusion

This contact management application provides a simple yet effective way to manage contacts while ensuring data integrity and a clean separation of concerns between frontend and backend. Feel free to contribute or enhance the application as needed!
