# Vitrue Health Exercise

A full-stack web application for viewing employee wellness suggestions, built with a .NET 8 Web API backend and a Vue.js 3 frontend.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Node.js 18+](https://nodejs.org/)

## Running the project

### Backend

```bash
cd VitrueWebAPI/VitrueWebAPI
dotnet restore
dotnet run
```

The API will start at `http://localhost:5156`. Swagger UI is available at `http://localhost:5156/swagger` and can be used to explore and test all endpoints.

### Frontend

```bash
cd Frontend
npm install
npm run dev
```

The frontend will start at `http://localhost:5173`. Open this in your browser to view the application.

### Tests

```bash
cd VitrueWebAPI
dotnet restore
dotnet test
```

> `dotnet restore` is required before the first run to ensure all NuGet packages are downloaded and available.

## Project structure

```
/
├── VitrueWebAPI/
│   ├── VitrueWebAPI/               # .NET 8 Web API
│   │   ├── Controllers/            # API endpoints
│   │   ├── Interfaces/             # Store abstractions
│   │   ├── Models/                 # Domain models, view models, request DTOs
│   │   ├── Services/               # In-memory store implementations
│   │   └── TestData/               # Seed data loaded at startup
│   └── VitrueWebAPI.Tests/         # MSTest unit tests
│       ├── Controllers/            # Tests for SuggestionController
│       └── Services/               # Tests for InMemorySuggestionStore and InMemoryEmployeeStore
└── Frontend/
    └── src/
        ├── components/             # Reusable Vue components
        ├── services/               # API fetch calls
        ├── types/                  # TypeScript interfaces and enums
        └── views/                  # Page-level components
```

## Design decisions

- **In-memory data store with async interface** — data is seeded from JSON files at startup. All store methods are defined as async even though the in-memory implementation has no async work to do, ensuring the interface reflects what a real database-backed implementation would require. This means the in-memory store can be swapped out without changing the interface or its callers.
- **View model pattern** — the `GET /suggestion/view` endpoint resolves each suggestion's employee name server-side and returns a flattened view model, keeping the frontend simple.
