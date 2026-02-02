## Solution Approach
### Frontend Logic
1. **Path visualization**:
The path defined in the CSV file is rendered as a polyline, allowing the user to visually understand the tracked route.

2. **User input**:
The user can input X and Y coordinates and trigger a calculation request to the backend.

3. **Result rendering**:
The backend responds with the calculated offset and station values for the given point. These results of the Offset and Station are visually represented in the UI.

### Backend Logic
1. **Parse input data**:
Coordinates are read from the CSV file and mapped into a strongly typed data structure.

2. **Build line segments from coordinates**:
Consecutive coordinates are paired to form a collection of line segments representing the polyline path.

3. **Evaluate each line segment**:
For each line segment, the algorithm 
- calculates Station and offset.
- Determine the closest segment.
- The line segment with the smallest offset value is selected, as it represents the closest segment to the given point.
- This segment determines the final information returned by the API.

## ▶️ How to Run Locally
Prerequisites

Make sure you have the following installed:

- .NET SDK 8+
- Node.js 18+
- Angular CLI
- Git

### Option 1 — Run API and Frontend as a Single Unit

(.NET serves the Angular static files – production-like setup)

Navigate to the backend project:

```
cd tracker-backend/Tracker.Api
```

Restore dependencies and run the application:

```
dotnet restore
dotnet run
```


Access the application:

- API: http://localhost:5027/api
- Frontend: http://localhost:5027

⚠️ Note
When running in this mode, the Angular application is served from the .NET wwwroot folder.
Any changes to the Angular source code require rebuilding the frontend, as the build output is already configured to be copied into wwwroot.

### Option 2 — Run Frontend and Backend Separately

(Recommended for development)

This option enables hot reload and faster iteration on the frontend.

Start the backend (same as Option 1):
```
cd tracker-backend/Tracker.Api
dotnet run
```

In a new terminal, navigate to the frontend project:

```
cd tracker-frontend
```

Install dependencies and start the Angular dev server:

```
npm install
ng serve
```

Access the application:

- Frontend: http://localhost:4200
- API: http://localhost:5027/api

✅ This setup is more practical for development, as frontend changes are reflected immediately without requiring a rebuild.
