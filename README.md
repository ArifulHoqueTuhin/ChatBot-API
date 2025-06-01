
AI-Powered Real-Time Chatbot with .NET Core, SignalR & Tavily AI
Features...

Real-Time Messaging — Instant communication using SignalR ,
Tavily AI Integration — Smart responses via Tavily AI API ,
Secure Authentication — Login/registration with ASP.NET Identity + JWT ,
Chat History — Paginated & filterable message retrieval ,
Message Moderation — Edit, delete (soft), and approve messages ,
Infinite Scroll Support — Dynamically load older messages ,



API Endpoints...
Authentication ,

POST	- /api/auth/register	Register new user ,
POST	- /api/auth/login	Login and receive JWT ,


POST -	/api/chat/send	- Send a user message, get AI reply ,
GET -	/api/chat/history -	Get paginated message history ,
PUT -	/api/chat/{id} -	Edit a message ,
DELETE -	/api/chat/{id} -	Soft-delete a message ,
PATCH -	/api/chat/{id}/approve	- Approve a message 


Hub URL	Description...
/chathub	SignalR hub for live updates
