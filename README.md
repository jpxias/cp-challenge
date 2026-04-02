# Event Manager Challenge - CivicPlus

This is a technical assessment developed for the CivicPlus recruitment process.

This project is built using a **Backend-for-Frontend (BFF)** architecture powered by **.NET 10.** To ensure seamless communication between our services, I implemented a custom HTTP client interceptor that automatically attaches the necessary authentication tokens to every outgoing request. This layer is designed with high resilience in mind; if a request fails due to an unexpected credential change or a token expiring earlier than its reported lifetime, the system performs a proactive, single-attempt retry. By transparently refreshing the token and re-issuing the call, we maintain a stable connection to the underlying services without interrupting the user experience.

On the frontend, we prioritize strict consistency and type safety through the use of **Orval**. Orval automatically generates all the necessary TypeScript models and handles the creation of our HTTP request hooks using **Swagger**. This "Contract First" approach eliminates the risk of manual integration errors and ensures that our client-side code is always a perfect reflection of the API schema. It significantly reduces boilerplate while keeping the entire development lifecycle synchronized and efficient.

To guarantee that the project remains portable and easy to test across different environments, the entire solution has been fully containerized. This Docker-based setup ensures that you can run the application with complete compatibility, regardless of your local operating system or specific configuration. You will find the step-by-step instructions below to get the environment up and running quickly.

---

## Tech Stack

**Frontend**

- React 18
- TypeScript
- Vite
- Material UI (MUI)
- Orval
- TanStack Query

**Backend**

- .NET 10 Web API
- Swagger
- MSTest
- FakeItEasy

**DevOps**

- Docker
- Docker Compose

---

## How to Run

### 1. Local Development (Without Containers)

**Prerequisites:**

- Node.js 22+
- .NET 10 SDK

#### Backend

```bash
cd backend/CivicPlusChallenge
dotnet restore
dotnet run
```

##### Run at: https://localhost:7190

##### Docs: http://localhost:7190/swagger/index.html

#### Frontend

```bash
cd frontend
npm install
npm run dev
```

##### Run at http://localhost:5173

### 2. Docker Setup (With Containers)

**Prerequisites:**

- Docker Desktop.

```bash
docker-compose up --build
```

##### App will run at:

##### Frontend: http://localhost:5173

##### Backend: http://localhost:5039/

##### Docs: http://localhost:5039/swagger/index.html
