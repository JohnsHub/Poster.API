# Poster: A Twitter‑Style Social Platform

## Overview
Poster is a scalable, full‑stack social media application modeled after Twitter. It provides authenticated users the ability to create, read, update, and delete posts, comment on and interact with others' content (likes, retweets), and manage user profiles. Designed for both performance and extensibility, Poster demonstrates best practices in modern web development.

## Key Features
- **User Authentication & Authorization**: Secure registration and login via JSON Web Tokens (JWT).
- **Post Management**: Create, edit, and delete text‑based posts in real time.
- **Interactions**: Comment, like, and retweet functionality tied to authenticated users.
- **User Profiles**: View and update profile information including bio, avatar, and location.
- **RESTful API**: Clean API design using ASP.NET Core Web API with Entity Framework Core and SQL Server.
- **Rich Front‑End**: Responsive UI built with React, Bootstrap, Axios, and React Icons.
- **Documentation & Testing**: Integrated Swagger for API exploration and Postman collection for endpoint testing.

## Technology Stack

| Layer        | Technology                                |
|--------------|-------------------------------------------|
| Backend      | .NET 9, ASP.NET Core Web API, C#          |
| Data Access  | Entity Framework Core, SQL Server         |
| Authentication | ASP.NET Core Identity, JWT Bearer       |
| Frontend     | React, React Router, Axios, Bootstrap     |
| Styling      | Bootstrap 5, custom CSS                   |
| Icons        | React Icons (FontAwesome)                 |
| Documentation| Swagger / OpenAPI, Postman                |

## Architecture

- **Backend**: Organized into Controllers, Data (DbContext), Models (domain entities & DTOs), and Services (authentication, business logic).
- **API Contracts**: DTOs for data transfer (e.g. `CreatePostDto`, `PostDto`, `CommentDto`) ensure clear boundaries and prevent over‑posting.
- **Frontend**: Functional React components under `src/components` and `src/pages`, with global state and authentication context in `src/context/AuthContext.jsx`.

## API Endpoints

| Method | Endpoint                       | Description                          |
|--------|--------------------------------|--------------------------------------|
| POST   | `/api/account/register`       | Register a new user                  |
| POST   | `/api/account/login`          | Authenticate and receive JWT         |
| GET    | `/api/posts`                  | Retrieve all posts                   |
| POST   | `/api/posts` (auth)           | Create a new post                    |
| PUT    | `/api/posts/{id}` (auth)      | Update a post                        |
| DELETE | `/api/posts/{id}` (auth)      | Delete a post                        |
| GET    | `/api/comments?postId={id}`   | Retrieve comments for a post         |
| POST   | `/api/comments` (auth)        | Create a comment                     |
| DELETE | `/api/comments/{id}` (auth)   | Delete a comment                     |
| GET    | `/api/likes?postId={id}`      | Retrieve likes for a post            |
| POST   | `/api/likes` (auth)           | Like a post                          |
| DELETE | `/api/likes/{id}` (auth)      | Remove a like                        |
| GET    | `/api/retweets?postId={id}`   | Retrieve retweets for a post         |
| POST   | `/api/retweets` (auth)        | Retweet a post                       |
| DELETE | `/api/retweets/{id}` (auth)   | Remove a retweet                     |

## Front‑End Components

- **NavigationBar**: Header with login/logout and navigation links.
- **AuthContext**: React Context for managing JWT storage and user state.
- **Home**: Displays feed of posts and new post form.
- **PostCard**: Card component for individual posts with edit, delete, and interaction controls.
- **CommentsList & NewCommentForm**: Display and create comments under each post.
- **EditPostForm**: Inline editing of post content.

## Testing

- **Swagger UI**: `https://localhost:{port}/swagger` for interactive API documentation.
- **Postman Collection**: Located in `/docs/PostmanCollection.json` for automated endpoint tests.

## License

This project is licensed under the MIT License. See [LICENSE](LICENSE) for details.

---

**Poster** demonstrates a comprehensive, production‑ready social media application with a focus on clean architecture, secure authentication, and a responsive user interface.

