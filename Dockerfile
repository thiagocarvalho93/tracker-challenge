############################################
# 1️⃣ Frontend build (Angular)
############################################
FROM node:20-alpine AS frontend-build

WORKDIR /frontend

COPY tracker-frontend/package*.json ./
RUN npm ci

COPY tracker-frontend .

RUN npm run build -- --configuration production


############################################
# 2️⃣ Backend build (.NET)
############################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS backend-build

WORKDIR /src

COPY tracker-backend ./tracker-backend

COPY --from=frontend-build \
    /frontend/dist/tracker-frontend \
    /src/tracker-backend/TrackerApi/wwwroot

WORKDIR /src/tracker-backend/TrackerApi
RUN dotnet restore
RUN dotnet publish -c Release -o /app


############################################
# 3️⃣ Runtime (ASP.NET)
############################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app

COPY --from=backend-build /app .

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "TrackerApi.dll"]
