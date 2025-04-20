# Poster API: ASP.NET Core Web API

## Overview

Poster API is the backend service powering the Poster social platform. It exposes a RESTful interface for user authentication, post management, and interactive features (comments, likes, retweets). Built with ASP.NET Core and Entity Framework, it emphasizes clean architecture, secure endpoints, and clear separation between domain models and data transfer objects.

## Key Features

- **Secure Authentication**: User registration and login with ASP.NET Core Identity and JWT bearer tokens.
- **Post CRUD**: Endpoints to create, read, update, and delete posts, with authorization limiting edits and deletions to post authors.
- **Commenting System**: Create and retrieve comments per post, with DTO projections to include author usernames.
- **Engagement**: Like and retweet endpoints, each scoped to the authenticated user.
- **DTO‑Driven Contracts**: Request and response DTOs (e.g. `CreatePostDto`, `PostDto`, `CommentDto`) enforce clear API contracts and prevent over‑posting.
- **Entity Framework Core**: Code‑first migrations keep database schema in sync with models, with explicit relationship configuration to avoid cascade issues.
- **Swagger / OpenAPI**: Interactive API documentation automatically generated for all endpoints.

## Technology Stack

| Layer          | Technology                                           |
|----------------|------------------------------------------------------|
| Framework      | ASP.NET Core 9                                       |
| Authentication | ASP.NET Core Identity, JWT Bearer                    |
| Data Access    | Entity Framework Core, SQL Server                    |
| Documentation  | Swashbuckle (Swagger)                                |
| Tools          | `dotnet ef` migrations, JSON Patch support           |

## Project Structure

```
Poster.API/
├─ Controllers/       # API controllers per resource (Account, Posts, Comments, Likes, Retweets, Users)
├─ Data/
│  └─ AppDbContext.cs  # EF Core DbContext with model configurations
├─ Models/
│  ├─ AppUser.cs       # Identity user extension
│  ├─ Post.cs          # Domain entities
│  ├─ Comment.cs
│  └─ Like.cs
│  └─ Retweet.cs
├─ Models/DTOs/       # Data transfer objects for requests and responses
├─ Program.cs         # Application startup (services, middleware)
├─ appsettings.json   # Connection strings and JWT settings
└─ Migrations/        # EF Core migrations
```

## API Endpoints

Authentication:

| Method | Route                         | Description                         |
|--------|-------------------------------|-------------------------------------|
| POST   | `/api/account/register`       | Register a new user                 |
| POST   | `/api/account/login`          | Authenticate and return JWT         |

Posts & Interactions:

| Method | Route                           | Description                          |
|--------|---------------------------------|--------------------------------------|
| GET    | `/api/posts`                    | Retrieve all posts (public)          |
| POST   | `/api/posts` (auth)             | Create a new post                    |
| PUT    | `/api/posts/{id}` (auth)        | Update an existing post              |
| DELETE | `/api/posts/{id}` (auth)        | Delete a post                        |
| GET    | `/api/comments?postId={id}`     | Retrieve comments for a given post   |
| POST   | `/api/comments` (auth)          | Create a comment                     |
| DELETE | `/api/comments/{id}` (auth)     | Delete a comment                     |
| GET    | `/api/likes?postId={id}`        | Retrieve likes for a given post      |
| POST   | `/api/likes` (auth)             | Like a post                          |
| DELETE | `/api/likes/{id}` (auth)        | Remove a like                        |
| GET    | `/api/retweets?postId={id}`     | Retrieve retweets for a given post   |
| POST   | `/api/retweets` (auth)          | Retweet a post                       |
| DELETE | `/api/retweets/{id}` (auth)     | Remove a retweet                     |
| GET    | `/api/users`                    | List all users                       |
| GET    | `/api/users/{id}`               | Retrieve user profile by ID          |
| DELETE | `/api/users/{id}` (auth)        | Delete user account (self only)      |

## Documentation

- **Swagger UI**: Navigate to `https://<host>/swagger` after startup for interactive API exploration.
- **Postman Collection**: A Postman collection is provided in the repository under `/docs/PostmanCollection.json`.

