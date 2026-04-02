# Event Manager Challenge - CivicPlus

This is a technical assessment developed for the CivicPlus recruitment process. The project consists of a **.NET 10 Web API backend** and a **React (Vite) frontend**, designed to handle event management.

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
