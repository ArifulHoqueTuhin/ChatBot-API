# ChatBot-API 
This is the backend API for the AI-Powered Real-Time Chatbot built with ASP.NET Core 8, SignalR, and Tavily AI.

##  Tech Stack
- ASP.NET Core 8 Web API
- Entity Framework Core (Code First)
- SQL Server
- ASP.NET Identity with JWT Authentication
- SignalR for real-time communication
- Tavily AI for intelligent chatbot responses

##  Features
- User Registration & Login (JWT-based)
- Real-time Chat using SignalR
- Tavily AI API Integration
- Chat history (Create, Read, Update, Delete)
- Session-based conversation tracking
- Role-based Authorization
- Secure HMAC-based message integrity

## Endpoints
POST /api/auth/register

POST /api/auth/login

GET /api/chat/session/{sessionId}

POST /api/chat/send

PUT /api/chat/edit/{id}

DELETE /api/chat/delete/{id}

