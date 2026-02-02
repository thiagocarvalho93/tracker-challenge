## Solution Approach
### Frontend
1. **Path visualization**:
The path defined in the CSV file is rendered as a polyline, allowing the user to visually understand the tracked route.

2. **User input**:
The user can input X and Y coordinates and trigger a calculation request to the backend.

3. **Result rendering**:
The backend responds with the calculated offset and station values for the given point.
These results are visually represented in the UI, including the offset line and the corresponding station along the path.

#### The inputs
Two text inputs, implemented using Angular Material, are provided for the X and Y coordinates.
Both inputs include validation to ensure that only numeric values are accepted, preventing invalid requests from being sent to the backend.

Validation feedback is displayed directly in the UI, improving usability and guiding the user to provide correct input.

#### The graph
All visual information is rendered using a graph built purely with SVG.
This approach was chosen to provide greater flexibility and high performance, while avoiding the overhead and limitations of external charting libraries.

Using SVG allows:
- Precise control over geometric elements (polyline, offset line, station)
- Efficient rendering, even with frequent updates
- A lightweight solution with no additional dependencies

#### Responsiveness and Scaling

The graph is fully responsive and automatically scales to fit the available viewport.

Coordinate values received from the backend are mapped to the SVG coordinate system using a normalization step, ensuring that:
- The full path is always visible regardless of screen size
- Aspect ratio is preserved
- User interactions and updates remain smooth across different devices

This approach ensures consistent visualization behavior on both desktop and smaller screens.

### Backend
1. **Receive User inputs**: The backend receives the X and Y coordinates provided by the frontend via a REST API request.

2. **Parse input data**:
The path coordinates are read from the CSV file and mapped into a strongly typed data structure, ensuring consistency and type safety.

3. **Build line segments from coordinates**:
Consecutive coordinates are paired to form a collection of line segments representing the polyline path.

4. **Evaluate each line segment**:
For each line segment, the algorithm
- Calculates the station and offset relative to the user-provided point
- Compares the computed offset against previously evaluated segments
- Identifies the segment with the smallest offset value, representing the closest segment to the point

5. **Send the response**: The calculated offset, station, and other relevant information are returned to the frontend as a structured API response.

#### Validation
#### Tests


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
