## Frontend
1. **Path visualization**:
The path defined in the CSV file is rendered as a polyline, allowing the user to visually understand the tracked route.

2. **User input**:
The user can input X and Y coordinates and trigger a calculation request to the backend.

3. **Result rendering**:
The backend responds with the calculated offset and station values for the given point.
These results are visually represented in the UI, including the offset line and the corresponding station along the path.

#### Overall layout
![layout](./images/layout.png)

#### 1. Inputs
![inputs](./images/coordinates-inputs.png)

Two text inputs, implemented using Angular Material, are provided for the X and Y coordinates.
Both inputs include validation to ensure that only numeric values are accepted, preventing invalid requests from being sent to the backend.

Validation feedback is displayed directly in the UI, improving usability and guiding the user to provide correct input.

To send the request the user can click the Update location button or press enter while on the X or Y input.

#### 2. Indicators
![indicators](./images/indicators.png)

The indicators display the Offset and Station values calculated by the backend.
Each indicator uses the same color as its corresponding graphical element in the graph, creating a clear visual association between numeric values and their geometric representation.

This design helps the user quickly understand how the calculated values relate to the rendered path and position.

### 3. Graph
![graph](./images/graph.png)

The graph contains the following:
- Polyline path: generated from the coordinates in the input CSV file, displayed as a black dashed line.
- Path endpoints: start and end markers, represented by red points with corresponding labels.
- User input point: the coordinate provided by the user, displayed as a blue point.
- Offset line: the perpendicular line connecting the user point to the closest point on the path, displayed in blue.
- Station line: the segment representing the station position along the path, displayed in red.

All visual information is rendered using a graph built purely with SVG.
This approach was chosen to provide greater flexibility and high performance, while avoiding the overhead and limitations of external charting libraries.

Using SVG allows:
- Precise control over geometric elements (polyline, offset line, station)
- Efficient rendering, even with frequent updates
- A lightweight solution with no additional dependencies

### Responsiveness
TODO

### Limitations & Future Improvements
TODO

## Backend
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

### Project Architecture
TODO

### Offset Calculation
In order to calculate the value of the offset between a line and a given point, a vectorial approach was taken. 
Considering the following points:
- A: Start of the line segment.
- B: End of the line segment.
- P: The external point provided by the user.
The offset is defined as the shortest distance between point P and the line segment AB.

#### Closest Point Calculation

The first step is to compute the point C, which represents the closest point on the line segment AB to the external point P.

1. Construct the vectors:
- AB = B − A
- AP = P − A

2. Project vector AP onto AB using the dot product:
TODO
3. Clamp the projection factor t to the interval [0 1] to ensure that the resulting point lies within the line segment rather than on the infinite line.

4. Compute the closest point:

Offset Value
Once the closest point C is determined, the offset is calculated as the Euclidean distance between P and C:

This value represents the perpendicular distance from the input point to the nearest line segment and is used to determine which segment of the polyline is closest to the user-provided coordinate.

### Station Calculation
The station value represents the accumulated distance along the polyline from its starting point up to the position closest to the user-provided coordinate.

It is calculated as the sum of:
- The lengths of all line segments preceding the closest segment, and
- The distance from the start of the closest line segment to the computed closest point on that segment.

### Validation
TODO
### Tests
TODO

## The cross paths problem
TODO

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
