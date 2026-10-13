FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY TestApi.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish TestApi.csproj -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_URLS=http://+:8080
# SQLite file lives in /tmp (data is lost when the container restarts)
ENV ConnectionStrings__Default="Data Source=/tmp/testapi.db"
EXPOSE 8080
ENTRYPOINT ["dotnet", "TestApi.dll"]
